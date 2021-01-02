
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

            DataObject.AddPastingHandler(this.textBox_TreeWidth, this.textBox_Pasting);
            DataObject.AddPastingHandler(this.textBox_KnobWidth, this.textBox_Pasting);

            var presenter = new Presenter.SettingsWindow();
            this.DataContext = presenter;
            presenter.RefreshView();
        }

        //======================================================================
        private bool CorrectInputText(object sender, string input)
        {
            var textBox = sender as TextBox;
            if (textBox is null) return false;

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
        private void UpdateTextBoxSource(object sender)
        {
            if (sender is TextBox textBox)
            {
                var binding = textBox.GetBindingExpression(TextBox.TextProperty);
                binding?.UpdateSource();
            }
        }


        //======================================================================
        private void window_Closing(object sender, CancelEventArgs e)
        {
            DataObject.RemovePastingHandler(this.textBox_TreeWidth, this.textBox_Pasting);
            DataObject.RemovePastingHandler(this.textBox_KnobWidth, this.textBox_Pasting);

            App.Core.Settings.Save();

            // Close this window if the MainWindow is lost
            var result = (App.Current.MainWindow as MainWindow)
                ?.NotifyChildWindowClosing(this) ?? MainWindow.WindowBehaviorRestriction.CanClose;

            if (result == MainWindow.WindowBehaviorRestriction.CanNotClose)
            {
                e.Cancel = true;
            }
        }

        //======================================================================
        private void window_SourceInitialized(object sender, EventArgs e)
        {
            Utility.WindowAttributes.HideTitleBarIcon  (this);
            Utility.WindowAttributes.HideMinimizeButton(this);
        }

        //======================================================================
        private void button_KnobColor_Click(object sender, RoutedEventArgs e)
        {
            (App.Current.MainWindow as MainWindow)?.CloseFileTreeWindow();
            this.popup_KnobColor.IsOpen = true;
        }

        //======================================================================
        private void textBox_Pasting(object sender, DataObjectPastingEventArgs e)
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

        //======================================================================
        private void textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (CorrectInputText(sender, e.Text))
            {
                e.Handled = true;
            }
        }

        //======================================================================
        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                //--------------------------------------------------------------
                case Key.Escape :
                {
                    (this.DataContext as Presenter.SettingsWindow)?.RefreshView();
                    break;
                }
                //--------------------------------------------------------------
                case Key.Enter :
                {
                    UpdateTextBoxSource(sender);
                    break;
                }
                //--------------------------------------------------------------
            }
        }

        //======================================================================
        private void control_Knob_GotFocus(object sender, RoutedEventArgs e)
        {
            (App.Current.MainWindow as MainWindow)?.CloseFileTreeWindow();
        }

        //======================================================================
        private void control_Tree_GotFocus(object sender, RoutedEventArgs e)
        {
            (App.Current.MainWindow as MainWindow)?.OpenFileTreeWindow(activate : false);
        }

        //======================================================================
        private void textBox_LostFocus(object sender, RoutedEventArgs e)
        {
            UpdateTextBoxSource(sender);
        }
    }
}
