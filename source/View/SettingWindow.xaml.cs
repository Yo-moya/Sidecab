
using System;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sidecab.View
{
    public partial class SettingWindow : Window
    {
        //======================================================================
        public SettingWindow()
        {
            InitializeComponent();

            DataObject.AddPastingHandler(textBox_TreeWidth, TextBox_Pasting);
            DataObject.AddPastingHandler(textBox_KnobWidth, TextBox_Pasting);

            DataContext = App.Presenter.Settings;
            popup_KnobColor.DataContext = App.Presenter.Settings.KnobColor;
        }

        //======================================================================
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            DataObject.RemovePastingHandler(textBox_TreeWidth, TextBox_Pasting);
            DataObject.RemovePastingHandler(textBox_KnobWidth, TextBox_Pasting);

            var mainWindow = App.Current.MainWindow as MainWindow;
            mainWindow?.NotifyChildWindowClosing(this);
        }

        //======================================================================
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }

        //======================================================================
        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            Utility.WindowAttributes.HideMinimizeButton(this);
        }

        //======================================================================
        private void button_KnobColor_Click(object sender, RoutedEventArgs e)
        {
            popup_KnobColor.IsOpen = true;
        }

        //======================================================================
        private void TextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null) return;

            var pasted = e.SourceDataObject.GetData(DataFormats.UnicodeText) as string;
            if (pasted == null) return;

            var selected = textBox.SelectionStart;

            var newText = Regex.Replace(pasted, @"\D", "");
            var allText = textBox.Text
                .Remove(textBox.SelectionStart, textBox.SelectionLength)
                .Insert(textBox.SelectionStart, newText);

            e.CancelCommand();
            e.Handled = true;

            textBox.Text = allText;
            textBox.SelectionStart = selected + newText.Length;
            textBox.SelectionLength = 0;
        }

        //======================================================================
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null) return;

            var selected = textBox.SelectionStart;

            var newText = Regex.Replace(e.Text, @"\D", "");
            var allText = textBox.Text
                .Remove(textBox.SelectionStart, textBox.SelectionLength)
                .Insert(textBox.SelectionStart, newText);

            e.Handled = true;

            textBox.Text = allText;
            textBox.SelectionStart = selected + newText.Length;
            textBox.SelectionLength = 0;
        }
    }
}
