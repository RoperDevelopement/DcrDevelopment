using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace EdocsUSA.Utilities.Controls
{
	public class ProgressBarEx : UserControl
	{
		protected SolidBrush BarBrush = new SolidBrush(SystemColors.ControlDark);
		
		public Color BarColor
		{
			get { return BarBrush.Color; }
			set  
			{ 
				bool invalidate = (BarColor.Equals(value) == false);
				BarBrush = new SolidBrush(value);
				if (invalidate) Invalidate();
			}
		}
		
		protected SolidBrush CaptionBrush = new SolidBrush(SystemColors.ControlText);
		
		public Color CaptionColor
		{
			get { return CaptionBrush.Color; }
			set 
			{
				bool invalidate = (CaptionColor.Equals(value) == false);
				CaptionBrush = new SolidBrush(value);
				if (invalidate) Invalidate();
			}
		}
		
		private int _Value = 0;
		public int Value
		{
			get { return _Value; }
			set
			{
					bool invalidate = (value != _Value);
			   if (value < 0 || value > 100)
			       throw new ArgumentOutOfRangeException("value");
			   _Value = value;
			   if (invalidate) Invalidate();
			}
		}
	    
		private string _Caption = string.Empty;
		public string Caption
		{
			get { return _Caption; }
			set
			{
				bool invalidate = (value.Equals(_Caption) == false);
				_Caption = value;
				if (invalidate) Invalidate();
			}
		}
		
		protected ToolTip ToolTip = new ToolTip();
		
		public string ToolTipText
		{
			get { return ToolTip.GetToolTip(this); }
			set 
			{ 
				ToolTip.SetToolTip(this, value);
			}
		}
		
		private void InitializeComponent()
		{
			this.MinimumSize = new Size(100, 25);
			this.BorderStyle = BorderStyle.Fixed3D;
			this.ToolTip = new ToolTip()
			{
				ShowAlways = true,
				InitialDelay = 50,
				ReshowDelay = 50
			};
		}
		
		
		public ProgressBarEx()
		{
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.ResizeRedraw, true);
			SetStyle(ControlStyles.Selectable, true);
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			InitializeComponent();
		}
		
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			const int progressMargin = 2;
			const int captionMargin = 5;
			//if (Value <= 0) return;
			Rectangle progressRectangle = new Rectangle(ClientRectangle.X + progressMargin,
			                                   ClientRectangle.Y + progressMargin,
			                                   ClientRectangle.Width * Value / 100 - progressMargin * 2,
			                                   ClientRectangle.Height - progressMargin * 2);
			PointF captionStart = new PointF(captionMargin, captionMargin);
			PointF captionCurrent = captionStart;
			SizeF captionSize = e.Graphics.MeasureString(Caption, SystemFonts.CaptionFont);
			
			RectangleF captionRectangle = new RectangleF(
				captionMargin
				, (ClientRectangle.Height / 2) - (captionSize.Height / 2)
				, captionSize.Width
				, captionSize.Height);
			
	
			e.Graphics.FillRectangle(BarBrush, progressRectangle);
		 	
			e.Graphics.DrawString(Caption, SystemFonts.CaptionFont, CaptionBrush, captionRectangle);
			
		}
	    
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			Invalidate();
		}
	}
}
