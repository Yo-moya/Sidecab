
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections.ObjectModel;

namespace Sidecab.Presenter
{
    public class Directory : ObservableObject
    {
        public Model.Directory Model { get; private set; }
        public string Name { get { return this.Model?.Name ?? ""; } }
        public string Path { get { return this.Model?.Path ?? ""; } }

        //----------------------------------------------------------------------
        public double TextFontSize
        {
            get
            {
                var settings = App.Core.Settings;
                if (this.Model is null) return settings.TreeFontSize;

                double gap = Math.Max(0, settings.TreeFontSizeLarge - settings.TreeFontSize);
                return gap * this.Model.GetFreshnessScale() + settings.TreeFontSize;
            }
        }

        //----------------------------------------------------------------------
        public ObservableCollection<Directory> Subdirectories
        {
            get
            {
                if (this.HasSubdirectories)
                {
                    if (this.Model.Subdirectories.Count == 0)
                    {
                        return new ObservableCollection<Directory> { new Directory() };
                    }
                }

                return this.subdirectories;
            }
        }

        //----------------------------------------------------------------------
        public bool HasSubdirectories
        {
            get { return this.Model?.HasSubdirectories ?? false; }
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

            this.Model.FreshnessUpdated += this.OnTextFontUpdated;
            this.Model. ChildrenUpdated += this.OnChildrenUpdated;
        }

        //======================================================================
        ~Directory()
        {
            if (this.Model is object)
            {
                this.Model.FreshnessUpdated -= this.OnTextFontUpdated;
                this.Model. ChildrenUpdated -= this.OnChildrenUpdated;
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
        }

        //======================================================================
        public void CopyPath()
        {
            this.Model.CopyPath();
        }


        //======================================================================
        private void OnTextFontUpdated()
        {
            RaisePropertyChanged(nameof(TextFontSize));
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

                    this.animationDuration = null;
                    break;
                //--------------------------------------------------------------
            }
        }

        //======================================================================
        private void AddSubdirectories()
        {
            if (this.animationDuration is null)
            {
                this.animationDuration = new Stopwatch();
                this.animationDuration.Start();
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
            var delay = Math.Max(0, 16 - this.animationDuration.ElapsedMilliseconds);
            System.Threading.Thread.Sleep((int)delay);

            this.animationDuration.Reset();
            this.animationDuration.Start();
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
        private Stopwatch animationDuration;
    }
}
