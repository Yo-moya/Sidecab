
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
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

            InitializeComponent();

            this.DataContext = App.Core.Settings;
            App.Core.Settings.PropertyChanged += OnSettingWidthChanged;
        }

        //======================================================================
        public void ShowWithAnimation(Window parentWindow)
        {
            this.Top    = parentWindow.Top;
            this.Height = parentWindow.Height;
            this.Left   = parentWindow.Left;
            this.Width  = App.Core.Settings.TreeWidth;

            //------------------------------------------------------------------
            if (App.Core.Settings.DockPosition == Type.DockPosition.Left)
            {
                this.border_FileTree.RenderTransformOrigin = new Point(0, 0);
            }
            //------------------------------------------------------------------
            else
            {
                this.border_FileTree.RenderTransformOrigin = new Point(1, 0);
                this.Left -= App.Core.Settings.TreeWidth - App.Core.Settings.KnobWidth;
            }
            //------------------------------------------------------------------

            if (TryFindResource("storyboard_AnimToShow") is Storyboard storyboard)
            {
                storyboard.Begin();
            }

            // Calling Hide() makes the window inactive, so avoid it.
            App.Current.MainWindow.Visibility = Visibility.Collapsed;

            Show();
        }

        //======================================================================
        public void HideWithAnimation()
        {
            if (TryFindResource("storyboard_AnimToHide") is Storyboard storyboard)
            {
                storyboard.Begin();
            }
        }

        //======================================================================
        private void OnSettingWidthChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Presenter.Settings.TreeWidth))
            {
                Width = App.Core.Settings.TreeWidth;
            }
        }

        //======================================================================
        private void treeWindow_Closing(object sender, CancelEventArgs e)
        {
            // Close this window if the MainWindow is lost
            var result = (App.Current.MainWindow as MainWindow)
                ?.NotifyChildWindowClosing(this) ?? MainWindow.WindowBehaviorRestriction.CanClose;

            if (result == MainWindow.WindowBehaviorRestriction.CanNotClose)
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
        private void storyboard_AnimToHide_Completed(object sender, EventArgs e)
        {
            App.Current.MainWindow.ShowActivated = false;
            App.Current.MainWindow.Show();

            Hide();
        }
    }
}
