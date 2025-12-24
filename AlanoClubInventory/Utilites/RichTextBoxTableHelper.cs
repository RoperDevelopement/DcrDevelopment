using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
 

namespace AlanoClubInventory.Utilites
{
    public static class RichTextBoxTableHelper
    {
        public static void InsertTable(RichTextBox rtb, int rows, int columns, bool hasHeader)
        {
            var table = new Table();

            // Add columns
            for (int c = 0; c < columns; c++)
                table.Columns.Add(new TableColumn());

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
                    row.Cells.Add(new TableCell(new Paragraph(new Run($"Row {r + 1}, Col {c + 1}"))));
                bodyGroup.Rows.Add(row);
            }
            table.RowGroups.Add(bodyGroup);

            // Insert into RichTextBox
            rtb.Document.Blocks.Add(table);
        }
        public static void InsertTablePostion(  System.Windows.Controls.RichTextBox rtb, int rows, int columns, bool hasHeader)
        {
            var table = new Table
            {
                CellSpacing = 2,
                // No spacing between cells
                BorderThickness = new Thickness(1), // Outer border thickness
                BorderBrush= System.Windows.Media.Brushes.Gray,
                Margin = new Thickness(10)// Outer border color,;
            };
            // Add columns
            for (int c = 0; c < columns; c++)
                table.Columns.Add(new TableColumn{ Width = new GridLength(100) });

            // Add header row if needed
            if (hasHeader)
            {
                var headerRow = new TableRow();
                for (int c = 0; c < columns; c++)
                    headerRow.Cells.Add(new TableCell(new Paragraph(new Run($"Header {c + 1}")))
                    {
                        BorderBrush = System.Windows.Media.Brushes.Black,
                        BorderThickness = new Thickness(1),
                        Padding = new Thickness(4),
                        FontWeight = FontWeights.Bold,
                        Background = System.Windows.Media.Brushes.LightGray,
                      TextAlignment = TextAlignment.Center,
                    });
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
          
            //position.Paragraph.Inlines.Add(new Run($"{AlanoCLubConstProp.CarrageReturnLineFeed}"));
            // rtb.Document.Blocks.Add(table);


            // Insert into RichTextBox

        }
        public static void DeleteSelectedTableRows(System.Windows.Controls.RichTextBox rtb)
        {
            // Get the current selection
            TextPointer start = rtb.Selection.Start;
            TextPointer end = rtb.Selection.End;

            // Walk up the logical tree to see if we're inside a TableCell
            var cell = start.Parent as TableCell ?? end.Parent as TableCell;
            if (cell == null) return;

            // From the cell, get the row
            var row = cell.Parent as TableRow;
            if (row == null) return;

            // From the row, get the row group
            var rowGroup = row.Parent as TableRowGroup;
            if (rowGroup == null) return;

            // Remove the row
            rowGroup.Rows.Remove(row);
        }
        public static void DeleteAllTableRows(RichTextBox rtb)
        {
            foreach (var block in rtb.Document.Blocks)
            {
                if (block is Table table)
                {
                    foreach (var group in table.RowGroups)
                    {
                        group.Rows.Clear();
                    }
                }
            }
        }
    }

}
