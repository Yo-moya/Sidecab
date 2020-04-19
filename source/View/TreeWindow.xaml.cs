
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.ComponentModel;

namespace Sidecab.View
{
    public partial class TreeWindow : Window
    {
        //======================================================================
        public TreeWindow()
        {
            this.WindowStyle = WindowStyle.None;
            this.ResizeMode = ResizeMode.NoResize;
            this.Topmost = true;

            InitializeComponent();

            this.DataContext = App.Presenter;
            App.Presenter.Settings.PropertyChanged += OnSettingWidthChanged;
        }

        //======================================================================
        public void SetSizePositionFrom(Window parentWindow)
        {
            this.Top    = parentWindow.Top;
            this.Height = parentWindow.Height;
            this.Width  = App.Presenter.Settings.TreeMinWidth;

            // To slide from screen right
            if (App.Model.Settings.DockPosition == Model.DockPosition.Right)
            {
                this.Left  = horizontalOrigin = parentWindow.Left + parentWindow.Width;
                this.Left -= App.Presenter.Settings.TreeMinWidth;
            }
            else
            {
                this.Left = parentWindow.Left;
            }
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

        //======================================================================
        private void treeWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (App.Model.Settings.DockPosition == Model.DockPosition.Right)
            {
                this.Left = horizontalOrigin - this.Width;
            }
        }


        private double horizontalOrigin = 0;
    }
}
