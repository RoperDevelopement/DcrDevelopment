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
  
    public static class RichTextBoxHelperXAML
    {
        public static readonly DependencyProperty XamlProperty =
            DependencyProperty.RegisterAttached(
                "Xaml",
                typeof(string),
                typeof(RichTextBoxHelperXAML),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnXamlChanged));

        public static void SetXaml(DependencyObject element, string value)
            => element.SetValue(XamlProperty, value);

        public static string GetXaml(DependencyObject element)
            => (string)element.GetValue(XamlProperty);

        private static void OnXamlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RichTextBox rtb)
            {
                rtb.TextChanged -= Rtb_TextChanged;

                var xaml = e.NewValue as string;
                if (!string.IsNullOrEmpty(xaml))
                {
                    using var stream = new MemoryStream(Encoding.UTF8.GetBytes(xaml));
                    var doc = (FlowDocument)XamlReader.Load(stream);
                    rtb.Document = doc;
                }
                else
                {
                    rtb.Document = new FlowDocument();
                }

                rtb.TextChanged += Rtb_TextChanged;
            }
        }

        private static void Rtb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is RichTextBox rtb)
            {
                using var stream = new MemoryStream();
                XamlWriter.Save(rtb.Document, stream);
                stream.Position = 0;
                using var reader = new StreamReader(stream);
                var xaml = reader.ReadToEnd();

                SetXaml(rtb, xaml);
            }
        }
    }

}
