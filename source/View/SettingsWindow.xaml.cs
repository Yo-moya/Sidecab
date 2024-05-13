
using System;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sidecab.View
{
    public partial class SettingsWindow : Window
    {
        private readonly Presenter.SettingsWindow _presenter;


        //----------------------------------------------------------------------
        public SettingsWindow()
        {
            InitializeComponent();

            DataObject.AddPastingHandler(TextBox_TreeWidth, TextBox_Pasting);

            DataContext = _presenter = new();
            _presenter.RefreshView();
        }

        //----------------------------------------------------------------------
        private bool CorrectInputText(object sender, string input)
        {
            if (sender is not TextBox textBox) return false;

            var selection = textBox.SelectionStart;
            var corrected = Regex.Replace(input, @"\D", "");
            var wholeText = textBox.Text
                .Remove(textBox.SelectionStart, textBox.SelectionLength)
                .Insert(textBox.SelectionStart, corrected);

            textBox.Text = wholeText;
            textBox.SelectionStart = selection + corrected.Length;
            textBox.SelectionLength = 0;

            return true;
        }

        //----------------------------------------------------------------------
        private void UpdateTextBoxSource(object sender)
        {
            if (sender is TextBox textBox)
            {
                var binding = textBox.GetBindingExpression(TextBox.TextProperty);
                binding?.UpdateSource();
            }
        }

        //----------------------------------------------------------------------
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            DataObject.RemovePastingHandler(TextBox_TreeWidth, TextBox_Pasting);
            _ = App.Settings.SaveAsync();
        }

        //----------------------------------------------------------------------
        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            WindowAttributes.HideTitleBarIcon  (this);
            WindowAttributes.HideMinimizeButton(this);
        }

        //----------------------------------------------------------------------
        private void TextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.SourceDataObject.GetData(DataFormats.UnicodeText) is string pasted)
            {
                if (CorrectInputText(sender, pasted))
                {
                    e.CancelCommand();
                    e.Handled = true;
                }
            }
        }

        //----------------------------------------------------------------------
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (CorrectInputText(sender, e.Text))
            {
                e.Handled = true;
            }
        }

        //----------------------------------------------------------------------
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape :
                    _presenter.RefreshView();
                    break;

                case Key.Enter :
                    UpdateTextBoxSource(sender);
                    break;
            }
        }

        //----------------------------------------------------------------------
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            UpdateTextBoxSource(sender);
        }
    }
}
