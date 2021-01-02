
using System.IO;
using System.Collections.Generic;

namespace Sidecab.Model
{
    public class Core
    {
        public Settings Settings { get; private set; } = new Settings();


        //======================================================================
        public Core()
        {
            RefreshDriveList();
        }

        //======================================================================
        public List<Drive> GetDriveList()
        {
            return new List<Drive>(this.driveList);
        }

        //======================================================================
        public List<Directory> GetBookmarks()
        {
            return new List<Directory>(this.bookmarks);
        }

        //======================================================================
        public void AddBookmark(Directory directory)
        {
            this.bookmarks = new List<Directory>() { directory };
            // this.bookmarks.Add(directory);
        }

        //======================================================================
        public void NotifyUsingDirectory(Directory directory)
        {
            this.recentDirectories.NotifyDirectory(directory);
        }

        //======================================================================
        public double GetDirectoryFreshnessScale(Directory directory)
        {
            var value = this.recentDirectories.GetFreshness(directory);
            return ((double)value) / this.recentDirectories.MaxFreshness;
        }


        //======================================================================
        private void RefreshDriveList()
        {
            var drives = DriveInfo.GetDrives();
            this.driveList = new List<Drive>(drives.Length);

            foreach (var d in drives)
            {
                if (d.IsReady) { this.driveList.Add(new Drive(d)); }
            }
        }


        private List<Drive> driveList;
        private List<Directory> bookmarks = new List<Directory>();
        private DirectoryHistory recentDirectories = new DirectoryHistory();
    }
}
