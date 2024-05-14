
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
        public event FreshnessUpdateHandler? FreshnessUpdated;

        public delegate void ChildrenUpdateHandler(UpdateType updateType);
        public event ChildrenUpdateHandler? ChildrenUpdated;


        public Folder? Parent { get; private init; } = null;
        public bool HasSubFolders { get; private init; } = false;
        public List<Folder> SubFolders { get; } = [];
        public string Name { get; protected init; } = string.Empty;


        private readonly FolderTree _tree;
        private readonly object _semaphoreForEnumeration = new();


        //----------------------------------------------------------------------
        public Folder(FolderTree tree)
        {
            _tree = tree;
        }

        //----------------------------------------------------------------------
        public Folder(FolderTree tree, DirectoryInfo info, Folder parent) : this(tree)
        {
            Name = ExtractNameFrom(info);
            HasSubFolders = info.EnumerateDirectories().GetEnumerator().MoveNext();
            Parent = parent;
        }


        //----------------------------------------------------------------------
        public void Open()
        {
            Process.Start("explorer.exe", GetFullPath());
            _tree.NotifyFolder(this);
        }

        //----------------------------------------------------------------------
        public void CopyPath()
        {
            Clipboard.SetText(GetFullPath());
        }

        //----------------------------------------------------------------------
        public string GetFullPath()
        {
            return (Parent?.GetFullPath() ?? string.Empty) + Name + "\\";
        }

        //----------------------------------------------------------------------
        public double GetFreshnessScale()
        {
            return _tree.GetFolderFreshnessScaleOf(this);
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
        private static string ExtractNameFrom(DirectoryInfo info)
        {
            var fullPath = Path.TrimEndingDirectorySeparator(info.FullName);
            var location = Path.GetDirectoryName(fullPath);

            return (location is null) ? fullPath : fullPath[location.Length..];
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
                        Folder folder = new(_tree, child, this);
                        lock(SubFolders) { SubFolders.Add(folder); }
                    }
                    catch (UnauthorizedAccessException)
                    {
                        continue;// skip if access denied
                    }

                    ChildrenUpdated?.Invoke(UpdateType.Growing);
                }
            }
        }
    }
}
