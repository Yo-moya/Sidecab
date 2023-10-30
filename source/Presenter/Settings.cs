
using System;
using System.ComponentModel;
using System.Windows.Media;

namespace Sidecab.Presenter
{
    public class Settings : ObservableObject
    {
        public int TreeWidthMin { get { return 100; } }
        public int TreeFontSizeMin { get { return 4; } }

        //----------------------------------------------------------------------
        public int TreeWidth
        {
            get { return this.model.TreeWidth; }
            set
            {
                this.model.TreeWidth = Math.Max(this.TreeWidthMin, value);
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
        public int TreeFontSize
        {
            get { return this.model.TreeFontSize; }
            set
            {
                this.model.TreeFontSize = Math.Max(this.TreeFontSizeMin, value);
                RaisePropertyChanged(nameof(this.TreeFontSize));

                if (this.model.TreeFontSize > this.model.TreeFontSizeLarge)
                {
                    this.model.TreeFontSizeLarge = this.model.TreeFontSize;
                    RaisePropertyChanged(nameof(this.TreeFontSizeLarge));
                }
            }
        }

        //----------------------------------------------------------------------
        public int TreeFontSizeLarge
        {
            get { return this.model.TreeFontSizeLarge; }
            set
            {
                this.model.TreeFontSizeLarge = Math.Max(this.TreeFontSize, value);
                RaisePropertyChanged(nameof(this.TreeFontSizeLarge));
            }
        }

        //----------------------------------------------------------------------
        public int DisplayIndex
        {
            get { return this.model.DisplayIndex; }
            set
            {
                this.model.DisplayIndex = value;
                RaisePropertyChanged(nameof(this.DisplayIndex));
            }
        }

        //----------------------------------------------------------------------
        public Type.DockPosition DockPosition
        {
            get { return this.model.DockPosition; }
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
        public Type.ColorValues KnobColor { get; }


        //======================================================================
        public Settings(Model.Settings model)
        {
            this.model = model;

            this.KnobColor = new Type.ColorValues(model.KnobColor);
            this.KnobColor.PropertyChanged += this.KnobColor_Changed;
        }

        //======================================================================
        public async void Load()
        {
            await this.model.Load();
            RaiseAllPropertiesChanged();
        }

        //======================================================================
        public async void Save()
        {
            await this.model.Save();
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
