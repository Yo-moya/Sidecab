
using System.IO;
using System.Collections.Generic;

namespace Sidecab.Model
{
    public class FolderTree
    {
        private List<Drive> _driveList = [];
        private LocationHistory _recentFolders = new();


        //----------------------------------------------------------------------
        public FolderTree()
        {
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
        public double GetFolderFreshnessScaleOf(Folder folder)
        {
            var value = _recentFolders.GetFreshness(folder);
            return ((double)value) / _recentFolders.MaxFreshness;
        }


        //----------------------------------------------------------------------
        private void RefreshDriveList()
        {
            foreach (var drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady)
                {
                     _driveList.Add(new(this, drive));
                }
            }
        }
    }
}
