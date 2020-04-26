
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
            }

            this.FileTreeWindow.ShowActivated = activate;
            this.FileTreeWindow.ShowWithAnimation(this);
        }

        //======================================================================
        public void CloseFileTreeWindow()
        {
            this.FileTreeWindow?.HideWithAnimation();
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
        public void CloseSettingsWindow()
        {
            this.SettingsWindow?.Close();
        }

        //======================================================================
        public bool NotifyChildWindowClosing(Window child)
        {
            if (child != null)
            {
                //--------------------------------------------------------------
                if (child == this.FileTreeWindow)
                {
                    return false;
                }
                //--------------------------------------------------------------
                if (child == this.SettingsWindow)
                {
                    this.SettingsWindow = null;
                    return true;
                }
                //--------------------------------------------------------------
            }

            return false;
        }


        //======================================================================
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileTreeWindow();
        }
    }
}
