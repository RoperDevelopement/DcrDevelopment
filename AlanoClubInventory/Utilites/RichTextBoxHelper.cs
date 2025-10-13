using Microsoft.ReportingServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

namespace AlanoClubInventory.Utilites
{
    public static class RichTextBoxHelper
    {
        // Attached property for binding FlowDocument
        public static readonly DependencyProperty BoundDocumentProperty =
            DependencyProperty.RegisterAttached(
                "BoundDocument",
                typeof(FlowDocument),
                typeof(RichTextBoxHelper),
                new FrameworkPropertyMetadata(null, OnBoundDocumentChanged));

        public static FlowDocument GetBoundDocument(DependencyObject obj)
        {
            return (FlowDocument)obj.GetValue(BoundDocumentProperty);
        }

        public static void SetBoundDocument(DependencyObject obj, FlowDocument value)
        {
            obj.SetValue(BoundDocumentProperty, value);
        }

        private static void OnBoundDocumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RichTextBox richTextBox)
           {
                // Unsubscribe from previous event
                richTextBox.TextChanged -= RichTextBox_TextChanged;

                // Set the new FlowDocument
                var newDocument = e.NewValue as FlowDocument;
                richTextBox.Document = newDocument;
                TextRange textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                // Subscribe to TextChanged to update the binding source
                if (newDocument != null)
                {
                    richTextBox.TextChanged += RichTextBox_TextChanged;
                }
            }
        }

        private static void RichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is RichTextBox richTextBox)
            {
                // Update the binding source
                var document = richTextBox.Document;
                var binding = BindingOperations.GetBindingExpression(richTextBox, BoundDocumentProperty);
                binding?.UpdateSource();
            }
        }
    }

}
