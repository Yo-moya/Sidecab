
using System;
using System.ComponentModel;
using System.Windows.Media;

namespace Sidecab.Presenter
{
    public class Settings : Utility.NoticeableVariables
    {
        //----------------------------------------------------------------------
        public int TreeMinWidth
        {
            get { return 100; }
        }

        //----------------------------------------------------------------------
        public int TreeWidth
        {
            get { return this.model.TreeWidth; }
            set
            {
                this.model.TreeWidth = Math.Max(TreeMinWidth, value);
                RaisePropertyChanged(nameof(this.TreeWidth));
            }
        }

        //----------------------------------------------------------------------
        public int KnobWidth
        {
            get { return this.model.KnobWidth; }
            set
            {
                this.model.KnobWidth = Math.Max(1, value);
                RaisePropertyChanged(nameof(this.KnobWidth));
            }
        }

        //----------------------------------------------------------------------
        public int DisplayIndex
        {
            get
            {
                return this.model.DisplayIndex;
            }

            set
            {
                this.model.DisplayIndex = value;
                RaisePropertyChanged(nameof(this.DisplayIndex));
            }
        }

        //----------------------------------------------------------------------
        public Data.DockPosition DockPosition
        {
            get
            {
                return this.model.DockPosition;
            }

            set
            {
                this.model.DockPosition = value;
                RaisePropertyChanged(nameof(this.DockPosition));
            }
        }

        //----------------------------------------------------------------------
        public SolidColorBrush KnobBrush
        {
            get { return new SolidColorBrush(this.model.KnobColor); }
        }

        //----------------------------------------------------------------------
        public Data.ColorValues KnobColor { get; }


        //======================================================================
        public Settings(Model.Settings model)
        {
            this.model = model;

            this.KnobColor = new Data.ColorValues(model.KnobColor);
            this.KnobColor.PropertyChanged += this.KnobColor_Changed;
        }

        //======================================================================
        public void Save()
        {
            this.model.Save();
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


        private Model.Settings model;
    }
}
