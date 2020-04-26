
namespace Sidecab.Presenter
{
    public class Core : Utility.ObserverableObject
    {
        public Settings Settings { get; private set; }


        //======================================================================
        public Core(Model.Core model)
        {
            this.model = model;
            this.Settings = new Settings(this.model.Settings);
        }


        private Model.Core model;
    }
}
