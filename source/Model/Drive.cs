
using System.IO;

namespace Sidecab.Model
{
    public class Drive : Directory
    {
        public string Label { get; private set; } = "";


        //======================================================================
        public Drive(DriveInfo info)
        {
            this.Label = info.VolumeLabel;
            this.Name = info.Name.Substring(0, 2);
        }
    }
}
