
namespace Sidecab.Presenter
{
    public class Core : Utility.ObserverableObject
    {
        public Model.Core Model { get; private set; }
        public Settings Settings { get; private set; }


        //======================================================================
        public Core()
        {
            this.Model = new Model.Core();
            this.Settings = new Settings(this.Model.Settings);
        }
    }
}
