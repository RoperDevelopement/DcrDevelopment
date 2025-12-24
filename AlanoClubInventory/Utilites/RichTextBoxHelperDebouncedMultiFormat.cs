using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Threading;
using System.Windows.Markup;
namespace AlanoClubInventory.Utilites
{
    public static class RichTextBoxHelperDebouncedMultiFormat
    {
        private static readonly Dictionary<RichTextBox, DispatcherTimer> _timers = new();

        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.RegisterAttached(
                "Content",
                typeof(string),
                typeof(RichTextBoxHelperDebouncedMultiFormat),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnContentChanged));

        public static readonly DependencyProperty FormatProperty =
            DependencyProperty.RegisterAttached(
                "Format",
                typeof(RichTextFormat),
                typeof(RichTextBoxHelperDebouncedMultiFormat),
                new PropertyMetadata(RichTextFormat.Xaml));

        public static void SetContent(DependencyObject element, string value) =>
            element.SetValue(ContentProperty, value);

        public static string GetContent(DependencyObject element) =>
            (string)element.GetValue(ContentProperty);

        public static void SetFormat(DependencyObject element, RichTextFormat value) =>
            element.SetValue(FormatProperty, value);

        public static RichTextFormat GetFormat(DependencyObject element) =>
            (RichTextFormat)element.GetValue(FormatProperty);

        private static void OnContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RichTextBox rtb)
            {
                rtb.TextChanged -= Rtb_TextChanged;
                rtb.TextChanged += Rtb_TextChanged;

                // Initialize debounce timer if not already
                if (!_timers.ContainsKey(rtb))
                {
                    var timer = new DispatcherTimer
                    {
                        Interval = TimeSpan.FromMilliseconds(400) // debounce interval
                    };
                    timer.Tick += (s, args) =>
                    {
                        timer.Stop();
                        UpdateContent(rtb);
                    };
                    _timers[rtb] = timer;
                }

                // Load initial content
                var format = GetFormat(rtb);
                var content = e.NewValue as string;
                if (string.IsNullOrEmpty(content))
                {
                    rtb.Document = new FlowDocument();
                }
                else
                {
                    switch (format)
                    {
                        case RichTextFormat.Xaml:
                            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(content)))
                                rtb.Document = (FlowDocument)XamlReader.Load(stream);
                            break;
                        case RichTextFormat.Rtf:
                            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(content)))
                            {
                                var range = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
                                range.Load(stream, DataFormats.Rtf);
                            }
                            break;
                        case RichTextFormat.PlainText:
                            rtb.Document = new FlowDocument(new Paragraph(new Run(content)));
                            break;
                    }
                }
            }
        }

        private static void Rtb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is RichTextBox rtb && _timers.TryGetValue(rtb, out var timer))
            {
                // Restart debounce timer
                timer.Stop();
                timer.Start();
            }
        }

        private static void UpdateContent(RichTextBox rtb)
        {
            var format = GetFormat(rtb);
            string content = string.Empty;

            switch (format)
            {
                case RichTextFormat.Xaml:
                    using (var stream = new MemoryStream())
                    {
                        XamlWriter.Save(rtb.Document, stream);
                        stream.Position = 0;
                        using var reader = new StreamReader(stream);
                        content = reader.ReadToEnd();
                    }
                    break;
                case RichTextFormat.Rtf:
                    using (var stream = new MemoryStream())
                    {
                        var range = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
                        range.Save(stream, DataFormats.Rtf);
                        stream.Position = 0;
                        using var reader = new StreamReader(stream);
                        content = reader.ReadToEnd();
                    }
                    break;
                case RichTextFormat.PlainText:
                    content = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd).Text;
                    break;
            }

            SetContent(rtb, content);
        }
    }

}
