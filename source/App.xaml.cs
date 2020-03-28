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
            var mainWindow = MainWindow as View.MainWindow;
            if (mainWindow != null)
            {
                mainWindow.CloseTreeWindow();
                mainWindow.CloseSettingWindow();
            }
        }
    }
}
