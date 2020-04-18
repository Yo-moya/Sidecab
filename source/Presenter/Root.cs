
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
                if (root.IsDrive)
                {
                    // 'Name' contains drive letter
                    return root.Name + " [ " + root.Label + " ]";
                }

                return "/ " + this.Name;
            }
        }


        //======================================================================
        public Root(Model.Root model) : base(model)
        {
            CollectSubdirectories();
        }
    }
}
