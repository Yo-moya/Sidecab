
using System.Runtime.InteropServices;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Sidecab.Presenter
{
    public class Folder : ObservableObject
    {
        public string Name => Model.Name;
        public string Path => Model.FetchFullPath();
        public bool HasSubFolders => Model.HasSubFolders;

        public ObservableCollection<Folder> SubFolders { get; } = [];

        //----------------------------------------------------------------------
        public double FontSize
        {
            get
            {
                var fontSize = App.Settings.FolderNameFontSize;
                var fontSizeMax = App.Settings.FolderNameFontSizeMax;
                var scale = Model?.GetFreshnessScale() ?? 0;
                return (fontSizeMax - fontSize) * scale + fontSize;
            }
        }


        protected Model.Folder Model { get; private init; }
        private static readonly Folder _dummyFolder = new();


        //----------------------------------------------------------------------
        public Folder(Model.Folder model)
        {
            Model = model;
            Model.FreshnessUpdated += OnFontSizeUpdated;

            if (HasSubFolders)
            {
                SubFolders.Add(_dummyFolder);
            }
        }

        //----------------------------------------------------------------------
        protected Folder(Folder other) : this(other.Model) {}
        private Folder() => Model = new(new());


        //----------------------------------------------------------------------
        public void StartRefreshChildren()
        {
            if (Monitor.TryEnter(SubFolders))
            {
                Task.Run(CollectSubFoldersOneByOne);
                Monitor.Exit(SubFolders);
            }
        }

        //----------------------------------------------------------------------
        public void Open() => Model.Open();
        public void CopyPath() => Model.CopyPath();


        //----------------------------------------------------------------------
        private void OnFontSizeUpdated()
        {
            RaisePropertyChanged(nameof(FontSize));
        }

        //----------------------------------------------------------------------
        private void CollectSubFoldersOneByOne()
        {
            Monitor.Enter(SubFolders);
            try
            {
                App.Current.Dispatcher.Invoke(() => SubFolders.Clear());

                foreach (var model in Model.SubFolders)
                {
                    AddFolderInOrder(new(model));
                }
            }
            finally
            {
                Monitor.Exit(SubFolders);
            }
        }

        //----------------------------------------------------------------------
        private void AddFolderInOrder(Folder subFolder)
        {
            for (int i = 0; i < SubFolders.Count; i++)
            {
                // Do natural numeric sorting
                // like "1", "2", "10"... instead of "1", "10", "2"...
                if (StrCmpLogicalW(SubFolders[i].Name, subFolder.Name) > 0)
                {
                    App.Current.Dispatcher.Invoke(() => SubFolders.Insert(i, subFolder));
                    return;
                }
            }

            App.Current.Dispatcher.Invoke(() => SubFolders.Add(subFolder));
        }

        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        private static extern int StrCmpLogicalW(string x, string y);
    }
}
