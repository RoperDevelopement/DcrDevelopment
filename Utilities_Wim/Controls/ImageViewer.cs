using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EdocsUSA.Utilities.Logging;
namespace EdocsUSA.Controls
{	
	/// <summary>Control for viewing an image, supports Panning, Zooming & Selecting.</summary>
	/// <remarks>Credit to: <see cref="http://bobpowell.net/zoompicbox.aspx"></remarks>
	public class ImageViewer : System.Windows.Forms.ScrollableControl
	{	
		
		#region enums
	
		public enum MouseToolMode
		{
			Pan,
			Select
		}
		
		public enum ImageScalingMode
		{
			Custom,
			FitHeight,
			FitWidth,
			Fit,
			None
		}
		
		#endregion Enums
		
		#region Properties
		
		private Brush _SelectionBrush = new SolidBrush(Color.FromArgb(128, SystemColors.ControlLight));
		/// <summary>Brush to use for filling the selected area.</summary>
		/// <remarks>Brush color should contrast with the color of SelectionPen.</remarks>
		public Brush SelectionBrush
		{
			get { return _SelectionBrush; }
			set { _SelectionBrush = value; }
		}
		
		private Pen _SelectionPen = new Pen(SystemColors.ControlDarkDark);
		/// <summary>Pen used for drawing the border around the selected area.</summary>
		/// <remarks>Should be a contrasting color to SelectionBrush.</remarks>
		public Pen SelectionPen
		{
			get { return _SelectionPen; }
			set { _SelectionPen = value; }
		}
		
		private Image _Image = null;
		public Image Image
		{
			get { return _Image; }
			set
			{
				_Image = value;
				OnImageChanged();
			}
		}
		
		
		private MouseToolMode _ToolMode = MouseToolMode.Pan;
		public MouseToolMode ToolMode
		{
			get { return _ToolMode; }
			set
			{
				_ToolMode = value;
				OnToolModeChanged();
			}
		}
		
		private ImageScalingMode _ScalingMode = ImageScalingMode.Fit;
		public ImageScalingMode ScalingMode
		{
			get { return _ScalingMode; }
			set
			{
				_ScalingMode = value;
				OnScalingModeChanged();
			}
		}

		private float _ZoomLevel = 1.0F;
		public float ZoomLevel
		{
			get { return _ZoomLevel; }
			set { _ZoomLevel = value; }
		}
		
		//TODO: Reset to configurable default
		//private double _ZoomMultiplier = Properties.SQImageViewer.Default.DefaultZoomMultiplier;
		private float _ZoomMultiplier = 0.15F;
		public float ZoomMultiplier
		{
			get { return _ZoomMultiplier; }
			set { _ZoomMultiplier = value; }
		}
		
		protected Point PanAnchor;
		protected bool Panning;
		
		private Point _SelectAnchor = Point.Empty;
		public Point SelectAnchor
		{
			get { return _SelectAnchor; }
			set
			{
				_SelectAnchor = value;
				OnSelectionChanged();
			}
		}
		
		private Point _SelectEnd = Point.Empty;
		public Point SelectEnd
		{
			get { return _SelectEnd; }
			set
			{
				_SelectEnd = value;
				OnSelectionChanged();
			}
		}

		/// <summary>Region of the image that has been selected (defined by SelectAnchor & SelectEnd).</summary>
		public Rectangle SelectedRectangle
		{
			get
			{
				return new Rectangle()
				{
					X = Math.Min(SelectAnchor.X, SelectEnd.X),
					Y = Math.Min(SelectAnchor.Y, SelectEnd.Y),
					Width = Math.Abs(SelectAnchor.X - SelectEnd.X),
					Height = Math.Abs(SelectAnchor.Y - SelectEnd.Y)
				};
			}
		}

		protected bool Selecting;

		#endregion Properties

		#region Events

		/// <summary>Fired when the Image object is changed (not fired on changes to the existing image)</summary>
		public event EventHandler ImageChanged;

