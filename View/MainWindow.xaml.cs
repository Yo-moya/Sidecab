
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Sidecab.View
{
    public partial class MainWindow
    {
        TreeWindow TreeWindow = null;


        //======================================================================
        public MainWindow()
        {
            InitializeComponent();

            DockedWidthOrHeight = App.Model.Settings.KnobWidth;
            Background = new SolidColorBrush(App.Model.Settings.KnobColor);
        }

        //======================================================================
        public void OpenTreeWindow()
        {
            if (TreeWindow == null)
            {
                TreeWindow = new TreeWindow();
                TreeWindow.Initialize(this);
                TreeWindow.Show();
            }
        }

        //======================================================================
        public void CloseTreeWindow()
        {
            TreeWindow?.Close();
        }

        //======================================================================
        public void NotifyChildWindowClosing(Window child)
        {
            if ((child != null) && (child == TreeWindow))
            {
                TreeWindow = null;
            }
        }

        //======================================================================
        private void AppBarWindow_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        //======================================================================
        private void AppBarWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenTreeWindow();
        }
    }
}
