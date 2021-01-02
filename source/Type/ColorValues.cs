
using System.Windows.Media;

namespace Sidecab.Type
{
    public class ColorValues : Utility.ObserverableObject
    {
        //----------------------------------------------------------------------
        public byte R
        {
            get { return this.r; }
            set
            {
                this.r = value;
                RaisePropertyChanged();
            }
        }

        //----------------------------------------------------------------------
        public byte G
        {
            get { return this.g; }
            set
            {
                this.g = value;
                RaisePropertyChanged();
            }
        }

        //----------------------------------------------------------------------
        public byte B
        {
            get { return this.b; }
            set
            {
                this.b = value;
                RaisePropertyChanged();
            }
        }


        //======================================================================
        public ColorValues(Color color)
        {
            this.r = color.R;
            this.g = color.G;
            this.b = color.B;
        }


        private byte r = 255;
        private byte g = 255;
        private byte b = 255;
    }
}
