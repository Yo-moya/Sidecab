﻿
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.ComponentModel;

namespace Sidecab.View
{
    public partial class TreeWindow : Window
    {
        //======================================================================
        public TreeWindow()
        {
            this.WindowStyle = WindowStyle.None;
            this.ResizeMode = ResizeMode.NoResize;
            this.Topmost = true;

            InitializeComponent();

            this.DataContext = App.Presenter;
            App.Presenter.Settings.PropertyChanged += SettingWidth_Changed;
        }

        //======================================================================
        public void SetSizePositionFrom(Window parentWindow)
        {
            this.Top    = parentWindow.Top;
            this.Left   = parentWindow.Left;
            this.Height = parentWindow.Height;

            if (App.Model.Settings.DockPosition == Model.DockPosition.Right)
            {
                this.Left -= App.Model.Settings.TreeWidth - App.Model.Settings.KnobWidth;
            }
        }

        //======================================================================
        private void SettingWidth_Changed(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Presenter.Settings.TreeWidth))
            {
                var storyboard = TryFindResource("ShowingAnim") as Storyboard;
                if (storyboard != null) { storyboard.Stop(); }

                Width = App.Presenter.Settings.TreeWidth;
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

        //======================================================================
        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                var storyboard = TryFindResource("ShowingAnim") as Storyboard;
                if (storyboard != null) { storyboard.Begin(); }
            }
        }
    }
}
