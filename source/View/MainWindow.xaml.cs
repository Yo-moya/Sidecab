
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

            DataContext = App.Presenter;
            DockedWidthOrHeight = App.Model.Settings.KnobWidth;

            Utility.WindowAttributes.SetAsToolWindow(this);
        }

        //======================================================================
        public void OpenTreeWindow()
        {
            if (TreeWindow == null)
            {
                TreeWindow = new TreeWindow();
                TreeWindow.Initialize(this);
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
