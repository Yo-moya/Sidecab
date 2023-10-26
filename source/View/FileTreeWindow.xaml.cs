
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
            WindowStyle = WindowStyle.None;
            ResizeMode = ResizeMode.NoResize;

            InitializeComponent();

            DataContext = App.Core.Settings;
            App.Core.Settings.PropertyChanged += OnSettingChanged;
        }

        //======================================================================
        public void ShowWithAnimation(Window parentWindow)
        {
            Top    = parentWindow.Top;
            Height = parentWindow.Height;
            Left   = parentWindow.Left;
            Width  = App.Core.Settings.TreeWidth;

            if (App.Core.Settings.DockPosition == Type.DockPosition.Left)
            {
                Border_FileTree.RenderTransformOrigin = new Point(0, 0);
            }
            else
            {
                Border_FileTree.RenderTransformOrigin = new Point(1, 0);
                Left -= App.Core.Settings.TreeWidth - App.Core.Settings.KnobWidth;
            }

            if (TryFindResource("Storyboard_AnimToShow") is Storyboard storyboard)
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
            if (TryFindResource("Storyboard_AnimToHide") is Storyboard storyboard)
            {
                storyboard.Begin();
            }
        }

        //======================================================================
        private void OnSettingChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Presenter.Settings.TreeWidth))
            {
                Width = App.Core.Settings.TreeWidth;
            }
        }

        //======================================================================
        private void TreeWindow_Closing(object sender, CancelEventArgs e)
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
        private void TreeWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                HideWithAnimation();
            }
        }

        //======================================================================
        private void Storyboard_AnimToHide_Completed(object sender, EventArgs e)
        {
            App.Current.MainWindow.ShowActivated = false;
            App.Current.MainWindow.Show();

            Hide();
        }
    }
}
