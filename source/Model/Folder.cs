
using System.IO;
using System.Windows;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;

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


        public Folder? Parent { get; private init; } = null;
        public string Name { get; protected init; } = string.Empty;

        //----------------------------------------------------------------------
        public bool HasSubFolders
        {
            get
            {
                // Returns true if it contains at least one sub folder
                foreach (var _ in SubFolders) return true;
                return false;
            }
        }

        //----------------------------------------------------------------------
        public IEnumerable<Folder> SubFolders
        {
            get
            {
                DirectoryInfo info = new(FetchFullPath());
                IEnumerator<DirectoryInfo> enumerator;

                try
                {
                    enumerator = info.EnumerateDirectories().GetEnumerator();
                }
                catch
                {
                    yield break;
                }

                while (enumerator.MoveNext())
                {
                    // We must put yield-return outside of try-block
                    yield return new(_tree, enumerator.Current, this);
                }
            }
        }


        private readonly FolderTree _tree;


        //----------------------------------------------------------------------
        public Folder(FolderTree tree)
        {
            _tree = tree;
        }

        //----------------------------------------------------------------------
        public Folder(FolderTree tree, DirectoryInfo info, Folder parent) : this(tree)
        {
            Name = info.Name;
            Parent = parent;
        }


        //----------------------------------------------------------------------
        public void Open()
        {
            Process.Start("explorer.exe", FetchFullPath());
            _tree.NotifyFolder(this);
        }

        //----------------------------------------------------------------------
        public void CopyPath()
        {
            Clipboard.SetText(FetchFullPath());
        }

        //----------------------------------------------------------------------
        public string FetchFullPath()
        {
            StringBuilder stringBuilder = new();
            CollectFolderNames(stringBuilder);
            return stringBuilder.ToString();
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
        private void CollectFolderNames(StringBuilder stringBuilder)
        {
            Parent?.CollectFolderNames(stringBuilder);

            stringBuilder.Append(Name);
            stringBuilder.Append('\\');
        }
    }
}
