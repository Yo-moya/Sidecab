
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections.ObjectModel;

namespace Sidecab.Presenter
{
    public class Directory : Utility.ObserverableObject
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

        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        private static extern int StrCmpLogicalW(string x, string y);


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
            this.model.CollectSubdirectories(true);
        }

        //======================================================================
        public void Open()
        {
            this.model.Open();
        }

        //======================================================================
        public void CopyPath()
        {
            this.model.CopyPath();
        }

        //======================================================================
        public Model.Directory ExposeModel()
        {
            return this.model;
        }


        //======================================================================
        private void OnChildrenUpdated(Model.Directory.UpdateType updateType)
        {
            switch (updateType)
            {
                //--------------------------------------------------------------
                case Model.Directory.UpdateType.Initialize :

                    this.subdirectories = new ObservableCollection<Directory>();
                    RaisePropertyChanged(nameof(Subdirectories));
                    break;
                //--------------------------------------------------------------
                case Model.Directory.UpdateType.Growing :

                    AddSubdirectories();
                    RaisePropertyChanged(nameof(Subdirectories));
                    break;
                //--------------------------------------------------------------
                case Model.Directory.UpdateType.Finished :

                    this.duration = null;
                    break;
                //--------------------------------------------------------------
            }
        }

        //======================================================================
        private void AddSubdirectories()
        {
            if (this.duration == null)
            {
                this.duration = new Stopwatch();
                this.duration.Start();
            }

            //------------------------------------------------------------------
            var modelCouunt = model.Subdirectories.Count;
            if (modelCouunt >  this.subdirectories.Count)
            {
                for (int i = this.subdirectories.Count; i < modelCouunt; i++)
                {
                    AddSubdirectory(new Directory(model.Subdirectories[i]));
                }
            }
            //------------------------------------------------------------------

            // To keep step with the expanding animation of the treeview
            var delay = Math.Max(0, 16 - this.duration.ElapsedMilliseconds);
            System.Threading.Thread.Sleep((int)delay);

            this.duration.Reset();
            this.duration.Start();
        }

        //======================================================================
        private void AddSubdirectory(Directory directory)
        {
            for (int i = 0; i < this.subdirectories.Count; i++)
            {
                var cmp = StrCmpLogicalW(this.subdirectories[i].Name, directory.Name);
                if (cmp > 0)
                {
                    App.Current.Dispatcher.Invoke(() => this.subdirectories.Insert(i, directory));
                    return;
                }
            }

            App.Current.Dispatcher.Invoke(() => this.subdirectories.Add(directory));
        }


        protected Model.Directory model;

        private ObservableCollection<Directory> subdirectories;
        private Stopwatch duration;
    }
}
