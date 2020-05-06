
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using System.Windows.Media;
using System.Windows.Input;

namespace Sidecab.View
{
    public partial class FileTree : UserControl
    {
        //----------------------------------------------------------------------
        public Presenter.FileTree Presenter
        {
            get { return this.DataContext as Presenter.FileTree; }
        }

        //======================================================================
        public FileTree()
        {
            InitializeComponent();
            this.DataContext = new Presenter.FileTree();

            var doubleClickTime = Utility.SystemAttributes.GetDoubleClickTime();
            this.clickTimer.Interval = new TimeSpan(0, 0, 0, 0, (int)doubleClickTime);
            this.clickTimer.Tick += clickTimer_Tick;
        }


        private DispatcherTimer clickTimer = new DispatcherTimer();


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
        private void clickTimer_Tick(object sender, EventArgs e)
        {
            this.clickTimer.Stop();
        }

        //======================================================================
        private void button_AppMenu_Click(object sender, RoutedEventArgs e)
        {
            var menu = FindResource("contextMenu_App") as ContextMenu;
            if (menu != null)
            {
                menu.PlacementTarget = this.button_AppMenu;
                menu.IsOpen = true;
            }
        }

        //======================================================================
        private void manuItem_Settings_Click(object sender, RoutedEventArgs e)
        {
            (App.Current.MainWindow as MainWindow)?.OpenSettingsWindow();
        }

        //======================================================================
        private void menuItem_CloseApp_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        //======================================================================
        private void treeView_Directories_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var clicked = FindParent<TreeViewItem>(e.OriginalSource as DependencyObject);
            if (clicked != null)
            {
                e.Handled = true;

                var directory = this.treeView_Directories.SelectedItem as Presenter.Directory;
                directory?.Open();
            }
        }

        //======================================================================
        private void treeView_Directories_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is ToggleButton) return;
            if (clickTimer.IsEnabled) return;

            var clicked = FindParent<TreeViewItem>(e.OriginalSource as DependencyObject);
            if (clicked != null)
            {
                if (clicked.IsExpanded == false)
                {
                    var directory = clicked.DataContext as Presenter.Directory;
                    directory?.CollectSubdirectories();
                }

                clickTimer.Start();
                clicked.IsExpanded = !clicked.IsExpanded;
            }
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

            var directory = treeView_Directories.SelectedItem as Presenter.Directory;
            this.manuItem_PinToRoot.IsEnabled = directory?.HasSomeSubdirectories ?? false;
        }

        //======================================================================
        private void manuItem_PinToRoot_Click(object sender, RoutedEventArgs e)
        {
            var directory = this.treeView_Directories.SelectedItem as Presenter.Directory;
            if (directory != null) { this.Presenter.SetRootDirectory(directory); }
        }

        //======================================================================
        private void menuItem_OpenFolder_Click(object sender, RoutedEventArgs e)
        {
            var directory = this.treeView_Directories.SelectedItem as Presenter.Directory;
            directory?.Open();
        }

        //======================================================================
        private void manuItem_CopyPath_Click(object sender, RoutedEventArgs e)
        {
            var directory = this.treeView_Directories.SelectedItem as Presenter.Directory;
            directory?.CopyPath();
        }
    }
}
