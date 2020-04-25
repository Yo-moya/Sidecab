
using System.Windows;
using System.Windows.Input;

namespace Sidecab.View
{
    public partial class MainWindow
    {
        TreeWindow TreeWindow = null;
        SettingsWindow SettingsWindow = null;


        //======================================================================
        public MainWindow()
        {
            InitializeComponent();
            Utility.WindowAttributes.SetAsToolWindow(this);
            this.DataContext = new Presenter.MainWindow();
        }

        //======================================================================
        public void OpenTreeWindow(bool activate = true)
        {
            if (this.TreeWindow == null)
            {
                this.TreeWindow = new TreeWindow();
                this.TreeWindow.SetSizePositionFrom(this);
                this.TreeWindow.ShowActivated = activate;
                this.TreeWindow.Show();
            }
        }

        //======================================================================
        public void CloseTreeWindow()
        {
            this.TreeWindow?.Close();
        }

        //======================================================================
        public void OpenSettingsWindow()
        {
            if (this.SettingsWindow == null)
            {
                this.SettingsWindow = new SettingsWindow();
                this.SettingsWindow.Show();
            }
        }

        //======================================================================
        public void CloseSettingWindow()
        {
            this.SettingsWindow?.Close();
        }

        //======================================================================
        public void NotifyChildWindowClosing(Window child)
        {
            if (child == null) return;

            //------------------------------------------------------------------
            if (child == this.TreeWindow)
            {
                this.TreeWindow = null;
                return;
            }
            //------------------------------------------------------------------
            if (child == this.SettingsWindow)
            {
                this.SettingsWindow = null;
                return;
            }
            //------------------------------------------------------------------
        }


        //======================================================================
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenTreeWindow();
        }
    }
}
