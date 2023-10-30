
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using System.Windows.Media;
using System.Windows.Input;
using System.ComponentModel;

namespace Sidecab.View
{
    public partial class FolderTree : UserControl
    {
        //----------------------------------------------------------------------
        public Presenter.FolderTree Presenter
        {
            get { return this.DataContext as Presenter.FolderTree; }
        }

        //======================================================================
        public FolderTree()
        {
            InitializeComponent();

            var isDesignTime = (bool)(DesignerProperties.IsInDesignModeProperty
                .GetMetadata(typeof(DependencyObject)).DefaultValue);

            // To avoid "Object reference not set to an instance of an object"
            if (isDesignTime == false)
            {
                this.DataContext = new Presenter.FolderTree();

                var doubleClickTime = SystemAttributes.GetDoubleClickTime();
                this.clickTimer.Interval = new TimeSpan(0, 0, 0, 0, (int)doubleClickTime);
                this.clickTimer.Tick += clickTimer_Tick;

                App.Core.Settings.PropertyChanged += this.OnSettingChanged;
            }
        }

        //======================================================================
        ~FolderTree()
        {
            App.Core.Settings.PropertyChanged -= this.OnSettingChanged;
        }


        private DispatcherTimer clickTimer = new DispatcherTimer();


        //======================================================================
        private void OnSettingChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Contains("Font"))
            {
                this.treeView_Directories.Items.Refresh();
                this.treeView_Directories.UpdateLayout();
            }
        }

        //======================================================================
        private T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            var parent = child;

            //------------------------------------------------------------------
            while ((parent is object) && ((parent is TreeViewItem) == false))
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
        private void Button_AppMenu_Click(object sender, RoutedEventArgs e)
        {
            if (FindResource("ContextMenu_App") is ContextMenu menu)
            {
                menu.PlacementTarget = Button_AppMenu;
                menu.IsOpen = true;
            }
        }

        //======================================================================
        private void MenuItem_Settings_Click(object sender, RoutedEventArgs e)
        {
            (App.Current.MainWindow as TreeWindow)?.OpenSettingsWindow();
        }

        //======================================================================
        private void MenuItem_CloseApp_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        //======================================================================
        private void treeView_Directories_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (FindParent<TreeViewItem>(e.OriginalSource as DependencyObject) is TreeViewItem)
            {
                e.Handled = true;

                var directory = this.treeView_Directories.SelectedItem as Presenter.Directory;
                directory?.Open();
            }
        }

        //======================================================================
        private void manuItem_Pin_Click(object sender, RoutedEventArgs e)
        {
            if (this.treeView_Directories.SelectedItem is Presenter.Directory directory)
            {
                this.Presenter.AddBookmark(directory);
            }

            this.Presenter.SelectLastRoot();
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

        //======================================================================
        private void treeView_Directories_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is ToggleButton) return;
            if (this.clickTimer.IsEnabled) return;

            if (FindParent<TreeViewItem>(e.OriginalSource as DependencyObject) is TreeViewItem clicked)
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
            if (clicked is null)
            {
                e.Handled = true;
                return;
            }

            clicked.IsSelected = true;

            var directory = treeView_Directories.SelectedItem as Presenter.Directory;
            this.manuItem_Pin.IsEnabled = directory?.HasSubdirectories ?? false;
        }

        //======================================================================
        private void treeViewItem_Directories_Expanded(object sender, RoutedEventArgs e)
        {
            if (this.clickTimer.IsEnabled) return;

            if (sender is TreeViewItem treeViewItem)
            {
                if (treeViewItem.DataContext is Presenter.Directory directory)
                {
                    directory.CollectSubdirectories();
                }

                clickTimer.Start();
            }
        }
    }
}
