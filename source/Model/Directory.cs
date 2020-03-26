
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sidecab.Model
{
    public class Directory
    {
        public string Path { get; private set; }
        public string Name { get; private set; }


        //======================================================================
        public Directory(string path)
        {
            SetPath(path);
        }

        //======================================================================
        public void SetPath(string path)
        {
            var location = System.IO.Path.GetDirectoryName(path);
            if (location == null) { location = path; }

            Path = path;
            Name = path.Substring(location.Length);

            if (Name.StartsWith('\\')) { Name = Name.Substring(1); }
        }

        //======================================================================
        public void Open()
        {
            Process.Start("explorer.exe", Path);
        }

        //======================================================================
        public List<Directory> ListSubdirectories()
        {
            //------------------------------------------------------------------
            try
            {
                var dirInfo = new System.IO.DirectoryInfo(Path);
                var subdirInfoList = dirInfo.GetDirectories();
                var subdirList = new List<Directory>(subdirInfoList.Length);

                foreach (var dir in subdirInfoList)
                {
                    if (dir.Attributes.HasFlag(FileAttributes.System) == false)
                    {
                        subdirList.Add(new Directory(dir.FullName));
                    }
                }

                return subdirList;
            }
            //------------------------------------------------------------------
            catch
            {
                return null;
            }
            //------------------------------------------------------------------
        }
    }
}
