
using System;
using System.IO;
using System.Collections.Generic;

namespace Sidecab.Model
{
    public class FolderTree
    {
        public static FolderTree Instance { get; private set; } = null;

        private List<Drive> _driveList;
        private LocationHistory _recentFolders = new LocationHistory();


        //----------------------------------------------------------------------
        public FolderTree()
        {
            if (FolderTree.Instance is not null)
            {
                throw new Exception(
                    "Multiple 'Model.FolderTree' instances created.");
            }

            FolderTree.Instance = this;
            RefreshDriveList();
        }

        //----------------------------------------------------------------------
        public List<Drive> GetDriveList()
        {
            return new(_driveList);
        }

        //----------------------------------------------------------------------
        public void NotifyFolder(Folder folder)
        {
            _recentFolders.NotifyFolder(folder);
        }

        //----------------------------------------------------------------------
        public double GetFolderFreshnessScale(Folder folder)
        {
            var value = _recentFolders.GetFreshness(folder);
            return ((double)value) / _recentFolders.MaxFreshness;
        }


        //----------------------------------------------------------------------
        private void RefreshDriveList()
        {
            var drives = DriveInfo.GetDrives();
            _driveList = new(drives.Length);

            foreach (var d in drives)
            {
                if (d.IsReady) { _driveList.Add(new(d)); }
            }
        }
    }
}
