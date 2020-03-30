
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;

namespace Sidecab.View
{
    public partial class TreeWindow : Window
    {
        //======================================================================
        public TreeWindow()
        {
            WindowStyle = WindowStyle.None;
            ResizeMode = ResizeMode.NoResize;
            Topmost = true;

            InitializeComponent();
            DataContext = App.Presenter;
        }

        //======================================================================
        public void SetSizePositionFrom(Window parentWindow)
        {
            Top    = parentWindow.Top;
            Left   = parentWindow.Left;
            Height = parentWindow.Height;

            if (App.Model.Settings.DockPosition == Model.DockPosition.Right)
            {
                Left -= App.Model.Settings.TreeWidth - App.Model.Settings.KnobWidth;
            }
        }

        //======================================================================
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            (App.Current.MainWindow as MainWindow)?.NotifyChildWindowClosing(this);
        }

        //======================================================================
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }
    }
}
