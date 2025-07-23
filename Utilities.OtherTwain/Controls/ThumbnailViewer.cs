using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace EdocsUSA.Utilities.Controls
{
	public partial class ThumbnailViewer : UserControl
	{
		#region Item Properties
		
		private string _DisplayMemberName = "Thumbnail";
		public string DisplayMemberName
		{
			get { return _DisplayMemberName; }
			set { _DisplayMemberName = value; }
		}
		
		protected readonly List<object> _Items = new List<object>();
		
		public IEnumerable<object> Items
		{ get { return _Items.AsEnumerable(); } }
		
		protected readonly HashSet<object> _CheckedItems = new HashSet<object>();
		
		public IEnumerable<object> CheckedItems
		{ get { return _CheckedItems.AsEnumerable(); } }
		
		public IEnumerable<object> CheckedItems_Sorted
		{ get { return CheckedItems.OrderBy(item=> _Items.IndexOf(item)); } }
		
		protected readonly HashSet<object> _SelectedItems = new HashSet<object>();
		
		public IEnumerable<object> SelectedItems
		{ get { return _SelectedItems.AsEnumerable(); } }
		
		public IEnumerable<object> SelectedItems_Sorted
		{ get { return _SelectedItems.OrderBy(item => _Items.IndexOf(item)); } }
		
		#endregion Item Properties
		
		#region Constructors
		
		public ThumbnailViewer()
		{
			InitializeComponent();
		}
		
		#endregion Constructors
		
		#region Item Management
		
		public bool ItemIsValid(object item)
		{
			Type itemType = item.GetType();
			//Get information about the property with the name defined in DisplayMemberBinding
			PropertyInfo thumbnailPropertyInfo = itemType.GetProperty(DisplayMemberName);
			//If the item doesn't have a property with the name defined in DisplayMemberBinding, fail
			if (thumbnailPropertyInfo == null)
			{ 
				Trace.TraceWarning("Item does not contain member " + DisplayMemberName);
				return false;				
			}
			//If the property defined in DisplayMemberBinding is not an Image, fail
			if ((thumbnailPropertyInfo.PropertyType == typeof(Image)) == false)
			{
				Trace.TraceWarning("Item's " + DisplayMemberName + " property is not an image");
				return false;
			}
			//If it hasn't failed yet, it must be valid.
			return true;
		}
		
		public void EnsureItemIsValid(object item)
		{
			if (ItemIsValid(item) == false)
			{ throw new InvalidOperationException("The specified object is not a valid ThumbnailViewerItem"); }
		}
		
		#endregion Item Management
	}
	
	internal class ThumbnailViewer_ThumbnailPanel : Panel
	{
		public ThumbnailViewer_ThumbnailPanel()
		{
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.ResizeRedraw, true);
			SetStyle(ControlStyles.Selectable, true);
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
		}
	}
	
	internal class ThumbnailViewer_VScrollBar : VScrollBar
	{ 
		public ThumbnailViewer_VScrollBar()
		{ SetStyle(ControlStyles.Selectable, false); }
	}
}
