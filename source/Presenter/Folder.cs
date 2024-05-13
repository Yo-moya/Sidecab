
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections.ObjectModel;

namespace Sidecab.Presenter
{
    public class Folder : ObservableObject
    {
        public string Name => Model?.Name ?? string.Empty;
        public string Path => Model?.GetFullPath() ?? string.Empty;

        public bool HasSubFolders => Model?.HasSubFolders ?? false;

        //----------------------------------------------------------------------
        public ObservableCollection<Folder> SubFolders =>
            (HasSubFolders && (Model.SubFolders.Count == 0))
                ? new ObservableCollection<Folder> { new Folder() }
                : _subFolders;

        //----------------------------------------------------------------------
        public double FontSize
        {
            get
            {
                var fontSize = App.Settings.FolderNameFontSize;
                var fontSizeLarge = App.Settings.FolderNameFontSizeLarge;
                var scale = Model?.GetFreshnessScale() ?? 0;

                return (fontSizeLarge - fontSize) * scale + fontSize;
            }
        }


        protected Model.Folder Model { get; private init; }

        private ObservableCollection<Folder> _subFolders;
        private Stopwatch _animationDuration;


        //----------------------------------------------------------------------
        public Folder()
        {
            // Initialize as a dummy
        }

        //----------------------------------------------------------------------
        public Folder(Model.Folder model)
        {
            Model = model;

            Model.FreshnessUpdated += OnFontSizeUpdated;
            Model.ChildrenUpdated  += OnChildrenUpdated;
        }

        //----------------------------------------------------------------------
        protected Folder(Folder other) : this(other.Model)
        {
        }


        //----------------------------------------------------------------------
        public void CollectSubFolders()
        {
            Model.CollectSubFoldersAsync(true);
        }

        //----------------------------------------------------------------------
        public void Open()
        {
            Model.Open();
        }

        //----------------------------------------------------------------------
        public void CopyPath()
        {
            Model.CopyPath();
        }


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

                    _subFolders = new ObservableCollection<Folder>();
                    RaisePropertyChanged(nameof(SubFolders));
                    break;

                case Sidecab.Model.Folder.UpdateType.Growing :

                    AddSubFolders();
                    RaisePropertyChanged(nameof(SubFolders));
                    break;

                case Sidecab.Model.Folder.UpdateType.Finished :

                    _animationDuration = null;
                    break;
            }
        }

        //----------------------------------------------------------------------
        private void AddSubFolders()
        {
            if (_animationDuration is null)
            {
                _animationDuration = new Stopwatch();
                _animationDuration.Start();
            }

            var actualCount = Model.SubFolders.Count;
            if (actualCount > _subFolders.Count)
            {
                for (int i = _subFolders.Count; i < actualCount; i++)
                {
                    AddSubdirectory(new(Model.SubFolders[i]));
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
        private void AddSubdirectory(Folder folder)
        {
            for (int i = 0; i < _subFolders.Count; i++)
            {
                if (StrCmpLogicalW(_subFolders[i].Name, folder.Name) > 0)
                {
                    App.Current.Dispatcher.Invoke(() => _subFolders.Insert(i, folder));
                    return;
                }
            }

            App.Current.Dispatcher.Invoke(() => _subFolders.Add(folder));
        }
    }
}
