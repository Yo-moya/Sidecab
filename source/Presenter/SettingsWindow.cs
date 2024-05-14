
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Sidecab.Presenter
{
    public class SettingsWindow : ObservableObject
    {
        public Settings Settings => App.Settings;

        //----------------------------------------------------------------------
        public string TreeWidthAsText
        {
            get => Settings.TreeWidth.ToString();
            set => Settings.TreeWidth = ConvertTextToNumber(value);
        }

        //----------------------------------------------------------------------
        public string FolderNameFontSizeAsText
        {
            get => Settings.FolderNameFontSize.ToString();
            set => Settings.FolderNameFontSize = ConvertTextToNumber(value);
        }

        //----------------------------------------------------------------------
        public string FolderNameFontSizeLargeAsText
        {
            get => Settings.FolderNameFontSizeMax.ToString();
            set => Settings.FolderNameFontSizeMax = ConvertTextToNumber(value);
        }

        //----------------------------------------------------------------------
        public Selector<string> DisplayIndexSelector
        {
            get
            {
                _displayIndexSelector.Current =
                    _displayIndexSelector.List[Settings.DisplayIndex];

                return _displayIndexSelector;
            }
        }

        //----------------------------------------------------------------------
        public Selector<string> DockPositionSelector
        {
            get
            {
                _dockPositionSelector.Index =
                    _dockPositionSelector.List.IndexOf(Settings.DockPosition.ToString());

                return _dockPositionSelector;
            }
        }

        private Selector<string> _displayIndexSelector;
        private Selector<string> _dockPositionSelector;


        //----------------------------------------------------------------------
        public SettingsWindow()
        {
            Settings.PropertyChanged += OnSettingChanged;

            var displayIndexList = new List<string>();
            var dockPositionList = new List<string>();

            foreach (var dockPos in Enum.GetValues(typeof(Type.DockPosition)))
            {
                dockPositionList.Add(dockPos?.ToString() ?? string.Empty);
            }

            _displayIndexSelector = new Selector<string>(displayIndexList);
            _displayIndexSelector.PropertyChanged += OnDisplayIndexChanged;

            _dockPositionSelector = new Selector<string>(dockPositionList);
            _dockPositionSelector.PropertyChanged += OnDockPositionChanged;
        }

        //----------------------------------------------------------------------
        public void RefreshView()
        {
            RaiseAllPropertiesChanged();
        }


        //----------------------------------------------------------------------
        private static int ConvertTextToNumber(string text)
        {
            text = Regex.Replace(text, @"\D", "");
            return int.Parse((text == "") ? "0" : text);
        }

        //----------------------------------------------------------------------
        private void OnSettingChanged(object? sender, PropertyChangedEventArgs e)
        {
            var property = GetType().GetProperty(e.PropertyName + "AsText");
            if (property is not null)
            {
                RaisePropertyChanged(property.Name);
                return;
            }

            if (e.PropertyName == nameof(Settings.DisplayIndex))
            {
                DisplayIndexSelector.Index = Settings.DisplayIndex;
                return;
            }

            if (e.PropertyName == nameof(Settings.DockPosition))
            {
                DockPositionSelector.Index = (int)Settings.DockPosition;
                return;
            }
        }

        //----------------------------------------------------------------------
        private void OnDisplayIndexChanged(object? sender, PropertyChangedEventArgs e)
        {
            Settings.DisplayIndex = _displayIndexSelector.Index;
        }

        //----------------------------------------------------------------------
        private void OnDockPositionChanged(object? sender, PropertyChangedEventArgs e)
        {
            foreach (var dockPos in Enum.GetValues(typeof(Type.DockPosition)))
            {
                if (_dockPositionSelector.Current == dockPos.ToString())
                {
                    Settings.DockPosition = (Type.DockPosition)dockPos;
                    return;
                }
            }
        }
    }
}
