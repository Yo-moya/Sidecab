
using System.ComponentModel;
using System.Windows.Media;

namespace Sidecab.Presenter
{
    public class Settings : Base
    {
        //----------------------------------------------------------------------
        public double KnobWidth
        {
            get { return _model.KnobWidth; }
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


        private Model.Settings _model;
        private ColorValues _knobColorValues;
    }
}
