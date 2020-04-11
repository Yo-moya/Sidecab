
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Windows.Media;

namespace Sidecab.Presenter
{
    public class Settings : Base
    {
        //----------------------------------------------------------------------
        public int TreeWidth
        {
            get { return this.model.TreeWidth; }
            set
            {
                this.model.TreeWidth = Math.Max(TreeMinWidth, value);

                RaisePropertyChanged(nameof(TreeWidth));
                RaisePropertyChanged(nameof(TreeWidthAsText));
            }
        }

        //----------------------------------------------------------------------
        public int TreeMinWidth
        {
            get { return 100; }
        }

        //----------------------------------------------------------------------
        public int KnobWidth
        {
            get { return this.model.KnobWidth; }
            set
            {
                this.model.KnobWidth = Math.Max(1, value);

                RaisePropertyChanged(nameof(this.KnobWidth));
                RaisePropertyChanged(nameof(this.KnobWidthAsText));
            }
        }

        //----------------------------------------------------------------------
        public WpfAppBar.MonitorInfo DisplayToDock
        {
            get
            {
                int index = 0;
                foreach (var m in WpfAppBar.MonitorInfo.GetAllMonitors())
                {
                    if (index == this.model.DisplayIndex) return m;
                    index++;
                }

                return null;
            }
        }

        //----------------------------------------------------------------------
        public WpfAppBar.AppBarDockMode DockPosition
        {
            get
            {
                return (this.model.DockPosition == Model.DockPosition.Left)
                    ? WpfAppBar.AppBarDockMode.Left : WpfAppBar.AppBarDockMode.Right;
            }
        }

        //----------------------------------------------------------------------
        public SolidColorBrush KnobBrush
        {
            get { return new SolidColorBrush(this.model.KnobColor); }
        }

        //----------------------------------------------------------------------
        public ColorValues KnobColor { get; }


        //----------------------------------------------------------------------
        public string TreeWidthAsText
        {
            get { return this.model.TreeWidth.ToString(); }
            set { TreeWidth = ConvertTextToNumber(value); }
        }

        //----------------------------------------------------------------------
        public string KnobWidthAsText
        {
            get { return this.model.KnobWidth.ToString(); }
            set { KnobWidth = ConvertTextToNumber(value); }
        }

        //----------------------------------------------------------------------
        public Selector<string> PositionSelector
        {
            get
            {
                this.positionSelector.Index =
                    this.positionSelector.List.IndexOf(this.model.DockPosition.ToString());

                return this.positionSelector;
            }
        }

        //----------------------------------------------------------------------
        public Selector<string> DisplaySelector
        {
            get
            {
                this.displaySelector.Current = this.displaySelector.List[this.model.DisplayIndex];
                return this.displaySelector;
            }
        }


        //======================================================================
        public Settings(Model.Settings model)
        {
            this.model = model;

            this.KnobColor = new ColorValues(model.KnobColor);
            this.KnobColor.PropertyChanged += this.KnobColor_Changed;

            var positions = new List<string>
            {
                Model.DockPosition.Left .ToString(),
                Model.DockPosition.Right.ToString(),
            };

            this.positionSelector = new Selector<string>(positions);
            this.positionSelector.PropertyChanged += Position_Changed;

            var displays = new List<string>();

            using (var monitors = WpfAppBar.MonitorInfo.GetAllMonitors().GetEnumerator())
            {
                int count = 0;
                while (monitors.MoveNext())
                {
                    count++;
                    displays.Add("Display " + count.ToString());
                }
            }

            this.displaySelector = new Selector<string>(displays);
            this.displaySelector.PropertyChanged += this.Display_Changed;
        }

        //======================================================================
        public void Save()
        {
            this.model.Save();
        }

        //======================================================================
        private void Display_Changed(object sender, PropertyChangedEventArgs e)
        {
            this.model.DisplayIndex = this.displaySelector.Index;
            RaisePropertyChanged(nameof(this.DisplayToDock));
        }

        //======================================================================
        private void Position_Changed(object sender, PropertyChangedEventArgs e)
        {
            foreach (var pos in Enum.GetValues(typeof(Model.DockPosition)))
            {
                if (this.positionSelector.Current == pos.ToString())
                {
                    this.model.DockPosition = (Model.DockPosition)pos;
                    RaisePropertyChanged(nameof(this.DockPosition));
                    return;
                }
            }
        }


        //======================================================================
        private void KnobColor_Changed(object sender, PropertyChangedEventArgs e)
        {
            Color color = new Color();

            color.R = this.KnobColor.R;
            color.G = this.KnobColor.G;
            color.B = this.KnobColor.B;
            color.A = 255;

            this.model.KnobColor = color;
            RaisePropertyChanged(nameof(this.KnobBrush));
        }

        //======================================================================
        private int ConvertTextToNumber(string text)
        {
            text = Regex.Replace(text, @"\D", "");
            return int.Parse((text == "") ? "0" : text);
        }


        private Model.Settings model;
        private Selector<string> displaySelector;
        private Selector<string> positionSelector;
    }
}
