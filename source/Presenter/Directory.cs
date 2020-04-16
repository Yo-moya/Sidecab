
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Sidecab.Presenter
{
    public class Directory : Base
    {
        public string Path { get { return this.model.Path; } }
        public string Name { get { return this.model.Name; } }

        //----------------------------------------------------------------------
        public ObservableCollection<Directory> Children
        {
            get
            {
                var source = this.model.GetChildren();
                this.children.Clear();

                foreach (var child in source)
                {
                    this.children.Add(new Directory(child));
                }

                return this.children;
            }
        }


        //======================================================================
        public Directory(Model.Directory model)
        {
            this.model = model;
            this.model.ChildrenUpdated += this.OnChildrenUpdated;
        }

        //======================================================================
        ~Directory()
        {
            this.model.ChildrenUpdated -= this.OnChildrenUpdated;
        }

        //======================================================================
        public void EnumerateSubdirectories()
        {
            this.model.EnumerateSubdirectories();
        }

        //======================================================================
        public void Open()
        {
            this.model.Open();
        }

        //======================================================================
        public Model.Directory ExposeModel()
        {
            return this.model;
        }


        //======================================================================
        private void OnChildrenUpdated()
        {
            RaisePropertyChanged(nameof(this.Children));
        }


        protected Model.Directory model;
        private ObservableCollection<Directory> children = new ObservableCollection<Directory>();
    }
}
