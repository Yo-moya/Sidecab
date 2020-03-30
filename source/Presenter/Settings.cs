
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
            get { return _model.TreeWidth; }
            set
            {
                _model.TreeWidth = Math.Max(10, value);

                RaisePropertyChanged(nameof(TreeWidth));
                RaisePropertyChanged(nameof(TreeWidthAsText));
            }
        }

        //----------------------------------------------------------------------
        public int KnobWidth
        {
            get { return _model.KnobWidth; }
            set
            {
                _model.KnobWidth = Math.Max(1, value);

                RaisePropertyChanged(nameof(KnobWidth));
                RaisePropertyChanged(nameof(KnobWidthAsText));
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
                    if (index == _model.DisplayIndex) return m;
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
                return (_model.DockPosition == Model.DockPosition.Left)
                    ? WpfAppBar.AppBarDockMode.Left : WpfAppBar.AppBarDockMode.Right;
            }
        }

        //----------------------------------------------------------------------
        public SolidColorBrush KnobBrush
        {
            get { return new SolidColorBrush(_model.KnobColor); }
        }

        //----------------------------------------------------------------------
        public ColorValues KnobColor { get; }


        //----------------------------------------------------------------------
        public string TreeWidthAsText
        {
            get { return _model.TreeWidth.ToString(); }
            set { TreeWidth = ConvertTextToNumber(value); }
        }

        //----------------------------------------------------------------------
        public string KnobWidthAsText
        {
            get { return _model.KnobWidth.ToString(); }
            set { KnobWidth = ConvertTextToNumber(value); }
        }

        //----------------------------------------------------------------------
        public Selector<string> PositionSelector
        {
            get
            {
                _positionSelector.Index =
                    _positionSelector.List.IndexOf(_model.DockPosition.ToString());

                return _positionSelector;
            }
        }

        //----------------------------------------------------------------------
        public Selector<string> DisplaySelector
        {
            get
            {
                _displaySelector.Current = _displaySelector.List[_model.DisplayIndex];
                return _displaySelector;
            }
        }


        //======================================================================
        public Settings(Model.Settings model)
        {
            _model = model;

            KnobColor = new ColorValues(model.KnobColor);
            KnobColor.PropertyChanged += KnobColor_Changed;

            var positions = new List<string>
            {
                Model.DockPosition.Left .ToString(),
                Model.DockPosition.Right.ToString(),
            };

            _positionSelector = new Selector<string>(positions);
            _positionSelector.PropertyChanged += Position_Changed;

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

            _displaySelector = new Selector<string>(displays);
            _displaySelector.PropertyChanged += Display_Changed;
        }

        //======================================================================
        public void Save()
        {
            _model.Save();
        }

        //======================================================================
        private void Display_Changed(object sender, PropertyChangedEventArgs e)
        {
            _model.DisplayIndex = _displaySelector.Index;
            RaisePropertyChanged(nameof(DisplayToDock));
        }

        //======================================================================
        private void Position_Changed(object sender, PropertyChangedEventArgs e)
        {
            foreach (var pos in Enum.GetValues(typeof(Model.DockPosition)))
            {
                if (_positionSelector.Current == pos.ToString())
                {
                    _model.DockPosition = (Model.DockPosition)pos;
                    RaisePropertyChanged(nameof(DockPosition));
                    return;
                }
            }
        }


        //======================================================================
        private void KnobColor_Changed(object sender, PropertyChangedEventArgs e)
        {
            Color color;

            color.R = KnobColor.R;
            color.G = KnobColor.G;
            color.B = KnobColor.B;
            color.A = 255;

            _model.KnobColor = color;
            RaisePropertyChanged(nameof(KnobBrush));
        }

        //======================================================================
        private int ConvertTextToNumber(string text)
        {
            text = Regex.Replace(text, @"\D", "");
            return int.Parse((text == "") ? "0" : text);
        }


        private Model.Settings _model;
        private Selector<string> _displaySelector;
        private Selector<string> _positionSelector;
    }
}
