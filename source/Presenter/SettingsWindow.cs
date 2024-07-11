
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace Sidecab.Presenter
{
    public partial class SettingsWindow : ObservableObject
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
        public string FolderNameFontSizeMaxAsText
        {
            get => Settings.FolderNameFontSizeMax.ToString();
            set => Settings.FolderNameFontSizeMax = ConvertTextToNumber(value);
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

        private readonly Selector<string> _dockPositionSelector;

        [GeneratedRegex(@"\D")]
        private static partial Regex NonDigitFinder();


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

            _dockPositionSelector = new Selector<string>(dockPositionList);
            _dockPositionSelector.PropertyChanged += OnDockPositionChanged;
        }

        //----------------------------------------------------------------------
        public void RefreshView()
        {
            RaiseAllPropertiesChanged();
        }

        //----------------------------------------------------------------------
        public static void CorrectTextBoxInput(TextBox textBox, string input)
        {
            var selection = textBox.SelectionStart;
            var corrected = NonDigitFinder().Replace(input, string.Empty);

            if (corrected.Length > 0)
            {
                var wholeText = textBox.Text
                    .Remove(selection, textBox.SelectionLength)
                    .Insert(selection, corrected);

                textBox.Text = wholeText;
                textBox.SelectionStart = selection + corrected.Length;
                textBox.SelectionLength = 0;
            }
        }

        //----------------------------------------------------------------------
        public static void UpdateTextBoxSource(TextBox textBox)
        {
            var binding = textBox.GetBindingExpression(TextBox.TextProperty);
            binding?.UpdateSource();
        }


        //----------------------------------------------------------------------
        private static int ConvertTextToNumber(string text)
        {
            try
            {
                var corrected = NonDigitFinder().Replace(text, string.Empty);
                return int.Parse((corrected.Length == 0) ? "0" : corrected);
            }
            catch
            {
                return 0;
            }
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

            if (e.PropertyName == nameof(Settings.DockPosition))
            {
                DockPositionSelector.Index = (int)Settings.DockPosition;
                return;
            }
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
