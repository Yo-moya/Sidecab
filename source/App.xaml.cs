
using System.Windows;
using System.Diagnostics;
using Microsoft.Win32;
using System;

namespace Sidecab
{
    public partial class App : Application
    {
        public static Presenter.Core Core { get; private set; }


        //======================================================================
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            App.Core = new Presenter.Core();
            SystemEvents.PowerModeChanged += PowerModeChangedEventHandler;

            //------------------------------------------------------------------
            Exit += (exitEventSender, exitEventSenderArgs) =>
            {
                SystemEvents.PowerModeChanged -= PowerModeChangedEventHandler;
            };
            //------------------------------------------------------------------
        }

        //----------------------------------------------------------------------
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            if (MainWindow is View.TreeWindow window)
            {
                window.ShowWithAnimation();
            }
        }


        //----------------------------------------------------------------------
        protected override void OnDeactivated(System.EventArgs e)
        {
            base.OnDeactivated(e);

            if (MainWindow is View.TreeWindow window)
            {
                window.HideWithAnimation();
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

                // Workaround : popupmenus don't appear after system resume
                Process.Start(processInfo);
                App.Current.Shutdown();
            }
        }
    }
}
