
namespace Sidecab.Presenter
{
    public class Root : Directory
    {
        //----------------------------------------------------------------------
        public string SelectorCaption
        {
            get
            {
                var root = this.model as Model.Root;
                if (root != null)
                {
                    if (root.IsDrive)
                    {
                        return root.Name + " [ " + root.Label + " ]";
                    }
                }

                return "/ " + this.Name;
            }
        }


        //======================================================================
        public Root(Model.Root model) : base(model)
        {
            EnumerateSubdirectories();
        }
    }
}
