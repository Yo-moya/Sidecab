
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sidecab.View
{
    //==========================================================================
    public partial class FileTree : UserControl
    {
        //======================================================================
        public FileTree()
        {
            InitializeComponent();
            DataContext = App.Presenter;
        }

        //======================================================================
        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                var directory = treeView_Directories.SelectedItem as Presenter.Directory;
                if (directory != null) { directory.Open(); }
            }
        }

        //======================================================================
        private void button_Settings_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = App.Current.MainWindow as MainWindow;
            mainWindow?.OpenSettingWindow();
        }
    }
}
