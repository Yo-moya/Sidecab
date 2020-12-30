
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
            App.Model.AddBookmark(directory.Model);
            RefreshRootSelector();

            this.RootSelector.Current = new TreeRoot(directory.Model);
        }


        //======================================================================
        private void RefreshRootSelector()
        {
            var driveList = App.Model.GetDriveList();
            var bookmarks = App.Model.GetBookmarks();

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
