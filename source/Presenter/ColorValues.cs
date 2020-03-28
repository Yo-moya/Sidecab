
using System.Windows.Media;

namespace Sidecab.Presenter
{
    public class ColorValues : Base
    {
        //----------------------------------------------------------------------
        public byte R
        {
            get { return _r; }
            set
            {
                _r = value;
                RaisePropertyChanged();
            }
        }

        //----------------------------------------------------------------------
        public byte G
        {
            get { return _g; }
            set
            {
                _g = value;
                RaisePropertyChanged();
            }
        }

        //----------------------------------------------------------------------
        public byte B
        {
            get { return _b; }
            set
            {
                _b = value;
                RaisePropertyChanged();
            }
        }


        //======================================================================
        public ColorValues(Color color)
        {
            _r = color.R;
            _g = color.G;
            _b = color.B;
        }


        private byte _r = 255;
        private byte _g = 255;
        private byte _b = 255;
    }
}
