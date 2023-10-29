
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.ComponentModel;

namespace Sidecab.View
{
    public partial class FileTreeWindow : Window
    {
        //----------------------------------------------------------------------
        public FileTreeWindow()
        {
            WindowStyle = WindowStyle.None;
            ResizeMode = ResizeMode.NoResize;

            InitializeComponent();

            DataContext = App.Core.Settings;
            App.Core.Settings.PropertyChanged += OnSettingChanged;
        }

        //----------------------------------------------------------------------
        public void ShowWithAnimation()
        {
            var workingArea = ScreenInfo.GetWorkingArea(this);

            Top    = workingArea.Top;
            Height = workingArea.Height;
            Left   = workingArea.Left;
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
        }

        //----------------------------------------------------------------------
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
    }
}
