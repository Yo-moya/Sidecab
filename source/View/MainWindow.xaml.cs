
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

            WindowAttributes.SetAsToolWindow(this);
            this.DataContext = new Presenter.MainWindow();
        }

        //======================================================================
        public void OpenTreeWindow(bool activate = true)
        {
            if (this.treeWindow is null)
            {
                this.treeWindow = new TreeWindow();
            }

            this.treeWindow.ShowActivated = activate;
            this.treeWindow.ShowWithAnimation();
        }

        //======================================================================
        public void CloseTreeWindow()
        {
            this.treeWindow?.HideWithAnimation();
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
                if (child == this.treeWindow)
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


        TreeWindow treeWindow = null;
        SettingsWindow settingsWindow = null;


        //======================================================================
        private void border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenTreeWindow();
        }
    }
}
