
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.ComponentModel;

namespace Sidecab.View
{
    public partial class FileTreeWindow : Window
    {
        //======================================================================
        public FileTreeWindow()
        {
            this.WindowStyle = WindowStyle.None;
            this.ResizeMode = ResizeMode.NoResize;
            this.Topmost = true;

            InitializeComponent();

            this.DataContext = App.Presenter.Settings;
            App.Presenter.Settings.PropertyChanged += OnSettingWidthChanged;
        }

        //======================================================================
        public void ShowWithAnimation(Window parentWindow)
        {
            this.Top    = parentWindow.Top;
            this.Height = parentWindow.Height;
            this.Left   = parentWindow.Left;
            this.Width  = App.Presenter.Settings.TreeWidth;

            //------------------------------------------------------------------
            if (App.Presenter.Settings.DockPosition == Data.DockPosition.Left)
            {
                this.border_FileTree.RenderTransformOrigin = new Point(0, 0);
            }
            //------------------------------------------------------------------
            else
            {
                this.border_FileTree.RenderTransformOrigin = new Point(1, 0);
                this.Left -= App.Presenter.Settings.TreeWidth - App.Presenter.Settings.KnobWidth;
            }
            //------------------------------------------------------------------

            var storyboard = TryFindResource("animToShow") as Storyboard;
            if (storyboard != null) { storyboard.Begin(); }

            Show();
        }

        //======================================================================
        public void HideWithAnimation()
        {
            var storyboard = TryFindResource("animToHide") as Storyboard;
            if (storyboard != null) { storyboard.Begin(); }
        }

        //======================================================================
        private void OnSettingWidthChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Presenter.Settings.TreeWidth))
            {
                Width = App.Presenter.Settings.TreeWidth;
            }
        }

        //======================================================================
        private void treeWindow_Closing(object sender, CancelEventArgs e)
        {
            // true if the MainWindow is gone
            var canClose = (App.Current.MainWindow as MainWindow)
                ?.NotifyChildWindowClosing(this) ?? true;

            if (canClose == false)
            {
                e.Cancel = true;
                HideWithAnimation();
            }
        }

        //======================================================================
        private void treeWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                HideWithAnimation();
            }
        }

        //======================================================================
        private void animToHide_Completed(object sender, EventArgs e)
        {
            Hide();
        }
    }
}
