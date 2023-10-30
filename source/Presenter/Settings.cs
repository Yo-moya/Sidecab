
using System;

namespace Sidecab.Presenter
{
    public class Settings : ObservableObject
    {
        public int TreeWidthMin => 100;
        public int TreeFontSizeMin => 4;

        //----------------------------------------------------------------------
        public int TreeWidth
        {
            get => _model.TreeWidth;
            set
            {
                _model.TreeWidth = Math.Max(TreeWidthMin, value);
                RaisePropertyChanged();
            }
        }

        //----------------------------------------------------------------------
        public int TreeFontSize
        {
            get => _model.TreeFontSize;
            set
            {
                _model.TreeFontSize = Math.Max(TreeFontSizeMin, value);
                RaisePropertyChanged();

                if (TreeFontSize > TreeFontSizeLarge)
                {
                    TreeFontSizeLarge = TreeFontSize;
                }
            }
        }

        //----------------------------------------------------------------------
        public int TreeFontSizeLarge
        {
            get => _model.TreeFontSizeLarge;
            set
            {
                _model.TreeFontSizeLarge = Math.Max(TreeFontSize, value);
                RaisePropertyChanged();
            }
        }

        //----------------------------------------------------------------------
        public int DisplayIndex
        {
            get => _model.DisplayIndex;
            set
            {
                _model.DisplayIndex = value;
                RaisePropertyChanged();
            }
        }

        //----------------------------------------------------------------------
        public Type.DockPosition DockPosition
        {
            get => _model.DockPosition;
            set
            {
                _model.DockPosition = value;
                RaisePropertyChanged();
            }
        }


        //----------------------------------------------------------------------
        public Settings(Model.Settings model)
        {
            _model = model;
        }

        //----------------------------------------------------------------------
        public async void Load()
        {
            await _model.Load();
            RaiseAllPropertiesChanged();
        }

        //----------------------------------------------------------------------
        public async void Save()
        {
            await _model.Save();
        }


        private Model.Settings _model;
    }
}
