
namespace Sidecab.Presenter
{
    public class TreeRoot : Directory
    {
        //----------------------------------------------------------------------
        public string SelectorCaption
        {
            get
            {
                var drive = this.model as Model.Drive;
                if (drive != null)
                {
                    // 'Name' contains drive letter
                    return drive.Name + " [ " + drive.Label + " ]";
                }

                return "/ " + this.Name;
            }
        }


        //======================================================================
        public Root(Model.Directory model) : base(model)
        {
        }
    }
}
