
namespace Sidecab.Presenter
{
    public class TreeRoot : Directory
    {
        //----------------------------------------------------------------------
        public string SelectorCaption
        {
            get
            {
                if (this.Model is Model.Drive drive)
                {
                    // 'Name' contains drive letter
                    return drive.Name + " [ " + drive.Label + " ]";
                }

                return "pinned // " + this.Name;
            }
        }


        //======================================================================
        public TreeRoot(Model.Directory model) : base(model)
        {
        }
    }
}
