
using System.IO;
using System.Collections.Generic;

namespace Sidecab.Model
{
    public class Root : Directory
    {
        //======================================================================
        public Root(string path) : base(path)
        {
        }

        //======================================================================
        public Root(System.IO.DriveInfo info)
        {
            Path = info.Name;
            Name = info.VolumeLabel;
        }

        //======================================================================
        public Directory BuildDirectory()
        {
            return new Directory(Path);
        }


        //======================================================================
        public static List<Root> EnumerateDrives()
        {
            var drives = DriveInfo.GetDrives();
            var list   = new List<Root>(drives.Length);

            foreach(var d in drives)
            {
                if (d.IsReady) { list.Add(new Root(d)); }
            }

            return list;
        }
    }
}
