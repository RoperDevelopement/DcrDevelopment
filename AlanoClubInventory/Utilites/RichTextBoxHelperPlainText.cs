using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
namespace AlanoClubInventory.Utilites
{
        public static class RichTextBoxHelperPlainText
    {
        public static readonly DependencyProperty PlainTextProperty =
            DependencyProperty.RegisterAttached(
                "PlainText",
                typeof(string),
                typeof(RichTextBoxHelperPlainText),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPlainTextChanged));

        public static readonly DependencyProperty SerializedProperty =
            DependencyProperty.RegisterAttached(
                "Serialized",
                typeof(string),
                typeof(RichTextBoxHelperPlainText),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty FormatProperty =
            DependencyProperty.RegisterAttached(
                "Format",
                typeof(RichTextFormat),
                typeof(RichTextBoxHelperPlainText),
                new PropertyMetadata(RichTextFormat.Xaml));

        public static void SetPlainText(DependencyObject element, string value) =>
            element.SetValue(PlainTextProperty, value);

        public static string GetPlainText(DependencyObject element) =>
            (string)element.GetValue(PlainTextProperty);

        public static void SetSerialized(DependencyObject element, string value) =>
            element.SetValue(SerializedProperty, value);

        public static string GetSerialized(DependencyObject element) =>
            (string)element.GetValue(SerializedProperty);

        public static void SetFormat(DependencyObject element, RichTextFormat value) =>
            element.SetValue(FormatProperty, value);

        public static RichTextFormat GetFormat(DependencyObject element) =>
            (RichTextFormat)element.GetValue(FormatProperty);

        private static void OnPlainTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RichTextBox rtb)
            {
                rtb.TextChanged -= Rtb_TextChanged;
                rtb.TextChanged += Rtb_TextChanged;

                var text = e.NewValue as string;
                rtb.Document = new FlowDocument(new Paragraph(new Run(text ?? string.Empty)));

                rtb.LostFocus -= Rtb_LostFocus;
                rtb.LostFocus += Rtb_LostFocus;
            }
        }

        private static void Rtb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is RichTextBox rtb)
            {
                var text = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd).Text;
                SetPlainText(rtb, text);
            }
        }

        private static void Rtb_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is RichTextBox rtb)
            {
                var format = GetFormat(rtb);
                string serialized = string.Empty;

                switch (format)
                {
                    case RichTextFormat.Xaml:
                        using (var stream = new MemoryStream())
                        {
                            XamlWriter.Save(rtb.Document, stream);
                            stream.Position = 0;
                            using var reader = new StreamReader(stream);
                            serialized = reader.ReadToEnd();
                        }
                        break;
                    case RichTextFormat.Rtf:
                        using (var stream = new MemoryStream())
                        {
                            var range = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
                            range.Save(stream, DataFormats.Rtf);
                            stream.Position = 0;
                            using var reader = new StreamReader(stream);
                            serialized = reader.ReadToEnd();
                        }
                        break;
                }

                SetSerialized(rtb, serialized);
            }
        }
    }

}
