/*
 * Created by SharpDevelop.
 * User: Sam Brinly
 * Date: 3/23/2011
 * Time: 11:01 AM
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EdocsUSA.Controls
{
	public class DataGridViewEx : DataGridView
	{
		private bool _ScrollSelectedRowsIntoView = true;
		public bool ScrollSelectedRowsIntoView
		{
			get { return _ScrollSelectedRowsIntoView; }
			set { _ScrollSelectedRowsIntoView = value; }
		}
		
		public int VisibleRowCount
		{ get  { return LastDisplayedScrollingRowIndex - FirstDisplayedScrollingRowIndex; } }
		
		protected override void OnRowPostPaint(DataGridViewRowPostPaintEventArgs e)
        {
            string rowNumber = (e.RowIndex + 1).ToString();
            
            SizeF textSize = e.Graphics.MeasureString(rowNumber, this.Font);
            if (this.RowHeadersWidth < (int)textSize.Width + 20) this.RowHeadersWidth = (int)(textSize.Width + 20);
            Brush brush = SystemBrushes.ControlText;
            e.Graphics.DrawString(rowNumber, this.Font, brush
                            , e.RowBounds.Location.X + 15
                            , e.RowBounds.Location.Y + ((e.RowBounds.Height - textSize.Height) / 2));

            base.OnRowPostPaint(e);
        }
		
		protected override void OnSelectionChanged(EventArgs e)
		{			
			if (ScrollSelectedRowsIntoView && (SelectedRowCount > 0))
			{			
				if (LastSelectedRow.Index < FirstDisplayedScrollingRowIndex)
				{ FirstDisplayedScrollingRowIndex = LastSelectedRow.Index; }
				else if (LastSelectedRow.Index > LastDisplayedScrollingRowIndex)
				{ FirstDisplayedScrollingRowIndex = LastSelectedRow.Index - DisplayedRowCount(false) + 1; }
			}
			
			base.OnSelectionChanged(e);
		}
		
		public int LastDisplayedScrollingRowIndex { get { return FirstDisplayedScrollingRowIndex + DisplayedRowCount(false) -1; } }
		
		public bool HasSelection { get { return SelectedRows.Count > 0; } }
		
		public int SelectedRowCount { get { return SelectedRows.Count; } }
		
		public DataGridViewRow LastSelectedRow  { get { return HasSelection ? SelectedRows[0] : null; } }
		
		public IEnumerable<DataGridViewRow> SelectedRowsOrderedByIndex
		{ get { return ( from DataGridViewRow row in SelectedRows orderby row.Index select row); } }
		
		public bool AreSelectedRowsSequential
		{
			get
			{
				DataGridViewRow[] selectedRows = (from DataGridViewRow row in SelectedRows orderby row.Index select row).ToArray();
				
				for (int i = 1; i < selectedRows.Length; i++)
				{ if (selectedRows[i].Index - selectedRows[i-1].Index != 1) return false; }
				
				return true;
			}
		}
		
		public void SelectSingleRow(int index)
		{
			if (Rows.Count == 0) return;
			if (index < 0) index = 0;
			if (index > Rows.Count - 1) index = Rows.Count - 1;
			
			ClearSelection();
			Rows[index].Selected = true;
		}
	}
}
