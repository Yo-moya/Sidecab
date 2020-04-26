
using System.Windows;
using System.Diagnostics;
using Microsoft.Win32;

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

            SystemEvents.PowerModeChanged += PowerModeChangedEventHandler;

            //------------------------------------------------------------------
            Exit += (exitEventSender, exitEventSenderArgs) =>
            {
                SystemEvents.PowerModeChanged -= PowerModeChangedEventHandler;
            };
            //------------------------------------------------------------------
        }

        //======================================================================
        protected override void OnDeactivated(System.EventArgs e)
        {
            var mainWindow = MainWindow as View.MainWindow;
            if (mainWindow != null)
            {
                mainWindow.CloseFileTreeWindow();
                mainWindow.CloseSettingWindow();
            }
        }

        //======================================================================
        private void PowerModeChangedEventHandler(object sender, PowerModeChangedEventArgs e)
        {
            if (e.Mode == PowerModes.Resume)
            {
                var appPath = Process.GetCurrentProcess().MainModule.FileName;

                //--------------------------------------------------------------
                var processInfo = new ProcessStartInfo()
                {
                    FileName = "cmd.exe",
                    Arguments = "/c timeout /t 2 /nobreak & start " + appPath,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                };
                //--------------------------------------------------------------

                // Workaround : popupmenus don't appear after resume
                Process.Start(processInfo);
                App.Current.Shutdown();
            }
        }
    }
}