		/// <summary>Fired when ToolMode is changed.</summary>
		public event EventHandler ToolModeChanged;

		/// <summary>Fired when ScalingMode is changed.</summary>
		public event EventHandler ScalingModeChanged;
		
		/// <summary>Fired when the selected region of the image is changed.</summary>
		public event EventHandler SelectionChanged;

		#endregion Events

		#region Constructors

		public ImageViewer()
		{
			//Double buffer the control
			this.SetStyle(ControlStyles.AllPaintingInWmPaint |
			              ControlStyles.UserPaint |
			              ControlStyles.ResizeRedraw |
			              ControlStyles.UserPaint |
			              ControlStyles.DoubleBuffer, true);
			this.AutoScroll = true;
		}

		#endregion Constructors

		#region Custom Event Handlers

		protected virtual void OnImageChanged()
		{
			ClearSelection();
			this.Invalidate();

			EventHandler handler = ImageChanged;
			if (handler != null)
			{ handler(this, null); }
		}

		public virtual void OnToolModeChanged()
		{
			switch (ToolMode)
			{
				case MouseToolMode.Pan:
					this.Cursor = Cursors.Hand;
					break;
				case MouseToolMode.Select:
					this.Cursor = Cursors.Cross;
					break;
				default:
					 TraceLogger.TraceLoggerInstance.TraceWarning("Unexpected MouseToolMode " + ToolMode);
					break;
			}

			EventHandler handler = ToolModeChanged;
			if (handler != null)
			{ handler(this, null); }
		}

		protected virtual void OnScalingModeChanged()
		{
			if (ImageExists() == false)
			{ return; }
			
			Invalidate();

			EventHandler handler = ScalingModeChanged;
			if (handler != null)
			{ handler(this, null); }
		}
		
		protected virtual void OnSelectionChanged()
		{
			Invalidate();
			
			EventHandler handler = SelectionChanged;
			if (handler != null)
			{ handler(this, null); }
		}

		#endregion Custom Event Handlers

		#region Control Event Handlers

		protected override void OnClick(EventArgs e)
		{
			//Focus the control to allow command keys to process
			this.Focus();
			base.OnClick(e);
		}
		
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			switch (keyData)
			{
				case Keys.Control | Keys.NumPad0:
					ScalingMode = ImageScalingMode.Fit;
					return true;
				case Keys.Control | Keys.NumPad1:
					ScalingMode = ImageScalingMode.None;
					return true;
				case Keys.Control | Keys.NumPad2:
					ScalingMode = ImageScalingMode.FitWidth;
					return true;
				case Keys.Control | Keys.NumPad3:
					ScalingMode = ImageScalingMode.FitHeight;
					return true;
				case Keys.Control | Keys.Add:
					ZoomIn();
					return true;
				case Keys.Control | Keys.Subtract:
					ZoomOut();
					return true;
				default: return base.ProcessCmdKey(ref msg, keyData);
			}
		}
		
