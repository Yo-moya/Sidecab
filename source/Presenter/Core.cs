
using System.ComponentModel;
using System.Collections.Generic;

namespace Sidecab.Presenter
{
    public class Core : Base
    {
        //----------------------------------------------------------------------
        public Settings Settings
        {
            get { return _settings; }
        }

        //----------------------------------------------------------------------
        public Directory RootDirectory
        {
            get { return new Directory(_model.RootDirectory); }
        }

        //----------------------------------------------------------------------
        public Selector<Drive> DriveSelector
        {
            get { return _driveSelector; }
        }


        //======================================================================
        public Core(Model.Core model)
        {
            _model = model;
            _settings = new Settings(model.Settings);

            var drives = new List<Drive>(model.DriveList.Count);
            foreach (var d in model.DriveList) { drives.Add(new Drive(d)); }

            _driveSelector = new Selector<Drive>(drives);
            _driveSelector.PropertyChanged += Drive_Changed;

            _rootDirectory = new Directory(model.RootDirectory);
        }

        //======================================================================
        private void Drive_Changed(object sender, PropertyChangedEventArgs e)
        {
            _model.SelectDrive(_driveSelector.Current.DriveLetter);
            RaisePropertyChanged(nameof(RootDirectory));
        }


        private Model.Core _model;
        private Settings _settings;
        private Selector<Drive> _driveSelector;
        private Directory _rootDirectory;
    }
}
