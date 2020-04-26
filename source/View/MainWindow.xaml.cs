
using System.Windows;
using System.Windows.Input;

namespace Sidecab.View
{
    public partial class MainWindow
    {
        FileTreeWindow FileTreeWindow = null;
        SettingsWindow SettingsWindow = null;


        //======================================================================
        public MainWindow()
        {
            InitializeComponent();
            Utility.WindowAttributes.SetAsToolWindow(this);
            this.DataContext = new Presenter.MainWindow();
        }

        //======================================================================
        public void OpenFileTreeWindow(bool activate = true)
        {
            if (this.FileTreeWindow == null)
            {
                this.FileTreeWindow = new FileTreeWindow();
                this.FileTreeWindow.ShowActivated = activate;
                this.FileTreeWindow.SetSizePositionFrom(this);
                this.FileTreeWindow.Show();
            }
        }

        //======================================================================
        public void CloseFileTreeWindow()
        {
            this.FileTreeWindow?.Close();
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
            if (child == this.FileTreeWindow)
            {
                this.FileTreeWindow = null;
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
            OpenFileTreeWindow();
        }
    }
}
