
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

            DataContext = _presenter = new();

            var doubleClickTime = SystemAttributes.GetDoubleClickTime();
            _clickTimer.Interval = new TimeSpan(0, 0, 0, 0, (int)doubleClickTime);
            _clickTimer.Tick += ClickTimer_Tick;

            App.Settings.PropertyChanged += OnSettingChanged;
        }


        //----------------------------------------------------------------------
        private void OnSettingChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName?.Contains("Font") == true)
            {
                TreeView_Folders.Items.Refresh();
                TreeView_Folders.UpdateLayout();
            }
        }

        //----------------------------------------------------------------------
        private T? FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            var current = child;
            while ((current is not null) && (current is not TreeViewItem))
            {
                current = VisualTreeHelper.GetParent(current);
            }

            return current as T;
        }

        //----------------------------------------------------------------------
        private void ClickTimer_Tick(object? sender, EventArgs e)
        {
            _clickTimer.Stop();
        }

        //----------------------------------------------------------------------
        private void Button_AppMenu_Click(object? sender, RoutedEventArgs e)
        {
            if (FindResource("ContextMenu_App") is ContextMenu menu)
            {
                menu.PlacementTarget = Button_AppMenu;
                menu.IsOpen = true;
            }
        }

        //----------------------------------------------------------------------
        private void MenuItem_Settings_Click(object? sender, RoutedEventArgs e)
        {
            (App.Current.MainWindow as TreeWindow)?.OpenSettingsWindow();
        }

        //----------------------------------------------------------------------
        private void MenuItem_CloseApp_Click(object? sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        //----------------------------------------------------------------------
        private void TreeView_Folders_PreviewMouseDoubleClick(object? sender, MouseButtonEventArgs e)
        {
            if ((e.OriginalSource is DependencyObject item) &&
                (FindParent<TreeViewItem>(item) is TreeViewItem))
            {
                e.Handled = true;

                var folder = TreeView_Folders.SelectedItem as Presenter.Folder;
                folder?.Open();
            }
        }

        //----------------------------------------------------------------------
        private void MenuItem_Pin_Click(object? sender, RoutedEventArgs e)
        {
            if (TreeView_Folders.SelectedItem is Presenter.Folder folder)
            {
                _presenter.AddBookmark(folder);
            }

            _presenter.SelectLastRoot();
        }

        //----------------------------------------------------------------------
        private void MenuItem_OpenFolder_Click(object? sender, RoutedEventArgs e)
        {
            if (TreeView_Folders.SelectedItem is Presenter.Folder folder)
            {
                folder.Open();
            }
        }

        //----------------------------------------------------------------------
        private void MenuItem_CopyPath_Click(object? sender, RoutedEventArgs e)
        {
            if (TreeView_Folders.SelectedItem is Presenter.Folder folder)
            {
                folder.CopyPath();
            }
        }

        //----------------------------------------------------------------------
        private void TreeView_Folders_PreviewMouseLeftButtonUp(
            object? sender, MouseButtonEventArgs e)
        {
            if (_clickTimer.IsEnabled || (e.OriginalSource is ToggleButton)) return;
            if (e.OriginalSource is DependencyObject clicked)
            {
                var treeViewItem = FindParent<TreeViewItem>(clicked);
                if (treeViewItem is not null)
                {
                    if ((treeViewItem.IsExpanded == false) &&
                        (treeViewItem.DataContext is Presenter.Folder folder))
                    {
                        folder.CollectSubFolders();
                    }

                    treeViewItem.IsExpanded = !treeViewItem.IsExpanded;
                    _clickTimer.Start();
                }
            }
        }

        //----------------------------------------------------------------------
        private void TreeView_Folders_PreviewMouseRightButtonUp(
            object? sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is DependencyObject clicked)
            {
                var treeViewItem = FindParent<TreeViewItem>(clicked);
                if (treeViewItem is not null)
                {
                    treeViewItem.IsSelected = true;

                    var folder = TreeView_Folders.SelectedItem as Presenter.Folder;
                    MenuItem_Pin.IsEnabled = folder?.HasSubFolders ?? false;
                }
            }

            e.Handled = true;
        }

        //----------------------------------------------------------------------
        private void TreeViewItem_Folders_Expanded(object? sender, RoutedEventArgs e)
        {
            if (_clickTimer.IsEnabled) return;
            if (sender is TreeViewItem treeViewItem)
            {
                if (treeViewItem.DataContext is Presenter.Folder folder)
                {
                    folder.CollectSubFolders();
                }

                _clickTimer.Start();
            }
        }
    }
}
