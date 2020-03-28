
using System.ComponentModel;
using System.Windows.Media;

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
        public DriveSelector DriveSelector
        {
            get { return _driveSelector; }
        }


        //======================================================================
        public Core(Model.Core model)
        {
            _model = model;
            _settings = new Settings(model.Settings);

            _driveSelector = new DriveSelector(model.DriveList);
            _driveSelector.PropertyChanged += Drive_Changed;

            _rootDirectory = new Directory(model.RootDirectory);
        }

        //======================================================================
        private void Drive_Changed(object sender, PropertyChangedEventArgs e)
        {
            _model.SelectDrive(_driveSelector.CurrentDrive.DriveLetter);
            RaisePropertyChanged(nameof(RootDirectory));
        }


        private Model.Core _model;
        private Settings _settings;
        private DriveSelector _driveSelector;
        private Directory _rootDirectory;
    }
}