		protected override void OnMouseDoubleClick(MouseEventArgs e)
		{
			base.OnMouseDoubleClick(e);
			CenterAtPointer(e);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			
			switch (ToolMode)
			{
				case MouseToolMode.Pan:
					BeginPan(e);
					break;
				case MouseToolMode.Select:
					ClearSelection();
					BeginSelect(e);
					break;
				default:
                    TraceLogger.TraceLoggerInstance.TraceWarning("Unexpected MouseToolMode " + ToolMode);
					break;
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			if (Panning)
			{ PanToPointer(e); }
			if (Selecting)
			{ SelectToPointer(e); }

		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if (Panning)
			{ EndPan(e); }
			if (Selecting)
			{ EndSelect(e); }
		}

		protected override void OnPaint(PaintEventArgs e)
		{			
			base.OnPaint(e);
            //TODO: Handle disposed image.
            TraceLogger.TraceLoggerInstance.TraceInformation("Painting image");
			if (Image == null)
			{
                TraceLogger.TraceLoggerInstance.TraceWarning("Image is null");
                return;
            }
            TraceLogger.TraceLoggerInstance.TraceWarning("Scaling");
			//Set ths zoom level based on the current ScalingMode and the control's size.
			switch (ScalingMode)
			{
				case ImageScalingMode.Custom:
					break;
				case ImageScalingMode.Fit:
					if (Image.Width > Image.Height)
					{ goto case ImageScalingMode.FitWidth; }
					else
					{ goto case ImageScalingMode.FitHeight; }
				case ImageScalingMode.FitHeight:
					{
						float zoomLevel = this.Height / (float)(Image.Height);
						//If the calculated zoom level will require horizontal scrolling, recalculate
						// (account for horizontal scrollbar height)
						if ((Image.Width * zoomLevel) > this.Width)
						{ zoomLevel = (this.Height - SystemInformation.HorizontalScrollBarHeight) / (float)(Image.Height); }
						ZoomLevel = zoomLevel;
					}
					break;
				case ImageScalingMode.FitWidth:
					{
						float zoomLevel = this.Width / (float)(Image.Width);
						//If the calculated zoom level will require vertical scrolling, recalculate
						// (account for vertical scrollbar width)
						if ((Image.Height * zoomLevel) > this.Height)
						{ zoomLevel = (this.Width - SystemInformation.VerticalScrollBarWidth) / (float)(Image.Width); }
						ZoomLevel = zoomLevel;
					}
					break;
				case ImageScalingMode.None:
					ZoomLevel = 1.0F;
					break;
				default:
                    TraceLogger.TraceLoggerInstance.TraceWarning("Unexpected ScalingModes " + ScalingMode);
					goto case ImageScalingMode.Fit;
			}			

			//TODO: Research performance / quality for InterpolationMode, CompostingQuality, & SmoothingMode
			e.Graphics.InterpolationMode = InterpolationMode.Low;
			e.Graphics.CompositingQuality = CompositingQuality.HighSpeed;
			e.Graphics.SmoothingMode = SmoothingMode.HighSpeed;
			e.Graphics.TextRenderingHint = TextRenderingHint.SystemDefault;
			e.Graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;

			//Calculate the new scroll bounds
			this.AutoScrollMinSize = new Size()
			{
				Width = (int)(this.Image.Width * ZoomLevel),
				Height = (int)(this.Image.Height * ZoomLevel)
			};

			//Calculate the correct location for the image
			//See BobPowell's article for more explanation.
			//Set up a zoom matrix
			Matrix zoomMatrix = new Matrix(ZoomLevel, 0, 0, ZoomLevel, 0, 0);
			//Translate the matrix into positions for the scrollbars
			zoomMatrix.Translate(this.AutoScrollPosition.X / ZoomLevel, this.AutoScrollPosition.Y / ZoomLevel);
			//Apply the transform to the graphics object
			e.Graphics.Transform = zoomMatrix;
			//Draw the image (ignoring image resolution settings)
			e.Graphics.DrawImage(Image, new Rectangle(0, 0, Image.Width, Image.Height));

			if ((SelectedRectangle.Width > 0) && (SelectedRectangle.Height > 0))
			{
				e.Graphics.FillRectangle(SelectionBrush, SelectedRectangle);
				e.Graphics.DrawRectangle(SelectionPen, SelectedRectangle);
			}
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			Invalidate();
		}

		#endregion Control Event Handlers

		#region Validation

		public bool ImageExists()
		{ return Image != null; }

		/// <summary>Throws an exception if the viewer does not have an image loaded.</summary>
		/// <exception cref="InvalidOperationException">If the viewer does not have an image loaded.</exception>
		public void EnsureImageExists()
		{
			if (ImageExists() == false)
			{
                TraceLogger.TraceLoggerInstance.TraceError("The requested operation requires an image to be loaded");
                throw new InvalidOperationException("The requested operation requires an image to be loaded");
            }
		}

		public bool SelectionExists()
		{ return ((SelectedRectangle.Width) != 0 && (SelectedRectangle.Height != 0)); }
		
		public void EnsureSelectionExists()
		{
			if (SelectionExists() == false)
			{
                TraceLogger.TraceLoggerInstance.TraceError("The requested operation requires a region of the image to be selected");
                throw new InvalidOperationException("The requested operation requires a region of the image to be selected");
            }
		}
		
		#endregion Validation

		#region Panning & Selecting

		private void CenterAtPointer(MouseEventArgs e)
		{
			if (ImageExists() == false)
			{ return; }

			//Get the center of the panel.
			Point panelCenter = new Point
			{
				X = (int)(Decimal.Divide(this.Width, 2)),
				Y = (int)(Decimal.Divide(this.Height, 2))
			};

			//Calculate the distance from the center of the panel to the pointer.
			int deltaX = panelCenter.X - e.Location.X;
			int deltaY = panelCenter.Y - e.Location.Y;

			//Calculate the new scroll position.
			//AutoScrollPosition returns negative values, but requires positive values to set.
			AutoScrollPosition = new Point()
			{
				X = -AutoScrollPosition.X - deltaX,
				Y = -AutoScrollPosition.Y - deltaY
			};
		}

		protected void BeginPan(MouseEventArgs e)
		{
			PanAnchor = e.Location;
			Panning = true;
		}

		protected void EndPan(MouseEventArgs e)
		{
			PanAnchor = Point.Empty;
			Panning = false;
		}

		protected Point ControlPointToImagePoint(Point p)
		{
			return new Point()
			{
				X = (int)((-this.AutoScrollPosition.X + p.X) / ZoomLevel),
				Y = (int)((-this.AutoScrollPosition.Y + p.Y) / ZoomLevel)
			};
		}

		protected void BeginSelect(MouseEventArgs e)
		{
			SelectAnchor = ControlPointToImagePoint(e.Location);
			SelectEnd = SelectAnchor;
			Selecting = true;
		}

		private void SelectToPointer(MouseEventArgs e)
		{
			SelectEnd = ControlPointToImagePoint(e.Location);
		}

		protected void EndSelect(MouseEventArgs e)
		{
			Selecting = false;
		}

		private void PanToPointer(MouseEventArgs e)
		{
			if (ImageExists() == false)
			{ return; }

			//Calculate the distance from the initial mouse down position to the current pointer
			int deltaX = PanAnchor.X - e.Location.X;
			int deltaY = PanAnchor.Y - e.Location.Y;

			//Calculte the new scroll position.
			//AutoScrollPosition returns negative values, but requires positive values to set.
			AutoScrollPosition = new Point()
			{
				X = -AutoScrollPosition.X + deltaX,
				Y = -AutoScrollPosition.Y + deltaY
			};

			PanAnchor = e.Location;
		}

		public void ClearSelection()
		{
			this.SuspendLayout();
			try
			{
				SelectAnchor = Point.Empty;
				SelectEnd = Point.Empty;
			}
			finally
			{ this.ResumeLayout(); }
		}

		#endregion Panning & Selecting

		#region Scaling & Zooming

		public void ScaleToOriginalSize()
		{ ScalingMode = ImageScalingMode.None; }
		
		public void ScaleToFit()
		{ ScalingMode = ImageScalingMode.Fit; }
		
		public void ScaleToFitHeight()
		{ ScalingMode = ImageScalingMode.FitHeight; }
		
		public void ScaleToFitWidth()
		{ ScalingMode = ImageScalingMode.FitWidth; }
		
		public void ZoomIn()
		{
			this.SuspendLayout();
			try
			{
				ScalingMode = ImageScalingMode.Custom;
				ZoomLevel *= (1 + ZoomMultiplier);
			}
			finally
			{ this.ResumeLayout(); }
		}

		public void ZoomOut()
		{
			this.SuspendLayout();
			try
			{
				ScalingMode = ImageScalingMode.Custom;
				ZoomLevel *= (1 - ZoomMultiplier);
			}
			finally
			{ this.ResumeLayout(); }
		}

		#endregion Scaling & Zooming
	}
}
