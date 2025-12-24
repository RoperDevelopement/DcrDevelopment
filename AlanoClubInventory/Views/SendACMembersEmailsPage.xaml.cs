using AlanoClubInventory.Interfaces;
using AlanoClubInventory.ViewModels;
using Microsoft.ReportingServices.Interfaces;
using PdfSharp.Pdf.Advanced;
using PdfSharp.Pdf.Filters;
using ScottPlot;
using ScottPlot.NamedColors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
//using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
//using System.Drawing;

//using System.Drawing;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
//freeicons https://www.flaticon.com/
namespace AlanoClubInventory.Views
{
    /// <summary>
    /// Interaction logic for CreateAlanoCLubNewsLettersPage.xaml
    /// </summary>
    public partial class SendACMembersEmailsPage : Page
    {
        private readonly SendACMembersEmailsViewModel viewModel = new SendACMembersEmailsViewModel();
        public SendACMembersEmailsPage()
        {
            InitializeComponent();
            DataContext = viewModel;

            FontFamilyCombo.SelectedIndex = 0;
            FontSizeCombo.SelectedIndex = 3;
            AlignLeftButton.IsChecked = true;
            MyRichTextBox.AppendText($"Dear Alano Club Member,{Utilites.AlanoCLubConstProp.CRLF}");
            MyRichTextBox.Focus();
            ColorComboBox.Items.Add(new ComboBoxItem { Content = "Text Color", Background = Brushes.White, Foreground = Brushes.Black });
            GetColors();
            ColorComboBox.SelectedIndex = 0;
            LineSpacingCombo.SelectedIndex = 0;

            //  FDPar.Margin = new Thickness(10,10,40,10);

        }
        private async void GetColors()
        {
            var brushProperties = typeof(Brushes).GetProperties(BindingFlags.Static | BindingFlags.Public);
            foreach (var property in brushProperties)
            {
                Brush brush = (Brush)property.GetValue(null);
                // Get the Brush object
                //var brush = (Brushes)property.GetValue(null);

                string colorName = property.Name;
                // Add the color name to the ComboBox
                ColorComboBox.Items.Add(new ComboBoxItem { Content = colorName, Background = brush, Foreground = Brushes.Black });

            }


        }
        private async void SelectALL(System.Windows.Controls.ListBox listBox)
        {
            viewModel.ACMembers.RemoveAt(0);
            viewModel.ACMembers.Insert(0, "UnSelect All Members");
            listBox.SelectedItems.Clear();

            // Select everything except the first item
            for (int i = 1; i < listBox.Items.Count; i++)
            {
                listBox.SelectedItems.Add(listBox.Items[i]);
            }
            // listBox.SelectAll();
        }
        private async void UnSelectALL(System.Windows.Controls.ListBox listBox)
        {
            viewModel.ACMembers.RemoveAt(0);
            viewModel.ACMembers.Insert(0, "Select All Members");
            listBox.SelectedItems.Clear();
            listBox.UnselectAll();
        }
        private async void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // var vm = (MainViewModel)DataContext;
            var listBox = (System.Windows.Controls.ListBox)sender;
            var selItem = listBox.SelectedItem as string;
            if (!(string.IsNullOrEmpty(selItem)))
            {
                // Clear current selection first
                if (string.Compare(selItem, "Select All Members", true) == 0)
                {
                    SelectALL(listBox);
                }
                else
                {
                    if (string.Compare(selItem, "UnSelect All Members", true) == 0)
                    {
                        UnSelectALL(listBox);
                    }
                }
            }
            // Select all items whenever selection changes

            viewModel.UpdateSelectedSummary(((System.Windows.Controls.ListBox)sender).SelectedItems);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var textRange = new TextRange(MyRichTextBox.Document.ContentStart, MyRichTextBox.Document.ContentEnd);
            //TestWindow testWindow = new TestWindow();
            //testWindow.Owner = Window.GetWindow(this);
            //testWindow.ShowDialog();

        }

