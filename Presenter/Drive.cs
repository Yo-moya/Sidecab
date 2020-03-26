
namespace Sidecab.Presenter
{
    public class Drive : Base
    {
        //----------------------------------------------------------------------
        public string Caption
        {
            get
            {
                return _model.DriveLetter.Substring(0, 2) + " " + _model.ValumeLabel;
            }
        }

        //----------------------------------------------------------------------
        public string DriveLetter
        {
            get
            {
                return _model.DriveLetter;
            }
        }


        //======================================================================
        public Drive(Model.Drive model)
        {
            _model = model;
        }


        private Model.Drive _model;
    }
}
