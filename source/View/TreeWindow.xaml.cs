
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace Sidecab.View
{
    public sealed partial class TreeWindow : Window
    {
        private SettingsWindow _settingsWindow;


        //----------------------------------------------------------------------
        public TreeWindow()
        {
            WindowStyle = WindowStyle.None;
            ResizeMode = ResizeMode.NoResize;

            InitializeComponent();
            DataContext = App.Core.Settings;
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
                Border_FolderTree.RenderTransformOrigin = new Point(0, 0);
            }
            else
            {
                Border_FolderTree.RenderTransformOrigin = new Point(1, 0);
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

        //----------------------------------------------------------------------
        public void OpenSettingsWindow()
        {
            _settingsWindow ??= new SettingsWindow();
            _settingsWindow.Closed += OnSettingsWindowClosed;
            _settingsWindow.Show();
        }


        //----------------------------------------------------------------------
        private void OnSettingsWindowClosed(object sender, EventArgs e)
        {
            _settingsWindow = null;
        }

        //----------------------------------------------------------------------
        private void TreeWindow_Closing(object sender, CancelEventArgs e)
        {
            _settingsWindow?.Close();
        }

        //----------------------------------------------------------------------
        private void TreeWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                HideWithAnimation();
            }
        }
    }
}
