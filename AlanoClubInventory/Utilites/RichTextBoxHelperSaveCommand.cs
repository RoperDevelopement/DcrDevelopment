using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
namespace AlanoClubInventory.Utilites
{
    
    
    public static class RichTextBoxHelperSaveCommand
    {
        public static readonly DependencyProperty SaveCommandProperty =
            DependencyProperty.RegisterAttached(
                "SaveCommand",
                typeof(ICommand),
                typeof(RichTextBoxHelperSaveCommand),
                new PropertyMetadata(null, OnSaveCommandChanged));

        public static void SetSaveCommand(DependencyObject element, ICommand value) =>
            element.SetValue(SaveCommandProperty, value);

        public static ICommand GetSaveCommand(DependencyObject element) =>
            (ICommand)element.GetValue(SaveCommandProperty);

        private static void OnSaveCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RichTextBox rtb && e.NewValue is ICommand cmd)
            {
                // Hook LostFocus if you want auto‑save too
                rtb.LostFocus -= Rtb_LostFocus;
                rtb.LostFocus += Rtb_LostFocus;

                // Optionally, you can trigger SaveCommand manually from ViewModel
                // by calling RichTextBoxHelper.UpdateContent(rtb) before executing cmd
            }
        }

        private static void Rtb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is RichTextBox rtb)
            {
                var serialized = SerializeDocument(rtb);
                var cmd = GetSaveCommand(rtb);
                if (cmd?.CanExecute(serialized) == true)
                    cmd.Execute(serialized);
            }
        }

        public static string SerializeDocument(RichTextBox rtb)
        {
            // Example: default to XAML serialization
            using var stream = new MemoryStream();
            XamlWriter.Save(rtb.Document, stream);
            stream.Position = 0;
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }

}
