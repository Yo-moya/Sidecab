
using System;
using System.Windows;
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
    }
}
