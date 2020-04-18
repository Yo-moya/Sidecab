
using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Sidecab.Model
{
    //==========================================================================
    public class Directory
    {
        //----------------------------------------------------------------------
        public enum UpdateType
        {
            New,
            Add,
        }

        public delegate void ChildrenUpdateHandler(UpdateType updateType);
        public event ChildrenUpdateHandler ChildrenUpdated;

        public bool HasSomeSubdirectories { get; private set; } = false;
        public string Name { get; protected set; } = "";

        //----------------------------------------------------------------------
        public string Path
        {
            get
            {
                return (this.parentDirectory?.Path ?? "") + this.Name + "\\";
            }
        }

        //-----------------------------------------------------------------------
        public List<Directory> Subdirectories
        {
            get
            {
                lock (this.subdirectories)
                {
                    return this.subdirectories;
                }
            }

            protected set
            {
                this.subdirectories = value;
            }
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
        protected Directory()
        {
        }

        //======================================================================
        public void Open()
        {
            Process.Start("explorer.exe", Path);
        }

        //======================================================================
        public async void EnumerateSubdirectories()
        {
            var path = this.Path;
            if (path.Length == 0) return;

            //------------------------------------------------------------------
            if (Monitor.TryEnter(this.semaphoreForEnumeration)) await Task.Run(() =>
            {
                lock (this.subdirectories)
                {
                    if (this.subdirectories.Count > 0) return;
                    this.subdirectories = new List<Directory>();
                }

                //--------------------------------------------------------------
                try
                {
                    var dirInfo = new DirectoryInfo(path);
                    foreach(var subdirInfo in dirInfo.EnumerateDirectories())
                    {
                        ProcessSubdirectory(subdirInfo);
                    }
                }
                //--------------------------------------------------------------
                catch
                {
                    return;
                }
                //--------------------------------------------------------------
            });
            //------------------------------------------------------------------
        }

        //======================================================================
        private void ProcessSubdirectory(DirectoryInfo directoryInfo)
        {
            Directory subdir;

            //------------------------------------------------------------------
            try
            {
                if (directoryInfo.Attributes.HasFlag(FileAttributes.System)) return;
                subdir = new Directory(directoryInfo.FullName, this);

                subdir.HasSomeSubdirectories =
                    directoryInfo.EnumerateDirectories().GetEnumerator().MoveNext();
            }
            //------------------------------------------------------------------
            catch
            {
                return;
            }
            //------------------------------------------------------------------

            lock (this.subdirectories) { this.subdirectories.Add(subdir); }
            this.ChildrenUpdated?.Invoke(UpdateType.Add);
        }


        private Directory parentDirectory;
        private List<Directory> subdirectories = new List<Directory>();
        private object semaphoreForEnumeration = new Object();
    }
}
