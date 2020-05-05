
using System.ComponentModel;
using System.Collections.Generic;

namespace Sidecab.Presenter
{
    public class FileTree : Utility.ObserverableObject
    {
        public Selector<Root> RootSelector { get; } = new Selector<Root>();


        //======================================================================
        public FileTree()
        {
            this.RootSelector.PropertyChanged += OnRootChanged;
            RefreshRootSelector();

            this.RootSelector.Index = 0;
        }

        //======================================================================
        public void RefreshRootSelector()
        {
            var rootList = new List<Root>(App.Model.RootList.Count);
            foreach (var r in App.Model.RootList) { rootList.Add(new Root(r)); }

            this.RootSelector.SetList(rootList);
            RaisePropertyChanged(nameof(this.RootSelector));
        }

        //======================================================================
        public void SetRootDirectory(Directory directory)
        {
            App.Model.SetRootDirectory(directory.ExposeModel());
            RefreshRootSelector();

            //------------------------------------------------------------------
            var newRootPath = directory.Path;
            foreach (var r in this.RootSelector.List)
            {
                if (r.Path == newRootPath)
                {
                    this.RootSelector.Current = r;
                    return;
                }
            }
            //------------------------------------------------------------------
        }


        //======================================================================
        private void OnRootChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Selector<Root>.Current))
            {
                this.RootSelector.Current?.CollectSubdirectories();
            }
        }
    }
}
