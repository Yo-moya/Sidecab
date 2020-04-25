
using System.ComponentModel;
using System.Collections.Generic;

namespace Sidecab.Presenter
{
    public class FileTree : Utility.NoticeableVariables
    {
        public Selector<Root> RootSelector { get; private set; }


        //======================================================================
        public FileTree()
        {
            RefreshRootSelector();
            this.RootSelector.Current.CollectSubdirectories();
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
                this.RootSelector.Current.CollectSubdirectories();
            }
        }

        //======================================================================
        private void RefreshRootSelector()
        {
            var rootList = new List<Root>(App.Model.RootList.Count);
            foreach (var r in App.Model.RootList) { rootList.Add(new Root(r)); }

            this.RootSelector = new Selector<Root>(rootList);
            this.RootSelector.PropertyChanged += OnRootChanged;

            RaisePropertyChanged(nameof(this.RootSelector));
        }
    }
}
