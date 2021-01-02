
using System.ComponentModel;

namespace Sidecab.Presenter
{
    public class MainWindow : Utility.ObserverableObject
    {
        public Settings Settings { get { return App.Core.Settings; } }

        //----------------------------------------------------------------------
        public WpfAppBar.MonitorInfo DisplayToDock
        {
            get
            {
                int index = 0;

                //--------------------------------------------------------------
                foreach (var m in WpfAppBar.MonitorInfo.GetAllMonitors())
                {
                    if (index == App.Core.Settings.DisplayIndex) return m;
                    index++;
                }
                //--------------------------------------------------------------

                return null;
            }
        }

       //----------------------------------------------------------------------
        public WpfAppBar.AppBarDockMode DockMode
        {
            get
            {
                return (App.Core.Settings.DockPosition == Type.DockPosition.Left ?
                    WpfAppBar.AppBarDockMode.Left : WpfAppBar.AppBarDockMode.Right);
            }
        }


        //======================================================================
        public MainWindow()
        {
            App.Core.Settings.PropertyChanged += OnSettingChanged;
        }

        //======================================================================
        ~MainWindow()
        {
            App.Core.Settings.PropertyChanged -= OnSettingChanged;
        }


        //======================================================================
        private void OnSettingChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                //--------------------------------------------------------------
                case nameof(App.Core.Settings.DisplayIndex) :
                    RaisePropertyChanged(nameof(this.DisplayToDock));
                    break;
                //--------------------------------------------------------------
                case nameof(App.Core.Settings.DockPosition) :
                    RaisePropertyChanged(nameof(this.DockMode));
                    break;
                //--------------------------------------------------------------
            }
        }
    }
}
