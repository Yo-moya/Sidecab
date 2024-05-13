
using System;
using System.Windows;

namespace Sidecab
{
    public sealed partial class App : Application
    {
        public static Presenter.Settings Settings { get; private set; }
        private View.TreeWindow _treeWindow;


        //----------------------------------------------------------------------
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            _treeWindow.ShowWithAnimation();
        }

        //----------------------------------------------------------------------
        protected override void OnDeactivated(System.EventArgs e)
        {
            base.OnDeactivated(e);
            _treeWindow.HideWithAnimation();
        }


        //----------------------------------------------------------------------
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            App.Settings = new();
            App.Settings.LoadedEvent += OnSettingsLoaded;
            App.Settings.LoadAsync();
        }

        //----------------------------------------------------------------------
        private void OnSettingsLoaded()
        {
            App.Settings.LoadedEvent -= OnSettingsLoaded;

            MainWindow = _treeWindow = new View.TreeWindow();
            MainWindow.Show();
        }
    }
}
