
using System.Text.RegularExpressions;

namespace Sidecab.Presenter
{
    public class SettingsWindow : Base
    {
        //----------------------------------------------------------------------
        public string TreeWidthAsText
        {
            get { return App.Presenter.Settings.TreeWidth.ToString(); }
            set { App.Presenter.Settings.TreeWidth = ConvertTextToNumber(value); }
        }

        //----------------------------------------------------------------------
        public string KnobWidthAsText
        {
            get { return App.Presenter.Settings.KnobWidth.ToString(); }
            set { App.Presenter.Settings.KnobWidth = ConvertTextToNumber(value); }
        }

        //----------------------------------------------------------------------
        public WpfAppBar.MonitorInfo DisplayToDock
        {
            get
            {
                int index = 0;
                foreach (var m in WpfAppBar.MonitorInfo.GetAllMonitors())
                {
                    if (index == App.Model.Settings.DisplayIndex) return m;
                    index++;
                }

                return null;
            }
        }

        //----------------------------------------------------------------------
        public Selector<string> PositionSelector
        {
            get
            {
                this.positionSelector.Index =
                    this.positionSelector.List.IndexOf(App.Presenter.Settings.DockPosition.ToString());

                return this.positionSelector;
            }
        }

        //----------------------------------------------------------------------
        public Selector<string> DisplaySelector
        {
            get
            {
                this.displaySelector.Current = this.displaySelector.List[App.Presenter.Settings.DisplayIndex];
                return this.displaySelector;
            }
        }


        //======================================================================
        private int ConvertTextToNumber(string text)
        {
            text = Regex.Replace(text, @"\D", "");
            return int.Parse((text == "") ? "0" : text);
        }


        private Selector<string>  displaySelector;
        private Selector<string> positionSelector;
    }
}
