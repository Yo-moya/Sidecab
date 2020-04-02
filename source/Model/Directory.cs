
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

        public string Path { get; protected set; } = "";
        public string Name { get; protected set; } = "";


        //======================================================================
        protected Directory()
        {
        }

        //======================================================================
        public Directory(string path)
        {
            var location = System.IO.Path.GetDirectoryName(path);
            if (location == null) { location = path; }

            this.Path = path;
            this.Name = path.Substring(location.Length);

            if (this.Name.StartsWith('\\'))
            {
                this.Name = this.Name.Substring(1);
            }
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
        public void ListSubdirectories(bool listSubSubdirectories = false)
        {
            if ((Path.Length > 0) && Monitor.TryEnter(listingSubdirectories))
            {
                EnumerateSubdirectories(listSubSubdirectories);
            }
        }

        //======================================================================
        private async void EnumerateSubdirectories(bool listSubSubdirectories)
        {
            await Task.Run(() =>
            {
                //--------------------------------------------------------------
                try
                {
                    var dirInfo = new System.IO.DirectoryInfo(this.Path);
                    var subdirInfoList = dirInfo.GetDirectories();
                    this.subdirectories = new List<Directory>(subdirInfoList.Length);

                    BuildSubdirectoryList(subdirInfoList, listSubSubdirectories);
                }
                //--------------------------------------------------------------
                catch
                {
                }
                //--------------------------------------------------------------
            });
        }

        //======================================================================
        private void BuildSubdirectoryList(
            DirectoryInfo[] directoryInfoSource, bool listSubSubdirectories)
        {
            foreach (var dirInfo in directoryInfoSource)
            {
                if (dirInfo.Attributes.HasFlag(FileAttributes.System)) continue;

                //----------------------------------------------------------
                lock (this.subdirectories)
                {
                    var subdir = new Directory(dirInfo.FullName);
                    subdir.parent = this;

                    if (listSubSubdirectories) { subdir.ListSubdirectories(); }
                    this.subdirectories.Add(subdir);
                }
                //----------------------------------------------------------

                this.ChildrenUpdated?.Invoke();
            }
        }


        private Directory parent;
        private object listingSubdirectories = new Object();
        private List<Directory> subdirectories = new List<Directory>();
    }
}
