
using System;
using System.IO;
using System.Windows;
using System.Diagnostics;
using System.Collections.Generic;
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
            Initialize,
            Growing   ,
            Finished  ,
        }

        public delegate void ChildrenUpdateHandler(UpdateType updateType);
        public event ChildrenUpdateHandler ChildrenUpdated;

        public Directory ParentDirectory  { get; private set; }
        public bool HasSomeSubdirectories { get; private set; } = false;
        public string Name { get; protected set; } = "";

        //----------------------------------------------------------------------
        public string Path
        {
            get
            {
                return (this.ParentDirectory?.Path ?? "") + this.Name + "\\";
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
        public Directory(DirectoryInfo info, Directory parent = null)
        {
            var path = info.FullName;

            if (path.EndsWith("\\")) { path = path.Substring(0, path.Length - 1); }
            var location = System.IO.Path.GetDirectoryName(path);

            this.Name = (location != null) ? path.Substring(location.Length) : path;
            if (this.Name.StartsWith("\\")) { this.Name = this.Name.Substring(1); }

            this.HasSomeSubdirectories =
                info.EnumerateDirectories().GetEnumerator().MoveNext();

            this.ParentDirectory = parent;
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
        public void CopyPath()
        {
            Clipboard.SetText(this.Path);
        }

        //======================================================================
        public async void CollectSubdirectories(bool force)
        {
            var path = this.Path;
            if (path.Length == 0) return;

            //------------------------------------------------------------------
            if (Monitor.TryEnter(this.semaphoreForEnumeration)) await Task.Run(() =>
            {
                lock (this.subdirectories)
                {
                    if ((force == false) && (this.subdirectories.Count > 0)) return;
                    this.subdirectories = new List<Directory>();
                }

                this.ChildrenUpdated?.Invoke(UpdateType.Initialize);

                var dirInfo = new DirectoryInfo(path);
                foreach(var subdirInfo in dirInfo.EnumerateDirectories())
                {
                    ProcessSubdirectory(subdirInfo);
                }

                this.ChildrenUpdated?.Invoke(UpdateType.Finished);
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
                subdir = new Directory(directoryInfo, this);
            }
            //------------------------------------------------------------------
            catch
            {
                return;
            }
            //------------------------------------------------------------------

            lock (this.subdirectories) { this.subdirectories.Add(subdir); }
            this.ChildrenUpdated?.Invoke(UpdateType.Growing);
        }


        private List<Directory> subdirectories = new List<Directory>();
        private object semaphoreForEnumeration = new Object();
    }
}
