
namespace Sidecab.Model
{
    public class Drive
    {
        public string DriveLetter { get; private set; }
        public string ValumeLabel { get; private set; }


        //======================================================================
        public Drive(System.IO.DriveInfo info)
        {
            DriveLetter = info.Name;
            ValumeLabel = info.VolumeLabel;
        }
    }
}
