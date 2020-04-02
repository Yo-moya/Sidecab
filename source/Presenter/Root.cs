
namespace Sidecab.Presenter
{
    public class Root : Directory
    {
        //----------------------------------------------------------------------
        public string SelectorCaption
        {
            get
            {
                var isDrive = (this.model as Model.Root)?.IsDrive ?? false;
                if (isDrive)
                {
                    var driveLetter = this.model.Path.Substring(0, this.model.Path.IndexOf(@"\"));
                    return driveLetter + " [ " + this.model.Name + " ]";
                }

                return "/ " + this.model.Name;
            }
        }


        //======================================================================
        public Root(Model.Root model) : base(model)
        {
            ListSubdirectories(listSubSubdirectories : true);
        }
    }
}
