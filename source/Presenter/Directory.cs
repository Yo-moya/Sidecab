
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Sidecab.Presenter
{
    public class Directory : Base
    {
        public string Path { get { return this.model.Path; } }
        public bool HasSomeSubdirectories { get { return this.model.HasSomeSubdirectories; } }

        //----------------------------------------------------------------------
        public string Name
        {
            get
            {
                return this.model?.Name ?? "";
            }
        }

        //----------------------------------------------------------------------
        public ObservableCollection<Directory> Subdirectories
        {
            get
            {
                if (this.model?.HasSomeSubdirectories ?? false)
                {
                    if (this.model.Subdirectories.Count == 0)
                    {
                        return new ObservableCollection<Directory> { new Directory() };
                    }
                }

                return this.subdirectories;
            }
        }


        //======================================================================
        public Directory()
        {
            // Initialize as a dummy
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
            if (this.model != null)
            {
                this.model.ChildrenUpdated -= this.OnChildrenUpdated;
            }
        }

        //======================================================================
        public void CollectSubdirectories()
        {
            this.subdirectories = null;
            this.model.CollectSubdirectories(true);
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
        private void OnChildrenUpdated(Model.Directory.UpdateType updateType)
        {
            bool raisePropertyChange = false;
            var source = new List<Model.Directory>(model.Subdirectories);

            if (this.subdirectories == null)
            {
                this.subdirectories = new ObservableCollection<Directory>();
                raisePropertyChange = true;
            }

            switch (updateType)
            {
                //--------------------------------------------------------------
                case Model.Directory.UpdateType.New :

                    App.Current.Dispatcher.Invoke(() =>
                    {
                        foreach (var s in source)
                        {
                            this.subdirectories.Add(new Directory(s));
                        }
                    });

                    break;
                //--------------------------------------------------------------
                case Model.Directory.UpdateType.Add :

                    if (source.Count > this.subdirectories.Count)
                    {
                        App.Current.Dispatcher.Invoke(() =>
                        {
                            for (int i = this.subdirectories.Count; i < source.Count; i++)
                            {
                                this.subdirectories.Add(new Directory(source[i]));
                            }
                        });
                    }

                    // To keep step with the expanding animation of the treeview
                    System.Threading.Thread.Sleep(16);
                    break;
                //--------------------------------------------------------------
            }

            if (raisePropertyChange)
            {
                RaisePropertyChanged(nameof(Subdirectories));
            }
        }


        protected Model.Directory model;
        private ObservableCollection<Directory> subdirectories;
    }
}
