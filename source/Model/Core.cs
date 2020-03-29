using System;
using System.Collections.Generic;
using System.IO;

namespace Sidecab.Model
{
    public class Core
    {
        public Settings Settings { get { return _settings; } }
        public Directory RootDirectory { get; private set; }
        public List<Drive> DriveList { get; private set; }


        //======================================================================
        public Core()
        {
            UpdateDriveList();
            _settings = Settings.Load() ?? new Settings();
        }

        //======================================================================
        public void SelectDrive(string driveLetter)
        {
            for (int i = 0; i < DriveList.Count; i++)
            {
                if (DriveList[i].DriveLetter == driveLetter)
                {
                    RootDirectory = _rootDirectoryList[i];
                    break;
                }
            }
        }

        //======================================================================
        private void UpdateDriveList()
        {
            var drives = DriveInfo.GetDrives();

            DriveList = new List<Drive>(drives.Length);
            _rootDirectoryList = new List<Directory>(drives.Length);

            //------------------------------------------------------------------
            foreach(var d in drives)
            {
                if (d.IsReady)
                {
                    DriveList.Add(new Drive(d));
                    _rootDirectoryList.Add(new Directory(d.Name));
                }
            }
            //------------------------------------------------------------------

            RootDirectory = _rootDirectoryList[0];
        }


        private Settings _settings;
        private List<Directory> _rootDirectoryList;
    }
}
