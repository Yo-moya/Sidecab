
using System.ComponentModel;
using System.Collections.Generic;

namespace Sidecab.Presenter
{
    public class FolderTree : ObservableObject
    {
        public Selector<FolderRoot> RootSelector { get; } = new();


        private Model.FolderTree _model = new();
        private List<Folder> _bookmarks = [];


        //----------------------------------------------------------------------
        public FolderTree()
        {
            RootSelector.PropertyChanged += OnRootChanged;
            RefreshRootSelector();

            RootSelector.Index = 0;
        }

        //----------------------------------------------------------------------
        public void AddBookmark(Folder folder)
        {
            _bookmarks.Add(folder);
            RefreshRootSelector();
        }

        //----------------------------------------------------------------------
        public void SelectLastRoot()
        {
            if (RootSelector.List.Count > 0)
            {
                RootSelector.Index = RootSelector.List.Count - 1;
            }
        }


        //----------------------------------------------------------------------
        private void RefreshRootSelector()
        {
            var driveList = _model.GetDriveList();
            var rootList = new List<FolderRoot>(driveList.Count + _bookmarks.Count);

            foreach (var item in  driveList) { rootList.Add(new FolderRoot(item)); }
            foreach (var item in _bookmarks) { rootList.Add(new FolderRoot(item)); }

            RootSelector.SetList(rootList);
            RaisePropertyChanged(nameof(RootSelector));
        }

        //----------------------------------------------------------------------
        private void OnRootChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Selector<FolderRoot>.Current))
            {
                RootSelector.Current?.CollectSubFolders();
            }
        }
    }
}
