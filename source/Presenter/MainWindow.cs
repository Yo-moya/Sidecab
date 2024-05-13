
using System.ComponentModel;

namespace Sidecab.Presenter
{
    public class MainWindow : ObservableObject
    {
        public Settings Settings { get { return App.Settings; } }

        //----------------------------------------------------------------------
        public WpfAppBar.MonitorInfo DisplayToDock
        {
            get
            {
                int index = 0;

                //--------------------------------------------------------------
                foreach (var m in WpfAppBar.MonitorInfo.GetAllMonitors())
                {
                    if (index == App.Settings.DisplayIndex) return m;
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
                return (App.Settings.DockPosition == Type.DockPosition.Left) ?
                    WpfAppBar.AppBarDockMode.Left : WpfAppBar.AppBarDockMode.Right;
            }
        }


        //======================================================================
        public MainWindow()
        {
            App.Settings.PropertyChanged += OnSettingChanged;
        }

        //======================================================================
        ~MainWindow()
        {
            App.Settings.PropertyChanged -= OnSettingChanged;
        }


        //======================================================================
        private void OnSettingChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(App.Settings.DisplayIndex) :
                    RaisePropertyChanged(nameof(DisplayToDock));
                    break;
                case nameof(App.Settings.DockPosition) :
                    RaisePropertyChanged(nameof(DockMode));
                    break;
            }
        }
    }
}
