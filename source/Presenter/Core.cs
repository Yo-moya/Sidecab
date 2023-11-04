
namespace Sidecab.Presenter
{
    public class Core : ObservableObject
    {
        public Model.Core Model  { get; private set; } = new Model.Core();
        public Settings Settings { get; private set; }


        //----------------------------------------------------------------------
        public Core()
        {
            Settings = new Settings(this.Model.Settings);
            Settings.LoadAsync();
        }
    }
}
