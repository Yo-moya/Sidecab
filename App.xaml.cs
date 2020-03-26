using System.Windows;

namespace Sidecab
{
    public partial class App : Application
    {
        public static Model.Core Model { get; private set; }
        public static Presenter.Core Presenter { get; private set; }


        //======================================================================
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Model = new Model.Core();
            Presenter = new Presenter.Core(Model);
        }

        //======================================================================
        protected override void OnDeactivated(System.EventArgs e)
        {
            (MainWindow as View.MainWindow).CloseTreeWindow();
        }
    }
}
