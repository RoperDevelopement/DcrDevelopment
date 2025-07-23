using BinMonitor.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BinMonitor
{
    public class GridPanel : Panel
    {
        private int _MaxColumns = 10;
        public int MaxColumns
        {
            get { return _MaxColumns; }
            set { _MaxColumns = value; }
        }

        public GridPanel()
        {
            this.DoubleBuffered = true;
            this.ResizeRedraw = true;
        }
        
        protected override void OnPaint(PaintEventArgs e)
        {
            
            //this.Controls.Clear();
            //e.Graphics.Clear(this.BackColor);
            
            if (this.Controls.Count <= 0)
            { return; }

            int columnCount = Math.Min(MaxColumns, this.Controls.Count);
            int rowCount = (int)(Math.Ceiling(Decimal.Divide(this.Controls.Count, columnCount)));
            int columnWidth = (int)(Math.Floor(Decimal.Divide(this.ClientSize.Width - this.Margin.Horizontal, columnCount)));
            int rowHeight = (int)(Math.Floor(Decimal.Divide(this.ClientSize.Height - this.Margin.Vertical, rowCount)));
            Size controlSize = new Size(columnWidth, rowHeight);

            Point p = new Point(this.Margin.Left, this.Margin.Top);
            for (int currentRow = 0; currentRow < rowCount; currentRow++)
            {
                if (currentRow > 0)
                {
                    p.X = this.Margin.Left;
                    p.Y = p.Y + rowHeight;
                }
                for (int currentColumn = 0; currentColumn < columnCount; currentColumn++)
                {
                    int controlIndex = (currentRow * columnCount) + currentColumn;
                    if (controlIndex >= this.Controls.Count)
                    { break; }
                    
                    if (currentColumn == 0)
                    { p.X = this.Margin.Left; }
                    else
                    { p.X = p.X + columnWidth; }

                    Controls[controlIndex].Size = controlSize;
                    Controls[controlIndex].Location = p;
                    //Debug.WriteLine("Drawing " + Controls[controlIndex]);
                    
                }
            }
            base.OnPaint(e);
        }
    }
}
