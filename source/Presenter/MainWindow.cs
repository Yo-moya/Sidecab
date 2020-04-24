
namespace Sidecab.Presenter
{
    public class MainWindow : Base
    {
        public Selector<Root> RootSelector { get; private set; }

       //----------------------------------------------------------------------
        public WpfAppBar.AppBarDockMode DockMode
        {
            get
            {
                return (App.Presenter.Settings.DockPosition == DockPosition.Left ?
                    WpfAppBar.AppBarDockMode.Left : WpfAppBar.AppBarDockMode.Right);
            }
        }
    }
}
