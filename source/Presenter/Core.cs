
using System.Collections.Generic;

namespace Sidecab.Presenter
{
    public class Core : Base
    {
        public Settings Settings { get; private set; }
        public Selector<Root> Root { get; private set; }


        //======================================================================
        public Core(Model.Core model)
        {
            this.model = model;
            this.model.RootListChanged += this.OnRootListChanged;

            Settings = new Settings(this.model.Settings);
            RefreshList();
        }

        //======================================================================
        ~Core()
        {
            this.model.RootListChanged -= this.OnRootListChanged;
        }

        //======================================================================
        public void SetPinnedDirectory(Directory directory)
        {
            this.model.SetPinnedDirectory(directory.ExposeModel());
            this.Root.Current = this.Root.List[this.Root.List.Count - 1];
        }


        //======================================================================
        private void OnRootListChanged()
        {
            RefreshList();
            RaisePropertyChanged(nameof(Root));
        }

        //======================================================================
        private void RefreshList()
        {
            var rootList = new List<Root>(this.model.RootList.Count);
            foreach (var r in this.model.RootList) { rootList.Add(new Root(r)); }

            Root = new Selector<Root>(rootList);
        }


        private Model.Core model;
    }
}
