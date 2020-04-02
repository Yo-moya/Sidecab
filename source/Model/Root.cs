
using System.IO;
using System.Collections.Generic;

namespace Sidecab.Model
{
    public class Root : Directory
    {
        public bool IsDrive { get; private set; }

        //======================================================================
        public Root(Directory directory) : base(directory.Path)
        {
            IsDrive = false;
            this.SetChildren(directory.GetChildren());
        }

        //======================================================================
        public Root(DriveInfo info)
        {
            IsDrive = true;

            this.Path = info.Name;
            this.Name = info.VolumeLabel;
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
