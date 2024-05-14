
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections.ObjectModel;

namespace Sidecab.Presenter
{
    public class Folder : ObservableObject
    {
        public string Name => Model.Name;
        public string Path => Model.GetFullPath();

        public bool HasSubFolders => Model.HasSubFolders;

        //----------------------------------------------------------------------
        public ObservableCollection<Folder> SubFolders =>
            (HasSubFolders && (Model.SubFolders.Count == 0))
                ? _dummyList : _subFolders;

        //----------------------------------------------------------------------
        public double FontSize
        {
            get
            {
                var fontSize = App.Settings.FolderNameFontSize;
                var fontSizeMax = App.Settings.FolderNameFontSizeMax;
                var scale = Model?.GetFreshnessScale() ?? 0;

                return (fontSizeMax - fontSize) * scale + fontSize;
            }
        }


        protected Model.Folder Model { get; private init; }

        //----------------------------------------------------------------------
        // A list used if the folder has children, but haven't collected yet.
        // It makes a ListViewItem like "[+] Foo"
        //----------------------------------------------------------------------
        private static readonly ObservableCollection<Folder> _dummyList = [new()];

        private ObservableCollection<Folder> _subFolders = [];
        private Stopwatch? _animationDuration;


        //----------------------------------------------------------------------
        public Folder(Model.Folder model)
        {
            Model = model;

            Model.FreshnessUpdated += OnFontSizeUpdated;
            Model.ChildrenUpdated  += OnChildrenUpdated;
        }

        //----------------------------------------------------------------------
        protected Folder(Folder other) : this(other.Model) {}

        //----------------------------------------------------------------------
        private Folder()
        {
            Model = new(new());
        }


        //----------------------------------------------------------------------
        public void CollectSubFolders()
        {
            Model.CollectSubFoldersAsync(true);
        }

        //----------------------------------------------------------------------
        public void Open() => Model.Open();
        public void CopyPath() => Model.CopyPath();


        //----------------------------------------------------------------------
        private void OnFontSizeUpdated()
        {
            RaisePropertyChanged(nameof(FontSize));
        }

        //----------------------------------------------------------------------
        private void OnChildrenUpdated(Model.Folder.UpdateType updateType)
        {
            switch (updateType)
            {
                case Sidecab.Model.Folder.UpdateType.Initialize :
                {
                    _subFolders = [];
                    RaisePropertyChanged(nameof(SubFolders));
                    break;
                }
                case Sidecab.Model.Folder.UpdateType.Growing :
                {
                    RefreshSubFolders();
                    break;
                }
                case Sidecab.Model.Folder.UpdateType.Finished :
                {
                    _animationDuration = null;
                    break;
                }
            }
        }

        //----------------------------------------------------------------------
        private void RefreshSubFolders()
        {
            if (_animationDuration is null)
            {
                _animationDuration = new Stopwatch();
                _animationDuration.Start();
            }

            var actualCount = Model.SubFolders.Count;
            if (actualCount > SubFolders.Count)
            {
                for (int i = SubFolders.Count; i < actualCount; i++)
                {
                    AddSubFolder(new(Model.SubFolders[i]));
                }
            }

            // To keep step with the expanding animation of the tree-view
            var delay = Math.Max(0, 16 - _animationDuration.ElapsedMilliseconds);
            System.Threading.Thread.Sleep((int)delay);

            _animationDuration.Reset();
            _animationDuration.Start();
        }


        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        private static extern int StrCmpLogicalW(string x, string y);

        //----------------------------------------------------------------------
        private void AddSubFolder(Folder folder)
        {
            for (int i = 0; i < SubFolders.Count; i++)
            {
                if (StrCmpLogicalW(SubFolders[i].Name, folder.Name) > 0)
                {
                    App.Current.Dispatcher.Invoke(() => SubFolders.Insert(i, folder));
                    return;
                }
            }

            App.Current.Dispatcher.Invoke(() => SubFolders.Add(folder));
        }
    }
}
