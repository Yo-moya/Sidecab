
using System.Threading.Tasks;

namespace Sidecab.Presenter
{
    public class Settings : ObservableObject
    {
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
                    RaisePropertyChanged(nameof(FolderNameFontSizeLarge));// has potential for adjustment
                }
            }
        }

        //----------------------------------------------------------------------
        public int FolderNameFontSizeLarge
        {
            get => _model.FolderNameFontSizeLarge;
            set => ModifyNestedProperty(_model, nameof(Model.Settings.FolderNameFontSizeLarge), value);
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


        //----------------------------------------------------------------------
        public Settings(Model.Settings model)
        {
            _model = model;
        }

        //----------------------------------------------------------------------
        public async void LoadAsync()
        {
            _model = await Task.Run(() => Model.Settings.Load(new IO.SettingsJson()));
            RaiseAllPropertiesChanged();
        }

        //----------------------------------------------------------------------
        public async Task<bool> SaveAsync()
        {
            return await Task.Run(() => _model.Save(new IO.SettingsJson()));
        }


        private Model.Settings _model;
    }
}
