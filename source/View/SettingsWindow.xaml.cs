
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
        //======================================================================
        public SettingsWindow()
        {
            InitializeComponent();

            DataObject.AddPastingHandler(this.textBox_TreeWidth, this.TextBox_Pasting);
            DataObject.AddPastingHandler(this.textBox_KnobWidth, this.TextBox_Pasting);

            this.DataContext = App.Presenter.Settings;
        }

        //======================================================================
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            DataObject.RemovePastingHandler(this.textBox_TreeWidth, this.TextBox_Pasting);
            DataObject.RemovePastingHandler(this.textBox_KnobWidth, this.TextBox_Pasting);

            (this.DataContext as Presenter.Settings)?.Save();
            (App.Current.MainWindow as MainWindow)?.NotifyChildWindowClosing(this);
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
            Utility.WindowAttributes.HideTitleBarIcon  (this);
            Utility.WindowAttributes.HideMinimizeButton(this);
        }

        //======================================================================
        private void button_KnobColor_Click(object sender, RoutedEventArgs e)
        {
            (App.Current.MainWindow as MainWindow)?.CloseTreeWindow();
            this.popup_KnobColor.IsOpen = true;
        }

        //======================================================================
        private void TextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            var pasted = e.SourceDataObject.GetData(DataFormats.UnicodeText) as string;
            if (pasted == null) return;

            if (CorrectInputText(sender, pasted))
            {
                e.CancelCommand();
                e.Handled = true;
            }
        }

        //======================================================================
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (CorrectInputText(sender, e.Text))
            {
                e.Handled = true;
            }
        }

        //======================================================================
        private bool CorrectInputText(object sender, string input)
        {
            var textBox = sender as TextBox;
            if (textBox == null) return false;

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

        //======================================================================
        private void KnobEditBox_GotFocus(object sender, RoutedEventArgs e)
        {
            (App.Current.MainWindow as MainWindow)?.CloseTreeWindow();
        }

        //======================================================================
        private void TreeEditBox_GotFocus(object sender, RoutedEventArgs e)
        {
            (App.Current.MainWindow as MainWindow)?.OpenTreeWindow(activate : false);
        }
    }
}
