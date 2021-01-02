
using System.Windows;
using System.Windows.Input;

namespace Sidecab.View
{
    public partial class MainWindow
    {
        //----------------------------------------------------------------------
        public enum WindowBehaviorRestriction
        {
            CanClose   ,
            CanNotClose,
        }


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
            if (this.fileTreeWindow is null)
            {
                this.fileTreeWindow = new FileTreeWindow();
            }

            this.fileTreeWindow.ShowActivated = activate;
            this.fileTreeWindow.ShowWithAnimation(this);
        }

        //======================================================================
        public void CloseFileTreeWindow()
        {
            this.fileTreeWindow?.HideWithAnimation();
        }

        //======================================================================
        public void OpenSettingsWindow()
        {
            if (this.settingsWindow is null)
            {
                this.settingsWindow = new SettingsWindow();
                this.settingsWindow.Show();
            }
        }

        //======================================================================
        public void CloseSettingsWindow()
        {
            this.settingsWindow?.Close();
        }

        //======================================================================
        public WindowBehaviorRestriction NotifyChildWindowClosing(Window child)
        {
            if (child is object)
            {
                //--------------------------------------------------------------
                if (child == this.fileTreeWindow)
                {
                    return WindowBehaviorRestriction.CanNotClose;
                }
                //--------------------------------------------------------------
                if (child == this.settingsWindow)
                {
                    this.settingsWindow = null;
                    return WindowBehaviorRestriction.CanClose;
                }
                //--------------------------------------------------------------
            }

            return WindowBehaviorRestriction.CanNotClose;
        }


        FileTreeWindow fileTreeWindow = null;
        SettingsWindow settingsWindow = null;


        //======================================================================
        private void border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileTreeWindow();
        }
    }
}
