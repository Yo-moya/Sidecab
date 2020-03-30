
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Input;

namespace Sidecab.View
{
    public partial class FileTree : UserControl
    {
        //======================================================================
        public FileTree()
        {
            InitializeComponent();
            DataContext = App.Presenter;
        }

        //======================================================================
        private T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            var parent = child;

            //------------------------------------------------------------------
            while ((parent != null) && (parent is TreeViewItem) == false)
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            //------------------------------------------------------------------

            return parent as T;
        }

        //======================================================================
        private void button_Settings_Click(object sender, RoutedEventArgs e)
        {
            (App.Current.MainWindow as MainWindow)?.OpenSettingWindow();
        }

        //======================================================================
        private void treeView_Directories_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is ToggleButton) return;

            var clicked = FindParent<TreeViewItem>(e.OriginalSource as DependencyObject);
            if (clicked != null) { clicked.IsExpanded = !clicked.IsExpanded; }
        }

        //======================================================================
        private void treeView_Directories_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var clicked = FindParent<TreeViewItem>(e.OriginalSource as DependencyObject);
            if (clicked != null)
            {
                e.Handled = true;

                var directory = treeView_Directories.SelectedItem as Presenter.Directory;
                if (directory != null) { directory.Open(); }
            }
        }
    }
}
