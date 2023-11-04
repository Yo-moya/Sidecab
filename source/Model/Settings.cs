
using System;

namespace Sidecab.Model
{
    //==========================================================================
    public interface ISettingsFile
    {
        Settings Load();
        bool Save(Settings settings);
    }


    //==========================================================================
    public sealed class Settings
    {
        public static int MinTreeWidth => 100;
        public static int MinFolderNameFontSize => 4;

        //----------------------------------------------------------------------
        public int TreeWidth
        {
            get => _treeWidth;
            set
            {
                _treeWidth = Math.Max(MinTreeWidth, value);
            }
        }

        //----------------------------------------------------------------------
        public int FolderNameFontSize
        {
            get => _folderNameFontSize;
            set
            {
                _folderNameFontSize = Math.Max(MinFolderNameFontSize, value);

                if (FolderNameFontSize > FolderNameFontSizeLarge)
                {
                    FolderNameFontSizeLarge = FolderNameFontSize;
                }
            }
        }

        //----------------------------------------------------------------------
        public int FolderNameFontSizeLarge
        {
            get => _folderNameFontSizeLarge;
            set
            {
                _folderNameFontSizeLarge = Math.Max(FolderNameFontSize, value);
            }
        }

        public Type.DockPosition DockPosition { get; set; }
        public int DisplayIndex { get; set; }


        private int _treeWidth = 200;
        private int _folderNameFontSize = 12;
        private int _folderNameFontSizeLarge = 12;


        //----------------------------------------------------------------------
        public static Settings Load(ISettingsFile fileIO)
        {
            return fileIO.Load() ?? new();
        }

        //----------------------------------------------------------------------
        public bool Save(ISettingsFile fileIO)
        {
            return fileIO.Save(this);
        }
    }
}
