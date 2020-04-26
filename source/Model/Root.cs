﻿
using System.IO;
using System.Collections.Generic;

namespace Sidecab.Model
{
    public class Root : Directory
    {
        public bool IsDrive { get; private set; } = false;
        public string Label { get; private set; } = "";


        //======================================================================
        public Root(DriveInfo info)
        {
            this.Label = info.VolumeLabel;
            this.Name = info.Name.Substring(0, 2);
            this.IsDrive = true;
        }

        // Copy directory as a root
        //======================================================================
        public Root(Directory directory)
            : base(new DirectoryInfo(directory.Path), directory.ParentDirectory)
        {
        }

        //======================================================================
        public static List<Root> EnumerateDrives()
        {
            var drives = DriveInfo.GetDrives();
            var list   = new List<Root>(drives.Length);

            foreach (var d in drives)
            {
                if (d.IsReady) { list.Add(new Root(d)); }
            }

            return list;
        }
    }
}
