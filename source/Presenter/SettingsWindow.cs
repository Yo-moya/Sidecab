
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Sidecab.Presenter
{
    public class SettingsWindow : Utility.ObserverableObject
    {
        public Settings Settings { get { return App.Core.Settings; } }

        //----------------------------------------------------------------------
        public string TreeWidthAsText
        {
            get { return this.Settings.TreeWidth.ToString(); }
            set { this.Settings.TreeWidth = ConvertTextToNumber(value); }
        }

        //----------------------------------------------------------------------
        public string KnobWidthAsText
        {
            get { return this.Settings.KnobWidth.ToString(); }
            set { this.Settings.KnobWidth = ConvertTextToNumber(value); }
        }

        //----------------------------------------------------------------------
        public Selector<string> DisplayIndexSelector
        {
            get
            {
                this.displayIndexSelector.Current =
                    this.displayIndexSelector.List[this.Settings.DisplayIndex];

                return this.displayIndexSelector;
            }
        }

        //----------------------------------------------------------------------
        public Selector<string> DockPositionSelector
        {
            get
            {
                this.dockPositionSelector.Index =
                    this.dockPositionSelector.List.IndexOf(this.Settings.DockPosition.ToString());

                return this.dockPositionSelector;
            }
        }


        //======================================================================
        public SettingsWindow()
        {
            var displayIndexList = new List<string>();
            var dockPositionList = new List<string>();

            //------------------------------------------------------------------
            using (var monitors = WpfAppBar.MonitorInfo.GetAllMonitors().GetEnumerator())
            {
                int count = 0;
                while (monitors.MoveNext())
                {
                    count++;
                    displayIndexList.Add("Display " + count.ToString());
                }
            }
            //------------------------------------------------------------------
            foreach (var dockPos in Enum.GetValues(typeof(Type.DockPosition)))
            {
                dockPositionList.Add(dockPos.ToString());
            }
            //------------------------------------------------------------------

            this.displayIndexSelector = new Selector<string>(displayIndexList);
            this.displayIndexSelector.PropertyChanged += this.OnDisplayIndexChanged;

            this.dockPositionSelector = new Selector<string>(dockPositionList);
            this.dockPositionSelector.PropertyChanged += this.OnDockPositionChanged;
        }

        //======================================================================
        ~SettingsWindow()
        {
            this.displayIndexSelector.PropertyChanged -= this.OnDisplayIndexChanged;
            this.dockPositionSelector.PropertyChanged -= this.OnDockPositionChanged;
        }

        //======================================================================
        public void RefreshView()
        {
            RaiseAllPropertiesChanged();
        }


        //======================================================================
        private int ConvertTextToNumber(string text)
        {
            text = Regex.Replace(text, @"\D", "");
            return int.Parse((text == "") ? "0" : text);
        }

        //======================================================================
        private void OnDisplayIndexChanged(object sender, PropertyChangedEventArgs e)
        {
            this.Settings.DisplayIndex = this.displayIndexSelector.Index;
        }

        //======================================================================
        private void OnDockPositionChanged(object sender, PropertyChangedEventArgs e)
        {
            foreach (var dockPos in Enum.GetValues(typeof(Type.DockPosition)))
            {
                if (this.dockPositionSelector.Current == dockPos.ToString())
                {
                    this.Settings.DockPosition = (Type.DockPosition)dockPos;
                    return;
                }
            }
        }


        private Selector<string> displayIndexSelector;
        private Selector<string> dockPositionSelector;
    }
}
