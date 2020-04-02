
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Sidecab.View
{
    public partial class MainWindow
    {
        TreeWindow TreeWindow = null;
        SettingWindow SettingWindow = null;


        //======================================================================
        public MainWindow()
        {
            InitializeComponent();
            Utility.WindowAttributes.SetAsToolWindow(this);
            this.DataContext = App.Presenter;
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
        public void OpenSettingWindow()
        {
            if (this.SettingWindow == null)
            {
                this.SettingWindow = new SettingWindow();
                this.SettingWindow.Show();
            }
        }

        //======================================================================
        public void CloseSettingWindow()
        {
            this.SettingWindow?.Close();
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
            if (child == this.SettingWindow)
            {
                this.SettingWindow = null;
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
