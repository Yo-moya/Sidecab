
using System;

namespace Sidecab.Model
{
    //==========================================================================
    public interface ISettingsFile
    {
        Settings? Load();
        bool Save(Settings settings);
    }


    //==========================================================================
    public sealed class Settings
    {
        public static int TreeWidthMin => 100;
        public static int FolderNameFontSizeMin => 4;

        //----------------------------------------------------------------------
        public int TreeWidth
        {
            get => _treeWidth;
            set
            {
                _treeWidth = Math.Max(TreeWidthMin, value);
            }
        }

        //----------------------------------------------------------------------
        public int FolderNameFontSize
        {
            get => _folderNameFontSize;
            set
            {
                _folderNameFontSize = Math.Max(FolderNameFontSizeMin, value);
                if (FolderNameFontSize > FolderNameFontSizeMax)
                {
                    FolderNameFontSizeMax = FolderNameFontSize;
                }
            }
        }

        //----------------------------------------------------------------------
        public int FolderNameFontSizeMax
        {
            get => _folderNameFontSizeMax;
            set
            {
                _folderNameFontSizeMax = Math.Max(FolderNameFontSize, value);
            }
        }

        public Type.DockPosition DockPosition { get; set; }
        public int DisplayIndex { get; set; }


        private int _treeWidth = 200;
        private int _folderNameFontSize = 12;
        private int _folderNameFontSizeMax = 14;


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
