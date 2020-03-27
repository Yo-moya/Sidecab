
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Sidecab.Presenter
{
    public class DriveSelector : Base
    {
        //----------------------------------------------------------------------
        public ObservableCollection<Drive> DriveList
        {
            get { return _driveList; }
        }

        //----------------------------------------------------------------------
        public Drive CurrentDrive
        {
            get { return _currentDrive; }
            set
            {
                _currentDrive = value;
                RaisePropertyChanged();
            }
        }


        //======================================================================
        public DriveSelector(List<Model.Drive> model)
        {
            _driveList = new ObservableCollection<Drive>();
            foreach (var d in model) { _driveList.Add(new Drive(d)); }
            _currentDrive = _driveList[0];
        }


        private ObservableCollection<Drive> _driveList;
        private Drive _currentDrive;
    }
}
