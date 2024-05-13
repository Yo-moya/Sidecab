
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
    public class Folder
    {
        //======================================================================
        public enum UpdateType
        {
            Initialize,
            Growing   ,
            Finished  ,
        }


        public delegate void FreshnessUpdateHandler();
        public event FreshnessUpdateHandler FreshnessUpdated;

        public delegate void ChildrenUpdateHandler(Folder.UpdateType updateType);
        public event ChildrenUpdateHandler ChildrenUpdated;


        public Folder Parent { get; private set; }
        public bool HasSubFolders { get; private set; } = false;
        public List<Folder> SubFolders { get; } = [];
        public string Name { get; protected set; } = "";


        private readonly object _semaphoreForEnumeration = new();


        //----------------------------------------------------------------------
        public Folder(DirectoryInfo info, Folder parent = null)
        {
            var fullPath = Path.TrimEndingDirectorySeparator(info.FullName);
            var location = Path.GetDirectoryName(fullPath);

            Name = (location is null) ? fullPath : fullPath.Substring(location.Length);

            HasSubFolders = info.EnumerateDirectories().GetEnumerator().MoveNext();
            Parent = parent;
        }

        //----------------------------------------------------------------------
        protected Folder()
        {
        }

        //----------------------------------------------------------------------
        public void Open()
        {
            Process.Start("explorer.exe", GetFullPath());
            FolderTree.Instance.NotifyFolder(this);
        }

        //----------------------------------------------------------------------
        public void CopyPath()
        {
            Clipboard.SetText(GetFullPath());
        }

        //----------------------------------------------------------------------
        public string GetFullPath()
        {
            return (Parent?.GetFullPath() ?? "") + Name + "\\";
        }

        //----------------------------------------------------------------------
        public double GetFreshnessScale()
        {
            return FolderTree.Instance.GetFolderFreshnessScale(this);
        }

        //----------------------------------------------------------------------
        public void InvokeFreshnessUpdateEvent()
        {
            FreshnessUpdated?.Invoke();
        }

        //----------------------------------------------------------------------
        public async void CollectSubFoldersAsync(bool force)
        {
            var path = GetFullPath();
            if (path.Length == 0) return;

            if (Monitor.TryEnter(_semaphoreForEnumeration)) await Task.Run(() =>
            {
                lock(SubFolders)
                {
                    if ((force == false) && (SubFolders.Count > 0)) return;
                    SubFolders.Clear();
                }

                ChildrenUpdated?.Invoke(UpdateType.Initialize);
                CollectSubFoldersOf(new(path));// call "growing" event inside it

                ChildrenUpdated?.Invoke(UpdateType.Finished);
            });
        }


        //----------------------------------------------------------------------
        private void CollectSubFoldersOf(DirectoryInfo info)
        {
            foreach (var child in info.EnumerateDirectories())
            {
                if (child.Attributes.HasFlag(FileAttributes.System) == false)
                {
                    try
                    {
                        Folder folder = new(child, this);
                        lock(SubFolders) { SubFolders.Add(folder); }
                    }
                    catch (UnauthorizedAccessException)
                    {
                        return;// skip if access denied
                    }

                    ChildrenUpdated?.Invoke(UpdateType.Growing);
                }
            }
        }
    }
}
