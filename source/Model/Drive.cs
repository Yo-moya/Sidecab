
using System.IO;

namespace Sidecab.Model
{
    public class Drive : Folder
    {
        public string Label { get; private set; } = "";


        //----------------------------------------------------------------------
        public Drive(DriveInfo info)
        {
            Label = info.VolumeLabel;
            Name = info.Name.Substring(0, 2);
        }
    }
}
