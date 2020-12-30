
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections.ObjectModel;

namespace Sidecab.Presenter
{
    public class Directory : Utility.ObserverableObject
    {
        public Model.Directory Model { get; private set; }
        public string Name { get { return this.Model?.Name ?? ""; } }
        public string Path { get { return this.Model?.Path ?? ""; } }
        public bool HasSubdirectories { get { return this.Model?.HasSubdirectories ?? false; } }

        //----------------------------------------------------------------------
        public double TextFontSize
        {
            get
            {
                if (this.Model != null)
                {
                    return this.Model.Priority * 14;
                }

                return 14;
            }
        }

        //----------------------------------------------------------------------
        public ObservableCollection<Directory> Subdirectories
        {
            get
            {
                if (this.Model?.HasSubdirectories ?? false)
                {
                    if (this.Model.Subdirectories.Count == 0)
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
            this.Model = model;
            this.Model.ChildrenUpdated += this.OnChildrenUpdated;
        }

        //======================================================================
        ~Directory()
        {
            if (this.Model != null)
            {
                this.Model.ChildrenUpdated -= this.OnChildrenUpdated;
            }
        }

        //======================================================================
        public void CollectSubdirectories()
        {
            this.Model.CollectSubdirectories(true);
        }

        //======================================================================
        public void Open()
        {
            this.Model.Open();
            RaisePropertyChanged(nameof(TextFontSize));
        }

        //======================================================================
        public void CopyPath()
        {
            this.Model.CopyPath();
        }


        //======================================================================
        private void OnChildrenUpdated(Model.Directory.UpdateType updateType)
        {
            switch (updateType)
            {
                //--------------------------------------------------------------
                case Sidecab.Model.Directory.UpdateType.Initialize :

                    this.subdirectories = new ObservableCollection<Directory>();
                    RaisePropertyChanged(nameof(Subdirectories));
                    break;
                //--------------------------------------------------------------
                case Sidecab.Model.Directory.UpdateType.Growing :

                    AddSubdirectories();
                    RaisePropertyChanged(nameof(Subdirectories));
                    break;
                //--------------------------------------------------------------
                case Sidecab.Model.Directory.UpdateType.Finished :

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
            var modelCount = this.Model.Subdirectories.Count;
            if (modelCount > this.subdirectories.Count)
            {
                for (int i = this.subdirectories.Count; i < modelCount; i++)
                {
                    AddSubdirectory(new Directory(this.Model.Subdirectories[i]));
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


        private ObservableCollection<Directory> subdirectories;
        private Stopwatch duration;
    }
}
