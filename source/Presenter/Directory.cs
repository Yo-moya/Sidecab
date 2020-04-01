
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Sidecab.Presenter
{
    public class Directory : Base
    {
        public string Path { get { return _model.Path; } }
        public string Name { get { return _model.Name; } }

        //----------------------------------------------------------------------
        public ObservableCollection<Directory> Children
        {
            get
            {
                var source = _model.GetChildren();
                _children.Clear();

                foreach (var child in source)
                {
                    _children.Add(new Directory(child));
                }

                return _children;
            }
        }


        //======================================================================
        public Directory(Model.Directory model)
        {
            _model = model;
            _model.ChildrenUpdated += OnChildrenUpdated;
        }

        //======================================================================
        ~Directory()
        {
            _model.ChildrenUpdated -= OnChildrenUpdated;
        }

        //======================================================================
        public void ListSubdirectories(bool listSubSubdirectories)
        {
            _model.ListSubdirectories(listSubSubdirectories);
        }

        //======================================================================
        public void Open()
        {
            _model.Open();
        }


        //======================================================================
        void OnChildrenUpdated()
        {
            RaisePropertyChanged(nameof(Children));
        }


        protected Model.Directory _model;
        private ObservableCollection<Directory> _children = new ObservableCollection<Directory>();
    }
}
