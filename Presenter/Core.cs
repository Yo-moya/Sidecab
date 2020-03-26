
using System.ComponentModel;

namespace Sidecab.Presenter
{
    public class Core : Base
    {
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

            _driveSelector = new DriveSelector(model.DriveList);
            _driveSelector.PropertyChanged += OnDriveChanged;

            _rootDirectory = new Directory(model.RootDirectory);
        }

        //======================================================================
        private void OnDriveChanged(object sender, PropertyChangedEventArgs e)
        {
            _model.SelectDrive(_driveSelector.CurrentDrive.DriveLetter);
            RaisePropertyChanged(nameof(RootDirectory));
        }


        private Model.Core _model;
        private DriveSelector _driveSelector;
        private Directory _rootDirectory;
    }
}
