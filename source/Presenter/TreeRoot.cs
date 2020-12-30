
namespace Sidecab.Presenter
{
    public class TreeRoot : Directory
    {
        //----------------------------------------------------------------------
        public string SelectorCaption
        {
            get
            {
                var drive = this.Model as Model.Drive;
                if (drive != null)
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
