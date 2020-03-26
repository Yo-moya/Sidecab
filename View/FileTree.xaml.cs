
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
    }
}
