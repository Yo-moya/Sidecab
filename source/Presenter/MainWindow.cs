
using System.ComponentModel;

namespace Sidecab.Presenter
{
    public class MainWindow : Utility.ObserverableObject
    {
        public Settings Settings { get { return App.Presenter.Settings; } }

        //----------------------------------------------------------------------
        public WpfAppBar.MonitorInfo DisplayToDock
        {
            get
            {
                int index = 0;

                //--------------------------------------------------------------
                foreach (var m in WpfAppBar.MonitorInfo.GetAllMonitors())
                {
                    if (index == this.Settings.DisplayIndex) return m;
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
                return (this.Settings.DockPosition == Data.DockPosition.Left ?
                    WpfAppBar.AppBarDockMode.Left : WpfAppBar.AppBarDockMode.Right);
            }
        }


        //======================================================================
        public MainWindow()
        {
            this.Settings.PropertyChanged += OnSettingChanged;
        }

        //======================================================================
        ~MainWindow()
        {
            this.Settings.PropertyChanged -= OnSettingChanged;
        }


        //======================================================================
        private void OnSettingChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                //--------------------------------------------------------------
                case nameof(this.Settings.DisplayIndex) :
                    RaisePropertyChanged(nameof(this.DisplayToDock));
                    break;
                //--------------------------------------------------------------
                case nameof(this.Settings.DockPosition) :
                    RaisePropertyChanged(nameof(this.DockMode));
                    break;
                //--------------------------------------------------------------
            }
        }
    }
}
