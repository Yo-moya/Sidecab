
using System.ComponentModel;
using System.Collections.Generic;

namespace Sidecab.Presenter
{
    public class FileTree : Utility.ObserverableObject
    {
        public Selector<TreeRoot> RootSelector { get; } = new Selector<TreeRoot>();


        //======================================================================
        public FileTree()
        {
            this.RootSelector.PropertyChanged += OnRootChanged;
            RefreshRootSelector();

            this.RootSelector.Index = 0;
        }

        //======================================================================
        public void AddBookmark(Directory directory)
        {
            App.Core.Model.AddBookmark(directory.Model);
            RefreshRootSelector();
        }

        //======================================================================
        public void SelectLastRoot()
        {
            if (RootSelector.List.Count > 0)
            {
                RootSelector.Index = RootSelector.List.Count - 1;
            }
        }


        //======================================================================
        private void RefreshRootSelector()
        {
            var driveList = App.Core.Model.GetDriveList();
            var bookmarks = App.Core.Model.GetBookmarks();

            var rootList = new List<TreeRoot>(driveList.Count + bookmarks.Count);
            foreach (var r in driveList) { rootList.Add(new TreeRoot(r)); }
            foreach (var r in bookmarks) { rootList.Add(new TreeRoot(r)); }

            this.RootSelector.SetList(rootList);
            RaisePropertyChanged(nameof(this.RootSelector));
        }

        //======================================================================
        private void OnRootChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Selector<TreeRoot>.Current))
            {
                this.RootSelector.Current?.CollectSubdirectories();
            }
        }
    }
}
