
namespace Sidecab.Presenter
{
    public class Root : Directory
    {
        //----------------------------------------------------------------------
        public string SelectorCaption
        {
            get
            {
                var driveLetter = _model.Path.Substring(0, _model.Path.IndexOf(@"\"));
                var interval = ((_model.Path.Length - driveLetter.Length) > 1) ? " ../ " : " ";
                return driveLetter + interval + _model.Name;
            }
        }


        //======================================================================
        public Root(Model.Root model) : base(model)
        {
            ListSubdirectories(listSubSubdirectories : true);
        }
    }
}
