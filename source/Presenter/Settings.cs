
using System;
using System.Threading.Tasks;

namespace Sidecab.Presenter
{
    public sealed class Settings : ObservableObject
    {
        public event Action? Loaded;


        //----------------------------------------------------------------------
        public int TreeWidth
        {
            get => _model.TreeWidth;
            set => ModifyNestedProperty(_model, nameof(Model.Settings.TreeWidth), value);
        }

        //----------------------------------------------------------------------
        public int FolderNameFontSize
        {
            get => _model.FolderNameFontSize;
            set
            {
                if (ModifyNestedProperty(_model, nameof(Model.Settings.FolderNameFontSize), value))
                {
                    RaisePropertyChanged(nameof(FolderNameFontSizeMax));// has potential for adjustment
                }
            }
        }

        //----------------------------------------------------------------------
        public int FolderNameFontSizeMax
        {
            get => _model.FolderNameFontSizeMax;
            set => ModifyNestedProperty(_model, nameof(Model.Settings.FolderNameFontSizeMax), value);
        }

        //----------------------------------------------------------------------
        public int DisplayIndex
        {
            get => _model.DisplayIndex;
            set => ModifyNestedProperty(_model, nameof(Model.Settings.DisplayIndex), value);
        }

        //----------------------------------------------------------------------
        public Type.DockPosition DockPosition
        {
            get => _model.DockPosition;
            set => ModifyNestedProperty(_model, nameof(Model.Settings.DockPosition), value);
        }


        private Model.Settings _model = new();


        //----------------------------------------------------------------------
        public async void LoadAsync()
        {
            var model = await Task.Run(() => Model.Settings.Load(new IO.SettingsJson()));
            if (model is not null)
            {
                _model = model;
                Loaded?.Invoke();
                RaiseAllPropertiesChanged();
            }
        }

        //----------------------------------------------------------------------
        public async Task<bool> SaveAsync()
        {
            return await Task.Run(() => _model.Save(new IO.SettingsJson()));
        }
    }
}