        // Font family
        private void FontFamilyCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FontFamilyCombo.SelectedItem is ComboBoxItem item)
            {
                var text = new TextRange(MyRichTextBox.Document.ContentStart, MyRichTextBox.Document.ContentEnd);
                if (string.IsNullOrWhiteSpace(text.Text))
                {
                    MyRichTextBox.Selection.ApplyPropertyValue(
                    TextElement.FontFamilyProperty, new FontFamily(item.Content.ToString()));
                }
                else
                {
                    MyRichTextBox.Selection.ApplyPropertyValue(
                     TextElement.FontFamilyProperty, new FontFamily(item.Content.ToString()));
                    // MyRichTextBox.Selection.ApplyPropertyValue(Inline.FontStyleProperty, item.Content.ToString());
                }
            }
            MyRichTextBox.Focus();
        }

        // Font size
        private void FontSizeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FontSizeCombo.SelectedItem is ComboBoxItem item)
            {
                var rtb = MyRichTextBox;
                var range = new TextRange(rtb.Selection.Start, rtb.Selection.End);
                if (string.IsNullOrWhiteSpace(range.Text))
                {
                    MyRichTextBox.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, Convert.ToDouble(item.Content));

                }
                else
                    MyRichTextBox.Selection.ApplyPropertyValue(Inline.FontSizeProperty, Convert.ToDouble(item.Content));

                // range.ApplyPropertyValue(TextElement.FontSizeProperty, Convert.ToDouble(item.Content));
                //MyRichTextBox.Selection.ApplyPropertyValue(
                //  TextElement.FontSizeProperty, Convert.ToDouble(item.Content));

            }
            MyRichTextBox.Focus();
        }

        // Bold / Italic / Underline
        private void Bold_Click(object sender, RoutedEventArgs e)
        {
            MyRichTextBox.Selection.ApplyPropertyValue(TextElement.FontWeightProperty,
        BoldButton.IsChecked == true ? FontWeights.Bold : FontWeights.Normal);
            if ((bool)BoldButton.IsChecked)
            {
                BoldButton.Background = new SolidColorBrush(System.Windows.Media.Colors.Gray);
            }
            else
                BoldButton.Background = new SolidColorBrush(System.Windows.Media.Colors.White);
            MyRichTextBox.Focus();
        }


        private void Italic_Click(object sender, RoutedEventArgs e)
        {
            MyRichTextBox.Selection.ApplyPropertyValue(TextElement.FontStyleProperty,
        ItalicButton.IsChecked == true ? FontStyles.Italic : FontStyles.Normal);
            if ((bool)ItalicButton.IsChecked)
            {
                ItalicButton.Background = new SolidColorBrush(System.Windows.Media.Colors.Gray);
            }
            else
                ItalicButton.Background = new SolidColorBrush(System.Windows.Media.Colors.White);
            MyRichTextBox.Focus();
        }

        private void Underline_Click(object sender, RoutedEventArgs e)
        {
            MyRichTextBox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty,
    UnderlineButton.IsChecked == true ? TextDecorations.Underline : null);
            if ((bool)UnderlineButton.IsChecked)
            {
                UnderlineButton.Background = new SolidColorBrush(System.Windows.Media.Colors.Gray);
            }
            else
                UnderlineButton.Background = new SolidColorBrush(System.Windows.Media.Colors.White);
            MyRichTextBox.Focus();
        }


        // Text color
        private void Red_Click(object sender, RoutedEventArgs e) =>
            MyRichTextBox.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Red);

        private void Blue_Click(object sender, RoutedEventArgs e) =>
            MyRichTextBox.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Blue);

        // Alignment
        private void AlignLeft_Click(object sender, RoutedEventArgs e)
        {
            if (AlignCenterButton.IsChecked == true)
                AlignCenterButton.IsChecked = false;
            if (AlignRightButton.IsChecked == true)
                AlignRightButton.IsChecked = false;
            MyRichTextBox.Selection.ApplyPropertyValue(Paragraph.TextAlignmentProperty, TextAlignment.Left);
            AlignLeftButton.IsChecked = true;
            MyRichTextBox.Focus();


        }

        private void AlignCenter_Click(object sender, RoutedEventArgs e)
        {
            if (AlignLeftButton.IsChecked == true)
                AlignLeftButton.IsChecked = false;
            if (AlignRightButton.IsChecked == true)
                AlignRightButton.IsChecked = false;
            AlignCenterButton.IsChecked = true;
            MyRichTextBox.Selection.ApplyPropertyValue(Paragraph.TextAlignmentProperty, TextAlignment.Center);
            MyRichTextBox.Focus();
        }


        private void AlignRight_Click(object sender, RoutedEventArgs e)
        {
            if (AlignLeftButton.IsChecked == true)
                AlignLeftButton.IsChecked = false;
            if (AlignCenterButton.IsChecked == true)
                AlignCenterButton.IsChecked = false;
            AlignRightButton.IsChecked = true;
            MyRichTextBox.Selection.ApplyPropertyValue(Paragraph.TextAlignmentProperty, TextAlignment.Right);
            MyRichTextBox.Focus();
        }


        //private void MyRichTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        //{
        //    return;
        //    var selection = MyRichTextBox.Selection;

        //    // Bold
        //    object weight = selection.GetPropertyValue(TextElement.FontWeightProperty);
        //    BoldButton.IsChecked = (weight != DependencyProperty.UnsetValue && weight.Equals(FontWeights.Bold));

        //    // Italic
        //    object style = selection.GetPropertyValue(TextElement.FontStyleProperty);
        //    ItalicButton.IsChecked = (style != DependencyProperty.UnsetValue && style.Equals(FontStyles.Italic));

        //    // Underline
        //    object deco = selection.GetPropertyValue(Inline.TextDecorationsProperty);
        //    UnderlineButton.IsChecked = (deco != DependencyProperty.UnsetValue && deco.Equals(TextDecorations.Underline));
        //    object family = selection.GetPropertyValue(TextElement.FontFamilyProperty);
        //    if (family != DependencyProperty.UnsetValue)
        //        FontFamilyCombo.SelectedItem = FontFamilyCombo.Items
        //            .Cast<ComboBoxItem>()
        //            .FirstOrDefault(i => i.Content.ToString() == ((FontFamily)family).Source);
        //    object size = selection.GetPropertyValue(TextElement.FontSizeProperty);
        //    if (size != DependencyProperty.UnsetValue)
        //        FontSizeCombo.SelectedItem = FontSizeCombo.Items
        //            .Cast<ComboBoxItem>()
        //            .FirstOrDefault(i => i.Content.ToString() == ((double)size).ToString());

        //    // Alignment
        //    object align = selection.GetPropertyValue(Paragraph.TextAlignmentProperty);
        //    AlignLeftButton.IsChecked = (align != DependencyProperty.UnsetValue && align.Equals(TextAlignment.Left));
        //    AlignCenterButton.IsChecked = (align != DependencyProperty.UnsetValue && align.Equals(TextAlignment.Center));
        //    AlignRightButton.IsChecked = (align != DependencyProperty.UnsetValue && align.Equals(TextAlignment.Right));



        //}
        private async void BtnInsertTable(object sender, RoutedEventArgs e)
        {
            TableRowsColumsPage tableRows = new TableRowsColumsPage();
            tableRows.Owner = Window.GetWindow(this);
            tableRows.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            tableRows.Height = 250;
            tableRows.Width = 250;
            tableRows.Title = "Insert Table Rows and Colums";
            tableRows.WindowState = WindowState.Normal;
            tableRows.ResizeMode = ResizeMode.NoResize;
            
            var results = tableRows.ShowDialog();
            if (results == true)
            {
                 Utilites.RichTextBoxTableHelper.InsertTablePostion(  MyRichTextBox,tableRows.RowsCount, tableRows.ColumsCount, tableRows.HasHeader);
                MyRichTextBox.Focus();
                var position = MyRichTextBox.CaretPosition;
                position.InsertParagraphBreak();
                //InsertTablePostion(MyRichTextBox, tableRows.RowsCount, tableRows.ColumsCount, true);
                //  MyRichTextBox.Document = doc.Document;
                //   InsertTableWithBorders(MyRichTextBox);
                //  Utilites.ALanoClubUtilites.ShowMessageBox("Not Implemeted", "Not DOne", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                return;
        }
        private async void ButtonSave(object sender, RoutedEventArgs e)
        {
            Utilites.OpenFileDialog openFile = new Utilites.OpenFileDialog();
            await openFile.SaveFileRichText(MyRichTextBox, "Rich Text Format (*.rtf)|*.rtf|XAML (*.xaml)|*.xaml|Text Files (*.txt)|*.txt");
        }
        private async void BtnInsertImage(object sender, RoutedEventArgs e)
        {
            Utilites.OpenFileDialog openFile = new Utilites.OpenFileDialog();
            // Title = "Select an Image"
            var fileName = await openFile.OpenFile(string.Empty, "Image Files|*.png;*.jpg;*.jpeg;*.bmp;*.gif");
            if (!(string.IsNullOrEmpty(fileName)))
            {
                var bitmap = new BitmapImage(new Uri(fileName));
                var image = new System.Windows.Controls.Image
                {
                    Source = bitmap,
                    Width = 200, // adjust size as needed
                    Stretch = System.Windows.Media.Stretch.Uniform
                };

                // Insert inline at caret
                new InlineUIContainer(image, MyRichTextBox.CaretPosition);
                //  MyRichTextBox.Document.Blocks.Add(new BlockUIContainer(image));
            }
        }

        private async void BtnPrint(object sender, RoutedEventArgs e)
        {

        }
        private async void BtnEmail(object sender, RoutedEventArgs e)
        {

        }
        private async void BtnExit(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
        private async void BtnAttcah(object sender, RoutedEventArgs e)
        {

        }

        private void MyRichTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {

            var selection = MyRichTextBox.Selection;

            // Bold
            object weight = selection.GetPropertyValue(TextElement.FontWeightProperty);
            BoldButton.IsChecked = (weight != DependencyProperty.UnsetValue && weight.Equals(FontWeights.Bold));

            // Italic
            object style = selection.GetPropertyValue(TextElement.FontStyleProperty);
            ItalicButton.IsChecked = (style != DependencyProperty.UnsetValue && style.Equals(FontStyles.Italic));

            // Underline
            object deco = selection.GetPropertyValue(Inline.TextDecorationsProperty);
            UnderlineButton.IsChecked = (deco != DependencyProperty.UnsetValue && deco.Equals(TextDecorations.Underline));
            FontSizeCombo_SelectionChanged(sender, null);
            FontFamilyCombo_SelectionChanged(sender, null);
            ColorChange(sender, null);


            //object family = selection.GetPropertyValue(TextElement.FontFamilyProperty);
            //if (family != DependencyProperty.UnsetValue)
            //            FontFamilyCombo.SelectedItem = FontFamilyCombo.Items
            //                .Cast<ComboBoxItem>()
            //                .FirstOrDefault(i => i.Content.ToString() == ((FontFamily)family).Source);
            //        object size = selection.GetPropertyValue(TextElement.FontSizeProperty);
            //        if (size != DependencyProperty.UnsetValue)
            //            FontSizeCombo.SelectedItem = FontSizeCombo.Items
            //                .Cast<ComboBoxItem>()
            //                .FirstOrDefault(i => i.Content.ToString() == ((double)size).ToString());



            // Alignment
            object align = selection.GetPropertyValue(Paragraph.TextAlignmentProperty);
            AlignLeftButton.IsChecked = (align != DependencyProperty.UnsetValue && align.Equals(TextAlignment.Left));
            AlignCenterButton.IsChecked = (align != DependencyProperty.UnsetValue && align.Equals(TextAlignment.Center));
            AlignRightButton.IsChecked = (align != DependencyProperty.UnsetValue && align.Equals(TextAlignment.Right));



        }
        private async void BtnCreate(object sender, RoutedEventArgs e)
        {
            if (MyRichTextBox != null)
            {
                MyRichTextBox.BeginInit();
                MyRichTextBox.Document.Blocks.Clear();
                MyRichTextBox.EndInit();
                viewModel.MyDocXaml = string.Empty;
                viewModel.MyDocRtf.BeginInit();
                viewModel.MyDocRtf.Document.Blocks.Clear();
                viewModel.MyDocRtf.EndInit();
                MyRichTextBox.AppendText($"Dear Alano Club Member,{Utilites.AlanoCLubConstProp.CRLF}");
                MyRichTextBox.Focus();


            }
            //Utilites.OpenFileDialog openFile = new Utilites.OpenFileDialog();
            // await openFile.SaveFileRichText(MyRichTextBox, "Rich Text Format (*.rtf)|*.rtf|XAML (*.xaml)|*.xaml|Text Files (*.txt)|*.txt");
        }
        private async void ColorChange(object sender, SelectionChangedEventArgs e)

        {
            if (MyRichTextBox != null)
            {
                if (ColorComboBox.SelectedItem is ComboBoxItem item)
                {
                    if (ColorComboBox.SelectedIndex != 0)
                    {
                        var colorName = item.Content.ToString();
                        var converter = new BrushConverter();
                        var brush = (Brush)converter.ConvertFromString(colorName!);
                        MyRichTextBox.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, brush);
                        ColorComboBox.Background = brush;
                    }
                    else
                    {
                        MyRichTextBox.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Black);
                        ColorComboBox.Background = Brushes.White;
                        ColorComboBox.Foreground = Brushes.Black;

                    }


                }
            }


            MyRichTextBox.Focus();
        }

        private void MyRichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var range = new TextRange(MyRichTextBox.Document.ContentStart,
                                      MyRichTextBox.Document.ContentEnd);

            // --- Update RTF ---
            //using (var rtfStream = new MemoryStream())
            // {
            // range.Save(rtfStream,System.Windows.DataFormats.Rtf);
            //   rtfStream.Position = 0;
            // using var reader = new StreamReader(rtfStream);
            // viewModel.MyDocXaml = reader.ReadToEnd();
            // }
            using var stream = new MemoryStream();

            XamlWriter.Save(MyRichTextBox.Document, stream);
            stream.Position = 0;
            using var reader = new StreamReader(stream, Encoding.UTF8);
            viewModel.MyDocXaml = reader.ReadToEnd();
            viewModel.MyDocRtf   = MyRichTextBox;
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    range.Save(ms, System.Windows.DataFormats.Rtf);
            //    ms.Position = 0;
            //    using (StreamReader readerRtf = new StreamReader(ms))
            //    {
            //       rtfContent = reader.ReadToEnd();
            //    }

            //}
        }
        private async void InsertTablePostion(System.Windows.Controls.RichTextBox rtb, int rows, int columns, bool hasHeader)
        {
            var table = new Table
            {
                CellSpacing = 2,
                // No spacing between cells
                BorderThickness = new Thickness(1), // Outer border thickness
                BorderBrush = System.Windows.Media.Brushes.Black,
                Margin = new Thickness(10)// Outer border color,;
            };
            // Add columns
            for (int c = 0; c < columns; c++)
                table.Columns.Add(new TableColumn { Width = new GridLength(100) });

            // Add header row if needed
            if (hasHeader)
            {
                var headerRow = new TableRow();
                for (int c = 0; c < columns; c++)
                    headerRow.Cells.Add(new TableCell(new Paragraph(new Run($"Header {c + 1}"))));
                var headerGroup = new TableRowGroup();

                headerGroup.Rows.Add(headerRow);
                table.RowGroups.Add(headerGroup);
            }

            // Add body rows
            var bodyGroup = new TableRowGroup();
            for (int r = 0; r < rows; r++)
            {
                var row = new TableRow();
                for (int c = 0; c < columns; c++)
                    row.Cells.Add(new TableCell(new Paragraph(new Run($"Row {r + 1}, Col {c + 1}")))
                    {
                        BorderBrush = System.Windows.Media.Brushes.Black,
                        BorderThickness = new Thickness(1),
                        Padding = new Thickness(4)
                    });


                bodyGroup.Rows.Add(row);
            }
            table.RowGroups.Add(bodyGroup);
            var position = rtb.CaretPosition;
            position.InsertParagraphBreak();
            position.Paragraph.SiblingBlocks.Add(table);
            rtb.Document.Blocks.Add(table);


            // Insert into RichTextBox

        }
        private void InsertTableWithBorders(System.Windows.Controls.RichTextBox rtb)
        {
            // Create a new table
            Table table = new Table();

            // Add columns
            for (int c = 0; c < 3; c++)
                table.Columns.Add(new TableColumn());

            // Create a row group
            TableRowGroup trg = new TableRowGroup();
            table.RowGroups.Add(trg);

            // Add rows with cells
            for (int r = 0; r < 3; r++)
            {
                TableRow row = new TableRow();
                for (int c = 0; c < 3; c++)
                {
                    // Create a cell with text
                    TableCell cell = new TableCell(new Paragraph(new Run($"R{r + 1}C{c + 1}")));

                    // Add borders
                    cell.BorderBrush = Brushes.Black;
                    cell.BorderThickness = new Thickness(1);

                    row.Cells.Add(cell);
                }
                trg.Rows.Add(row);
            }

            // Insert table at caret position
            var position = rtb.CaretPosition;
            position.InsertParagraphBreak();
            position.Paragraph.SiblingBlocks.Add(table);
        }
        private async void LineSpacingCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LineSpacingCombo.SelectedItem is ComboBoxItem item)
            {
                if (MyRichTextBox ==null)
                {
                    return;
                }
            
                double lineHeight = double.Parse(item.Tag.ToString());

                // Apply to current selection
                var range = new TextRange(MyRichTextBox.Selection.Start, MyRichTextBox.Selection.End);
                if (!range.IsEmpty)
                {
                    range.ApplyPropertyValue(Paragraph.LineHeightProperty, lineHeight);
                    range.ApplyPropertyValue(Paragraph.LineStackingStrategyProperty, LineStackingStrategy.BlockLineHeight);
                }
                else
                {
                    // Apply globally to FlowDocument defaults
                    MyRichTextBox.Document.LineHeight = lineHeight;
                    MyRichTextBox.Document.LineStackingStrategy = LineStackingStrategy.BlockLineHeight;
                }
            }
        }
        private async void MyRichTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {

            if (e.Key == Key.Enter)
            {
                // Check if caret is inside a TableCell
                var cell = MyRichTextBox.CaretPosition.Parent as TableCell;
                if (cell != null)
                {
                    // Optional: check if it's the last row
                    var row = cell.Parent as TableRow;
                    var group = row?.Parent as TableRowGroup;
                    if (group != null && group.Rows[group.Rows.Count - 1] == row)
                    {
                        // Suppress default behavior
                        e.Handled = true;

                        // Instead, insert a paragraph break after the table
                        var position = row.ElementEnd;
                        MyRichTextBox.CaretPosition = position;
                        MyRichTextBox.CaretPosition.InsertParagraphBreak();
                    }
                }
            }
        }
    }
}
// vm:ListBoxHelper.BindableSelectedItems="{Binding MembersSelected,Mode=TwoWay,UpdateSourceTrigger=PropertyChanged}"
//private void MyRichTextBox_KeyDown(object sender, KeyEventArgs e)
//{
//    if (e.Key == Key.Enter)
//    {
//        var cell = MyRichTextBox.CaretPosition.Parent as TableCell;
//        if (cell != null)
//        {
//            var row = cell.Parent as TableRow;
//            var group = row?.Parent as TableRowGroup;

//            if (group != null && group.Rows[group.Rows.Count - 1] == row)
//            {
//                if (!AllowTableRowInsert)
//                {
//                    // Suppress default behavior
//                    e.Handled = true;

//                    // Move caret outside the table
//                    var position = row.ElementEnd;
//                    MyRichTextBox.CaretPosition = position;
//                    MyRichTextBox.CaretPosition.InsertParagraphBreak();
//                }
//                // else: let WPF insert a new row normally
//            }
//        }
//    }