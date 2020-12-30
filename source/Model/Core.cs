
using System.IO;
using System.Collections.Generic;

namespace Sidecab.Model
{
    public class Core
    {
        public Settings Settings { get; private set; }


        //======================================================================
        public Core()
        {
            this.Settings = Settings.Load() ?? new Settings();
            RefreshRootList();
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
        public void RefreshRootList()
        {
            var drives = DriveInfo.GetDrives();
            this.driveList = new List<Drive>(drives.Length);

            foreach (var d in drives)
            {
                if (d.IsReady) { this.driveList.Add(new Drive(d)); }
            }
        }

        //======================================================================
        public void AddBookmark(Directory directory)
        {
            this.bookmarks = new List<Directory>() { directory };
            // this.bookmarks.Add(directory);
        }


        private List<Drive> driveList;
        private List<Directory> bookmarks = new List<Directory>();
    }
}
