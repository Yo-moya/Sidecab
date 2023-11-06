
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
        private Presenter.FolderTree _presenter;
        private readonly DispatcherTimer _clickTimer = new();


        //----------------------------------------------------------------------
        public FolderTree()
        {
            InitializeComponent();

            var isDesignTime = (bool)(DesignerProperties.IsInDesignModeProperty
                .GetMetadata(typeof(DependencyObject)).DefaultValue);

            // To avoid "Object reference not set to an instance of an object"
            if (isDesignTime == false)
            {
                DataContext = _presenter = new();

                var doubleClickTime = SystemAttributes.GetDoubleClickTime();
                _clickTimer.Interval = new TimeSpan(0, 0, 0, 0, (int)doubleClickTime);
                _clickTimer.Tick += clickTimer_Tick;

                App.Core.Settings.PropertyChanged += OnSettingChanged;
            }
        }


        //----------------------------------------------------------------------
        private void OnSettingChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Contains("Font"))
            {
                TreeView_Directories.Items.Refresh();
                TreeView_Directories.UpdateLayout();
            }
        }

        //----------------------------------------------------------------------
        private T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            var parent = child;
            while ((parent is object) && ((parent is TreeViewItem) == false))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            return parent as T;
        }

        //----------------------------------------------------------------------
        private void clickTimer_Tick(object sender, EventArgs e)
        {
            _clickTimer.Stop();
        }

        //----------------------------------------------------------------------
        private void Button_AppMenu_Click(object sender, RoutedEventArgs e)
        {
            if (FindResource("ContextMenu_App") is ContextMenu menu)
            {
                menu.PlacementTarget = Button_AppMenu;
                menu.IsOpen = true;
            }
        }

        //----------------------------------------------------------------------
        private void MenuItem_Settings_Click(object sender, RoutedEventArgs e)
        {
            (App.Current.MainWindow as TreeWindow)?.OpenSettingsWindow();
        }

        //----------------------------------------------------------------------
        private void MenuItem_CloseApp_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        //----------------------------------------------------------------------
        private void TreeView_Directories_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (FindParent<TreeViewItem>(e.OriginalSource as DependencyObject) is TreeViewItem)
            {
                e.Handled = true;

                var directory = TreeView_Directories.SelectedItem as Presenter.Directory;
                directory?.Open();
            }
        }

        //----------------------------------------------------------------------
        private void MenuItem_Pin_Click(object sender, RoutedEventArgs e)
        {
            if (TreeView_Directories.SelectedItem is Presenter.Directory directory)
            {
                _presenter.AddBookmark(directory);
            }

            _presenter.SelectLastRoot();
        }

        //----------------------------------------------------------------------
        private void MenuItem_OpenFolder_Click(object sender, RoutedEventArgs e)
        {
            var directory = TreeView_Directories.SelectedItem as Presenter.Directory;
            directory?.Open();
        }

        //----------------------------------------------------------------------
        private void MenuItem_CopyPath_Click(object sender, RoutedEventArgs e)
        {
            var directory = TreeView_Directories.SelectedItem as Presenter.Directory;
            directory?.CopyPath();
        }

        //----------------------------------------------------------------------
        private void TreeView_Directories_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is ToggleButton) return;
            if (_clickTimer.IsEnabled) return;

            if (FindParent<TreeViewItem>(e.OriginalSource as DependencyObject) is TreeViewItem clicked)
            {
                if (clicked.IsExpanded == false)
                {
                    var directory = clicked.DataContext as Presenter.Directory;
                    directory?.CollectSubdirectories();
                }

                _clickTimer.Start();
                clicked.IsExpanded = !clicked.IsExpanded;
            }
        }

        //----------------------------------------------------------------------
        private void TreeView_Directories_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            var clicked = FindParent<TreeViewItem>(e.OriginalSource as DependencyObject);
            if (clicked is null)
            {
                e.Handled = true;
                return;
            }

            clicked.IsSelected = true;

            var directory = TreeView_Directories.SelectedItem as Presenter.Directory;
            MenuItem_Pin.IsEnabled = directory?.HasSubdirectories ?? false;
        }

        //----------------------------------------------------------------------
        private void TreeViewItem_Directories_Expanded(object sender, RoutedEventArgs e)
        {
            if (_clickTimer.IsEnabled) return;

            if (sender is TreeViewItem treeViewItem)
            {
                if (treeViewItem.DataContext is Presenter.Directory directory)
                {
                    directory.CollectSubdirectories();
                }

                _clickTimer.Start();
            }
        }
    }
}
