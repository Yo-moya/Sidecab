
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

            Path = path;
            Name = path.Substring(location.Length);

            if (Name.StartsWith('\\')) { Name = Name.Substring(1); }
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

            //------------------------------------------------------------------
            try
            {
                Monitor.Enter(_subdirectories);
                children = new List<Directory>(_subdirectories);
            }
            //------------------------------------------------------------------
            finally
            {
                Monitor.Exit(_subdirectories);
            }
            //------------------------------------------------------------------

            return children;
        }

        //======================================================================
        public async void ListSubdirectories(bool listSubSubdirectories = false)
        {
            await _semaphore.WaitAsync();

            //------------------------------------------------------------------
            try
            {
                EnumerateSubdirectories(listSubSubdirectories);
            }
            //------------------------------------------------------------------
            finally
            {
                _semaphore.Release();
            }
            //------------------------------------------------------------------
        }

        //======================================================================
        private async void EnumerateSubdirectories(bool listSubSubdirectories)
        {
            await Task.Run(() =>
            {
                //--------------------------------------------------------------
                try
                {
                    var dirInfo = new System.IO.DirectoryInfo(Path);
                    var subdirInfoList = dirInfo.GetDirectories();
                    _subdirectories = new List<Directory>(subdirInfoList.Length);

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
                bool isLocked = false;

                //----------------------------------------------------------
                try
                {
                    var subdir = new Directory(dirInfo.FullName);
                    subdir._parent = this;

                    Monitor.Enter(_subdirectories, ref isLocked);
                    if (listSubSubdirectories) { subdir.ListSubdirectories(); }
                    _subdirectories.Add(subdir);
                }
                //----------------------------------------------------------
                catch
                {
                }
                //----------------------------------------------------------
                finally
                {
                    if (isLocked) { Monitor.Exit(_subdirectories); }
                    ChildrenUpdated?.Invoke();
                }
                //----------------------------------------------------------
            }
        }


        private Directory _parent;
        private SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private List<Directory> _subdirectories = new List<Directory>();
    }
}
