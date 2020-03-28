
using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.ComponentModel;

namespace Sidecab.View
{
    public partial class SettingWindow : Window
    {
        //======================================================================
        public SettingWindow()
        {
            InitializeComponent();

            DataContext = App.Presenter.Settings;
            popup_KnobColor.DataContext = App.Presenter.Settings.KnobColor;
        }

        //======================================================================
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            var mainWindow = App.Current.MainWindow as MainWindow;
            mainWindow?.NotifyChildWindowClosing(this);
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
        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            Utility.WindowAttributes.HideMinimizeButton(this);
        }

        //======================================================================
        private void button_KnobColor_Click(object sender, RoutedEventArgs e)
        {
            popup_KnobColor.IsOpen = true;
        }
    }
}
