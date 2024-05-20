
using System;
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
                if (sender is TextBox textBox)
                {
                    Presenter.SettingsWindow.CorrectTextBoxInput(textBox, pasted);
                    e.CancelCommand();
                    e.Handled = true;
                }
            }
        }

        //----------------------------------------------------------------------
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                Presenter.SettingsWindow.CorrectTextBoxInput(textBox, e.Text);
                e.Handled = true;
            }
        }

        //----------------------------------------------------------------------
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape :
                {
                    _presenter.RefreshView();
                    break;
                }
                case Key.Enter :
                {
                    if (sender is TextBox textBox)
                    {
                        Presenter.SettingsWindow.UpdateTextBoxSource(textBox);
                    }
                    break;
                }
            }
        }

        //----------------------------------------------------------------------
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                Presenter.SettingsWindow.UpdateTextBoxSource(textBox);
            }
        }

    }
}
