
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.ComponentModel;

namespace Sidecab.View
{
    public partial class FileTreeWindow : Window
    {
        //======================================================================
        public FileTreeWindow()
        {
            this.WindowStyle = WindowStyle.None;
            this.ResizeMode = ResizeMode.NoResize;
            this.Topmost = true;

            InitializeComponent();

            this.DataContext = App.Presenter.Settings;
            App.Presenter.Settings.PropertyChanged += OnSettingWidthChanged;
        }

        //======================================================================
        public void SetSizePositionFrom(Window parentWindow)
        {
            this.Top    = parentWindow.Top;
            this.Height = parentWindow.Height;
            this.Left   = parentWindow.Left;
            this.Width  = App.Presenter.Settings.TreeWidth;

            //------------------------------------------------------------------
            if (App.Presenter.Settings.DockPosition == Data.DockPosition.Left)
            {
                border_FileTree.RenderTransformOrigin = new Point(0, 0);
            }
            //------------------------------------------------------------------
            else
            {
                border_FileTree.RenderTransformOrigin = new Point(1, 0);
                this.Left -= App.Presenter.Settings.TreeWidth - App.Presenter.Settings.KnobWidth;
            }
            //------------------------------------------------------------------
        }

        //======================================================================
        private void OnSettingWidthChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Presenter.Settings.TreeWidth))
            {
                var storyboard = TryFindResource("ShowingAnim") as Storyboard;
                if (storyboard != null) { storyboard.Stop(); }

                Width = App.Presenter.Settings.TreeWidth;
            }
        }

        //======================================================================
        private void treeWindow_Closing(object sender, CancelEventArgs e)
        {
            (App.Current.MainWindow as MainWindow)?.NotifyChildWindowClosing(this);
        }

        //======================================================================
        private void treeWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }

        //======================================================================
        private void treeWindow_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                var storyboard = TryFindResource("ShowingAnim") as Storyboard;
                if (storyboard != null) { storyboard.Begin(); }
            }
        }
    }
}
