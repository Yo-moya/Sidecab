
using System;
using System.Windows;

namespace Sidecab
{
    public sealed partial class App : Application
    {
        public static Presenter.Core Core { get; private set; }


        //----------------------------------------------------------------------
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Core = new Presenter.Core();
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
    }
}
