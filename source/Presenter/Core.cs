
using System.ComponentModel;
using System.Collections.Generic;

namespace Sidecab.Presenter
{
    public class Core : Base
    {
        public Settings Settings { get; private set; }
        public Selector<Root> RootLocations { get; private set; }


        //======================================================================
        public Core(Model.Core model)
        {
            _model = model;
            Settings = new Settings(model.Settings);

            var roots = new List<Root>(model.RootLocations.Count);
            foreach (var loc in model.RootLocations) { roots.Add(new Root(loc)); }

            RootLocations = new Selector<Root>(roots);
            RootLocations.PropertyChanged += Root_Changed;
        }

        //======================================================================
        private void Root_Changed(object sender, PropertyChangedEventArgs e)
        {
            _model.SetRoot(RootLocations.Current.Path);
            RaisePropertyChanged(nameof(RootLocations));
        }


        private Model.Core _model;
    }
}
