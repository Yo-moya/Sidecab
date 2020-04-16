
using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Sidecab.Model
{
    public class Directory
    {
        public event Action ChildrenUpdated;

        public string Name { get; protected set; } = "";
        public string Path
        {
            get
            {
                return (this.parentDirectory?.Path ?? "") + this.Name + "\\";
            }
        }


        //======================================================================
        protected Directory()
        {
        }

        //======================================================================
        public Directory(string path, Directory parent = null)
        {
            var location = System.IO.Path.GetDirectoryName(path);

            this.Name = (location != null) ? path.Substring(location.Length) : path;
            if (this.Name.StartsWith("\\")) { this.Name = this.Name.Substring(1); }

            this.parentDirectory = parent;
        }

        //======================================================================
        public void Open()
        {
            Process.Start("explorer.exe", Path);
        }

        //======================================================================
        public List<Directory> GetChildren()
        {
            List<Directory> children = null;

            lock (this.subdirectories)
            {
                children = new List<Directory>(this.subdirectories);
            }

            return children;
        }

        //======================================================================
        public void SetChildren(List<Directory> source)
        {
            lock (this.subdirectories)
            {
                this.subdirectories = source;
            }
        }

        //======================================================================
        public async void EnumerateSubdirectories()
        {
            var path = this.Path;

            if ((path.Length > 0) && Monitor.TryEnter(semaphoreForListing))
            {
                await Task.Run(() =>
                {
                    var info = new DirectoryInfo(path);
                    UpdateSubdirectoryList(info.GetDirectories());
                });
            }
        }

        //======================================================================
        private void UpdateSubdirectoryList(DirectoryInfo[] subdirectoryInfo)
        {
            //------------------------------------------------------------------
            lock (this.subdirectories)
            {
                this.subdirectories = new List<Directory>(subdirectoryInfo.Length);
            }
            //------------------------------------------------------------------

            foreach (var info in subdirectoryInfo)
            {
                //--------------------------------------------------------------
                try
                {
                    if (info.Attributes.HasFlag(FileAttributes.System)  == false)
                    {
                        lock (this.subdirectories)
                        {
                            var subdir = new Directory(info.FullName, this);
                            this.subdirectories.Add(subdir);
                        }

                        this.ChildrenUpdated?.Invoke();
                    }
                }
                //--------------------------------------------------------------
                catch
                {
                }
                //--------------------------------------------------------------
            }
        }


        private Directory parentDirectory;
        private List<Directory> subdirectories = new List<Directory>();
        private object semaphoreForListing = new Object();
    }
}
