
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
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
        public SolidColorBrush KnobBrush
        {
            get { return new SolidColorBrush(_model.KnobColor); }
        }

        //----------------------------------------------------------------------
        public ColorValues KnobColor
        {
            get { return _knobColorValues; }
        }


        //======================================================================
        public Settings(Model.Settings model)
        {
            _model = model;

            _knobColorValues = new ColorValues(model.KnobColor);
            _knobColorValues.PropertyChanged += KnobColor_Changed;
        }

        //======================================================================
        private void KnobColor_Changed(object sender, PropertyChangedEventArgs e)
        {
            Color color;

            color.R = _knobColorValues.R;
            color.G = _knobColorValues.G;
            color.B = _knobColorValues.B;
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
        private ColorValues _knobColorValues;
    }
}
