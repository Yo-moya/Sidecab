
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
            DataContext = App.Presenter;
        }

        //======================================================================
        public void OpenTreeWindow(bool activate = true)
        {
            if (TreeWindow == null)
            {
                TreeWindow = new TreeWindow();
                TreeWindow.SetSizePositionFrom(this);
                TreeWindow.ShowActivated = activate;
                TreeWindow.Show();
            }
        }

        //======================================================================
        public void CloseTreeWindow()
        {
            TreeWindow?.Close();
        }

        //======================================================================
        public void OpenSettingWindow()
        {
            if (SettingWindow == null)
            {
                SettingWindow = new SettingWindow();
                SettingWindow.Show();
            }
        }

        //======================================================================
        public void CloseSettingWindow()
        {
            SettingWindow?.Close();
        }

        //======================================================================
        public void NotifyChildWindowClosing(Window child)
        {
            if (child == null) return;

            //------------------------------------------------------------------
            if (child == TreeWindow)
            {
                TreeWindow = null;
                return;
            }
            //------------------------------------------------------------------
            if (child == SettingWindow)
            {
                SettingWindow = null;
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
