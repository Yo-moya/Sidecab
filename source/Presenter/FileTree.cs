
using System.ComponentModel;
using System.Collections.Generic;

namespace Sidecab.Presenter
{
    public class FileTree : Base
    {
        public Selector<Root> RootSelector { get; private set; }


        //======================================================================
        public FileTree(Model.Core model)
        {
            App.Model.RootListChanged += RefreshRoot;

            RefreshRoot();
            RootSelector.Current.CollectSubdirectories();
        }

        //======================================================================
        ~FileTree()
        {
            App.Model.RootListChanged -= RefreshRoot;
        }

        //======================================================================
        public void SetRootDirectory(Directory directory)
        {
            App.Model.SetRootDirectory(directory.ExposeModel());

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
                RootSelector.Current.CollectSubdirectories();
            }
        }

        //======================================================================
        private void RefreshRoot()
        {
            var rootList = new List<Root>(this.model.RootList.Count);
            foreach (var r in this.model.RootList) { rootList.Add(new Root(r)); }

            this.RootSelector = new Selector<Root>(rootList);
            this.RootSelector.PropertyChanged += OnRootChanged;

            RaisePropertyChanged(nameof(RootSelector));
        }


        private Model.Core model;
    }
}
