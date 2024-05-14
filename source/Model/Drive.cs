
using System.IO;

namespace Sidecab.Model
{
    public class Drive : Folder
    {
        public string Label { get; private init; }


        //----------------------------------------------------------------------
        public Drive(FolderTree tree, DriveInfo info) : base(tree)
        {
            Label = info.VolumeLabel;
            Name  = info.Name[..2];
        }
    }
}
