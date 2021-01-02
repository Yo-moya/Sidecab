
namespace Sidecab.Presenter
{
    public class Core : Utility.ObserverableObject
    {
        public Model.Core Model  { get; private set; } = new Model.Core();
        public Settings Settings { get; private set; }


        //======================================================================
        public Core()
        {
            this.Settings = new Settings(this.Model.Settings);
            this.Settings.Load();
        }
    }
}
