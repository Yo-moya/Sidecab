
namespace Sidecab.Presenter
{
    public sealed class FolderRoot : Folder
    {
        //----------------------------------------------------------------------
        public string SelectorCaption
        {
            get
            {
                if (base.Model is Model.Drive drive)
                {
                    // 'Name' property contains drive letter
                    return $"{drive.Name} {drive.Label}";
                }

                return "pinned // " + Name;
            }
        }


        //----------------------------------------------------------------------
        public FolderRoot(Model.Folder model) : base(model)
        {
        }

        // Escalate from normal folder object to root
        //----------------------------------------------------------------------
        public FolderRoot(Folder target) : base(target)
        {
        }
    }
}
