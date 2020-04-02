
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
            this.DataContext = App.Presenter;
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
        private void treeView_Directories_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var clicked = FindParent<TreeViewItem>(e.OriginalSource as DependencyObject);
            if (clicked != null)
            {
                e.Handled = true;

                var directory = this.treeView_Directories.SelectedItem as Presenter.Directory;
                if (directory != null) { directory.Open(); }
            }
        }

        //======================================================================
        private void treeView_Directories_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is ToggleButton) return;

            var clicked = FindParent<TreeViewItem>(e.OriginalSource as DependencyObject);
            if (clicked != null) { clicked.IsExpanded = !clicked.IsExpanded; }
        }

        //======================================================================
        private void treeView_Directories_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            var clicked = FindParent<TreeViewItem>(e.OriginalSource as DependencyObject);
            if (clicked == null)
            {
                e.Handled = true;
                return;
            }

            clicked.IsSelected = true;
        }

        //======================================================================
        private void treeView_Directories_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            (e.NewValue as Presenter.Directory)?.ListSubdirectories(listSubSubdirectories : true);
        }

        //======================================================================
        private void manuItem_Root_Click(object sender, RoutedEventArgs e)
        {
            var directory = this.treeView_Directories.SelectedItem as Presenter.Directory;
            if (directory == null) return;

            App.Presenter.SetPinnedDirectory(directory);
        }

        //======================================================================
        private void menuItem_Open_Click(object sender, RoutedEventArgs e)
        {
            var directory = this.treeView_Directories.SelectedItem as Presenter.Directory;
            if (directory == null) return;

            directory.Open();
        }
    }
}
