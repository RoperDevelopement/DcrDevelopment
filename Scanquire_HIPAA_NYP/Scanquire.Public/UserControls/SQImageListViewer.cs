using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EdocsUSA.Utilities.Extensions;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using EdocsUSA.Utilities;
using Scanquire.Public.Extensions;
using FreeImageAPI;
using EdocsUSA.Controls;
using System.Threading.Tasks;
using ETL = EdocsUSA.Utilities.Logging.TraceLogger;
namespace Scanquire.Public.UserControls
{
    /// <summary>
    /// Custom control for navigating SQImages.
    /// </summary>
    public partial class SQImageListViewer : UserControl
    {
        #region Enums

        /// <summary>Size of the visible thumbnails</summary>
        public enum ThumbnailSizeMode
        {
            Small,
            Medium,
            Large
        }

        public enum NavigationDirection
        {
            /// <summary>Navigate to the item directly above the active item</summary>
            Up,
            /// <summary>Navigate to the item directly below the active item</summary>
            Down,
            /// <summary>Navigate to the previous item</summary>
            Left,
            /// <summary>Navigate to the next item</summary>
            Right,
            /// <summary>Navigate to the item one screen below the active item</summary>
            PageDown,
            /// <summary>Navigate to the item one screen above the active item</summary>
            PageUp,
            /// <summary>Navigate to the first item</summary>
            Home,
            /// <summary>Navigate to the last item</summary>
            End
        }

        public enum ImageThumbnailViewMode
        {
            Thumbnails,
            Image,
            ThumbnailsAndImage
        }

        #endregion Enums

        #region Properties

        private static Properties.SQImageListViewer DefaultSettings = Properties.SQImageListViewer.Default;

        private readonly BusyStatus _BusyStatus = new BusyStatus();
        public BusyStatus BusyStatus
        { get { return _BusyStatus; } }

        private ImageThumbnailViewMode _ViewMode = ImageThumbnailViewMode.ThumbnailsAndImage;
        public ImageThumbnailViewMode ViewMode
        {
            get { return _ViewMode; }
            set
            {
                ImageThumbnailViewMode previousValue = _ViewMode;
                _ViewMode = value;
                OnViewModeChanged(previousValue);
            }
        }

        //Default width of the thumbnail panel.
        public static int DefaultSplitterDistance
        { get { return DefaultSettings.DefaultSplitterDistance; } }

        #region Item Properties

        protected readonly SynchronizedBindingList<SQImageListViewerItem> _Items;

        //protected readonly List<SQImageListViewerItem> _Items = new List<SQImageListViewerItem>();

        /// <summary>Current contents of the viewer</summary>
        public IEnumerable<SQImageListViewerItem> Items
        { get { return _Items.AsEnumerable(); } }

        /// <summary>Total number of items currently in the viewer.</summary>
        public int ItemCount
        { get { return _Items.Count; } }

        /// <summary>Enumeration of all selected items in the viewer.</summary>
        public IEnumerable<SQImageListViewerItem> Selected
        { get { return _Items.Where((item => item.Selected == true)); } }

        /// <summary>Number of selected items in the viewer.</summary>
        public int SelectedItemCount
        { get { return Selected.Count(); } }

        /// <summary>Enumeration of all un-selected items in the viewer.</summary>
        public IEnumerable<SQImageListViewerItem> UnSelected
        { get { return _Items.Where((item => item.Selected == false)); } }

        /// <summary>Total number of un-selected </summary>
        public int UnSelectedItemCount
        { get { return UnSelected.Count(); } }

        /// <summary>Enumerable of all checked items</summary>
        public IEnumerable<SQImageListViewerItem> Checked
        { get { return _Items.Where(item => (item.Checked == true)); } }

        /// <summary>Total number of checked items</summary>
        public int CheckedItemCount
        { get { return Checked.Count(); } }

        /// <summary>Enumerable of all un-checked items</summary>
        public IEnumerable<SQImageListViewerItem> UnChecked
        { get { return _Items.Where(item => (item.Checked == false)); } }

        /// <summary>Total number of un-checked items</summary>
        public int UnCheckedItemCount
        { get { return UnChecked.Count(); } }

        /// <summary>Enumerable of all images in the viewer</summary>
        public IEnumerable<SQImage> Images
        { get { return Items.Select(item => item.Value); } }

        /// <summary>Enumerable of all selected images in the viewer</summary>
        public IEnumerable<SQImage> SelectedImages
        { get { return Selected.Select(item => item.Value); } }

        /// <summary>Enumerable of all un-selected images in the viewer</summary>
        public IEnumerable<SQImage> UnSelectedImages
        { get { return UnSelected.Select(item => item.Value); } }

        /// <summary>Enumerable of all checked images in the viewer</summary>
        public IEnumerable<SQImage> CheckedImages
        { get { return Checked.Select(item => item.Value); } }

        /// <summary>Enumerable of all un-checked images in the viewer</summary>
        public IEnumerable<SQImage> UnCheckedImages
        { get { return UnChecked.Select(item => item.Value); } }

        protected SQImageListViewerItem _ActiveItem = null;
        /// <summary>The item that currently has focus</summary>
        public SQImageListViewerItem ActiveItem
        {
            get { return _ActiveItem; }
            set
            {
                SQImageListViewerItem previousValue = _ActiveItem;
                _ActiveItem = value;
                OnActiveItemChanged(previousValue);
            }
        }

        /// <summary>The index of the item that currently has focus</summary>
        public int ActiveItemIndex
        {
            get
            {
                if (HasActiveItem() == true)
                { return ActiveItem.Index; }
                else
                { return -1; }
            }
        }

        /// <summary>The image associated with the currently focused item.</summary>
        public SQImage ActiveImage
        { get { return ActiveItem == null ? null : ActiveItem.Value; } }

        #endregion Item Properties

        #region Thumbnail Display Properties

        /// <summary>Spacing between displayed thumbnails.</summary>
        protected readonly Padding ThumbnailMargin = new Padding(DefaultSettings.ThumbnailMargin);

        /// <summary>Size of the border around each thumbnail.</summary>
        protected readonly Padding ThumbnailBorder = new Padding(DefaultSettings.ThumbnailBorder);

        /// <summary>Color of a thumbnail's border when not checked or selected.</summary>
        protected readonly Color ThumbnailBorderColor = SystemColors.Control;

        /// <summary>Color of an thumbnail's border when checked.</summary>
        protected readonly Color CheckedThumbnailBorderColor = SystemColors.ControlDark;

        /// <summary>Color of a thumbnail's border when selected.</summary>
        protected readonly Color SelectedThumbnailBorderColor = SystemColors.Highlight;

        /// <summary>Font for thumbnail captions when ThumbnailSizeMode is small.</summary>
        protected readonly Font SmallThumbnailCaptionFont = SystemFonts.SmallCaptionFont;

        /// <summary>Font for thumbnail captions when ThumbnailSizeMode is medium.</summary>
        protected readonly Font MediumThumbnailCaptionFont = SystemFonts.SmallCaptionFont;

        /// <summary>Font for thumbnail captions when ThumbnailSizeMode is large.</summary>
        protected readonly Font LargeThumbnailCaptionFont = SystemFonts.CaptionFont;

        /// <summary>Brush for painting captions for selected thumbnails.</summary>
        protected readonly Brush SelectedCaptionBrush = new SolidBrush(SystemColors.HighlightText);

        /// <summary>Brush for painting captions for unselected thumbnails.</summary>
        protected readonly Brush UnSelectedCaptionBrush = new SolidBrush(SystemColors.ControlText);

        /// <summary>FormatString for painting thumbnail captions.</summary>
        protected readonly StringFormat CaptionStringFormat = new StringFormat()
        {
            LineAlignment = StringAlignment.Center,
            Alignment = StringAlignment.Center
        };

        //TODO: Retrieve from settings
        private ThumbnailSizeMode _CurrentThumbnailSizeMode = ThumbnailSizeMode.Small;
        public ThumbnailSizeMode CurrentThumbnailSizeMode
        {
            get { return _CurrentThumbnailSizeMode; }
            set
            {
                ThumbnailSizeMode previousValue = _CurrentThumbnailSizeMode;
                _CurrentThumbnailSizeMode = value;
                OnThumbnailSizeModeChanged(previousValue);
            }
        }

        /// <summary>Pixel dimensions of thumbnail images defined by ThumbnailSizeMode</summary>
        public Size ThumbnailImageSize
        {
            get
            {

                switch (CurrentThumbnailSizeMode)
                {
                    case ThumbnailSizeMode.Small:
                        return SmallThumbnailSize;
                    case ThumbnailSizeMode.Medium:
                        return MediumThumbnailSize;
                    case ThumbnailSizeMode.Large:
                        return LargeThumbnailSize;
                    default:
                        ETL.TraceLoggerInstance.TraceWarning("Unexpected ThumbnailSizeMode " + CurrentThumbnailSizeMode);
                        goto case ThumbnailSizeMode.Small;
                }
            }
        }

        /// <summary>Total size of displayed a object (including the image, caption, border and margin).</summary>
        public Size ThumbnailControlSize
        {
            get
            {
                return new Size()
                {
                    Width = ThumbnailImageSize.Width + ThumbnailMargin.Horizontal + ThumbnailBorder.Horizontal,
                    Height = ThumbnailImageSize.Height + ThumbnailMargin.Vertical + ThumbnailMargin.Vertical + ((int)Math.Ceiling(ThumbnailCaptionFont.GetHeight()))
                };
            }
        }

        private Size _LargeThumbnailSize = DefaultSettings.DefaultLargeThumbnailSize;
        /// <summary>Size of thumbnail images to display when ThumbnailSize = Large</summary>
        public Size LargeThumbnailSize
        {
            get { return _LargeThumbnailSize; }
            set
            {
                Size previousValue = _LargeThumbnailSize;
                _LargeThumbnailSize = value;
                OnLargeThumbnailSizeChanged(previousValue);
            }
        }

        private Size _SmallThumbnailSize = DefaultSettings.DefaultSmallThumbnailSize;
        /// <summary>Size of thumbnail images to display when ThumbnailSize = Small</summary>
        public Size SmallThumbnailSize
        {
            get { return _SmallThumbnailSize; }
            set
            {
                Size previousValue = _SmallThumbnailSize;
                _SmallThumbnailSize = value;
                OnSmallThumbnailSizeChanged(previousValue);
            }
        }

        private Size _MediumThumbnailSize = DefaultSettings.DefaultMediumThumbnailSize;
        /// <summary>Size of thumbnail images to display when ThumbnailSize = Medium</summary>
        public Size MediumThumbnailSize
        {
            get { return _MediumThumbnailSize; }
            set
            {
                Size previousValue = _MediumThumbnailSize;
                _MediumThumbnailSize = value;
                OnMediumThumbnailSizeChanged(previousValue);
            }
        }

        /// <summary>Maximum number of thumbnail columns that will fit in ThumbnailPanel.</summary>
        public int MaxVisibleThumbnailColumns
        {
            get
            {
                //If ThumbnailControlSize does not have a defined width, log and return zero.
                //Prevents divide by zero error.
                if (ThumbnailControlSize.Width <= 0)
                {
                    ETL.TraceLoggerInstance.TraceWarning("ThumbnailControlSize.Width is 0");
                    return 0;
                }

                return (int)(Math.Floor(Decimal.Divide(ThumbnailPanel.Width, ThumbnailControlSize.Width)));
            }
        }

        /// <summary>Maximum number of thumbnail rows that will fit in ThumbnailPanel.</summary>
        public int MaxVisibleThumbnailRows
        {
            get
            {
                //Ensure ThumbnailControlSize has a defined height.
                //Prevents divide by zero error
                if (ThumbnailControlSize.Height <= 0)
                {
                    ETL.TraceLoggerInstance.TraceWarning("ThumbnailControlSize.Height is 0");
                    return 0;
                }

                return (int)(Math.Floor(Decimal.Divide(ThumbnailPanel.Height, ThumbnailControlSize.Height)));
            }
        }

        /// <summary>Maximum number of thumbnails that will fir in ThumbnailPanel</summary>
        public int MaxVisibleThumbnails
        { get { return MaxVisibleThumbnailRows * MaxVisibleThumbnailColumns; } }

        /// <summary>Total number of rows for all thumbnails</summary>
        public int TotalThumbnailRows
        {
            get
            {
                //Prevent divide by zero error
                if (MaxVisibleThumbnailColumns <= 0)
                { return 0; }

                return (int)(Math.Ceiling(Decimal.Divide(ItemCount, MaxVisibleThumbnailColumns)));
            }
        }

        /// <summary>Index of the first visible thumbnail (based on the current scroll position)</summary>
        protected int FirstVisibleThumbnailIndex
        { get { return ThumbnailScrollBarValue * MaxVisibleThumbnailColumns; } }

        /// <summary>Index of the first visible (on-screen) item</summary>
        protected SQImageListViewerItem FirstVisibleItem
        {
            get
            {
                int index = FirstVisibleThumbnailIndex;
                return (index < 0) ? null : _Items[index];
            }
        }

        /// <summary>Index of the last visible (on-screen) item</summary>
        protected int LastVisibleThumbnailIndex
        { get { return (int)(Math.Min(FirstVisibleThumbnailIndex + MaxVisibleThumbnails - 1, ItemCount - 1)); } }

        /// <summary>The last item visible (on-screen)</summary>
        protected SQImageListViewerItem LastVisibleItem
        {
            get
            {
                int index = LastVisibleThumbnailIndex;
                return (index < 0) ? null : _Items[index];
            }
        }

        /// <summary>Maximum value for the thumbnail scrollbar (based on number of thumbnails)</summary>
        protected int MaxThumbnailScrollBarValue
        { get { return Math.Max(0, TotalThumbnailRows - MaxVisibleThumbnailRows); } }

        /// <summary>Current value of the thumbnail scrollbar</summary>
        /// <remarks>Normalizes to >= 0</remarks>
        protected int ThumbnailScrollBarValue
        {
            get { return (int)(Math.Max(0, ThumbnailScrollBar.Value)); }
            set { ThumbnailScrollBar.Value = value; }
        }

        /// <summary>Index of the initial item selected for a multi-select operation.</summary>
        protected int ThumbnailSelectionAnchor;

        /// <summary>Brush to paint the border of items that are checked.</summary>
        protected Brush CheckedThumbnailBorderBrush = new SolidBrush(SystemColors.Control);

        /// <summary>Brush to paint the border of items that are not checked</summary>
        protected Brush UnCheckedThumbnailBorderBrush = new SolidBrush(Color.DarkRed);

        /// <summary>Brush to paint the thumbnails of items that are selected</summary>
        protected Brush SelectedThumbnailBrush = new SolidBrush(Color.FromArgb(125, SystemColors.Highlight));

        /// <summary>Brush to paint the thumbnails of items that are not selected</summary>
        protected Brush DisabledThumbnailBrush = new SolidBrush(Color.FromArgb(128, SystemColors.Control));

        /// <summary>Pen for painting the border of the focused item.</summary>
        protected Pen ActiveImageBorderPen = new Pen(SystemColors.ActiveBorder)
        {
            DashStyle = DashStyle.Dash,
            Width = 3
        };

        /// <summary>Font for displaying the thumbnail caption.</summary>
        protected Font ThumbnailCaptionFont
        {
            get
            {
                switch (CurrentThumbnailSizeMode)
                {
                    case ThumbnailSizeMode.Small:
                        return SmallThumbnailCaptionFont;
                    case ThumbnailSizeMode.Medium:
                        return MediumThumbnailCaptionFont;
                    case ThumbnailSizeMode.Large:
                        return LargeThumbnailCaptionFont;
                    default:
                        ETL.TraceLoggerInstance.TraceWarning("Unexpected ThumbnailSizeMode " + CurrentThumbnailSizeMode);
                        goto case ThumbnailSizeMode.Medium;
                }
            }
        }

        #endregion Thumbnail Display Properties

        #region Image Editing Properties

        /// <summary>Amount to deskew an image by for a single manual deskew operation.</summary>
        private float _DeskewAngle = DefaultSettings.DefaultDeskewAngle;
        public float DeskewAngle
        {
            get { return _DeskewAngle; }
            set { _DeskewAngle = value; }
        }

        /// <summary>Color to fill in the selected region of the active item during a fill operation.</summary>
        public Color FillColor
        {
            get { return SelectFillColorButton.BackColor; }
            set { SelectFillColorButton.BackColor = value; }
        }

        #endregion Image Editing Properties

        #endregion Properties

        public SQImageListViewer()
        {
            //Initialize the list before initializing the component to avoid ObjectReference exceptions
            _Items = new SynchronizedBindingList<SQImageListViewerItem>(this);

            InitializeComponent();
            BusyStatus.StatusChanged += BusyStatus_StatusChanged;
            _Items.ListChanged += _Items_ListChanged;

            this.SplitContainer.SplitterDistance = DefaultSplitterDistance;
        }

        /// <summary>Disable/Enable the control during busy operations</summary>
        void BusyStatus_StatusChanged(object sender, EventArgs e)
        {
            this.UseWaitCursor = BusyStatus.Busy;
            this.Enabled = (BusyStatus.Busy == false);
            this.Focus();
        }

        #region Item Events

        private bool _Saved = true;
        /// <summary>True if the collection has not been modified since the last save operation.</summary>
        public bool Saved
        {
            get { return _Saved; }
            set
            {
                _Saved = value;
                OnSavedStatusChanged();
            }
        }

        public event EventHandler ItemCountChanged;
        public event EventHandler SelectedItemCountChanged;
        public event EventHandler CheckedItemCountChanged;
        public event EventHandler SavedStatusChanged;
        public event EventHandler ActiveItemChanged;
        public event EventHandler ActiveItemCheckedChanged;
        public event EventHandler ActiveItemSelectedChanged;
        public event EventHandler ActiveItemValueUpdated;

        void _Items_ListChanged(object sender, ListChangedEventArgs e)
        {
            switch (e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    OnItemCountChanged();
                    OnCheckedItemCountChanged();
                    NavigateTo(_Items[e.NewIndex]);
                    Saved = false;
                    break;
                case ListChangedType.ItemChanged:
                    SQImageListViewerItem changedItem = _Items[e.NewIndex];
                    Saved = false;
                    if (IsThumbnailVisible(e.NewIndex))
                    { InvalidateThumbnailPanel(); }
                    if (e.PropertyDescriptor != null)
                    {
                        string propertyName = e.PropertyDescriptor.Name;
                        switch (propertyName)
                        {
                            case "Selected":
                                if (IsActive(changedItem))
                                { OnActiveItemSelectedChanged(); }
                                OnSelectedItemCountChanged();
                                break;
                            case "Checked":
                                if (IsActive(changedItem))
                                { OnActiveItemCheckedChanged(); }
                                OnCheckedItemCountChanged();
                                break;
                            case "Value":
                                if (IsActive(changedItem))
                                { OnActiveItemValueUpdated(); }
                                break;
                        }
                    }
                    break;
                case ListChangedType.ItemDeleted:
                    //TODO:Dont invalidate if deleted item is after last visible index.
                    InvalidateThumbnailPanel();
                    Saved = false;
                    OnItemCountChanged();
                    break;
                case ListChangedType.ItemMoved:
                    InvalidateThumbnailPanel();
                    Saved = false;
                    break;
                case ListChangedType.Reset:
                    InvalidateThumbnailPanel();
                    OnItemCountChanged();
                    OnCheckedItemCountChanged();
                    OnSelectedItemCountChanged();
                    Saved = true;
                    break;
            }
        }

        protected virtual void OnItemCountChanged()
        {
            UpdateThumbnailScrollBar();
            EventHandler handler = ItemCountChanged;
            if (handler != null)
            { handler(this, null); }
        }

        protected virtual void OnSelectedItemCountChanged()
        {
            EventHandler handler = SelectedItemCountChanged;
            if (handler != null)
            { handler(this, null); }
        }

        protected virtual void OnCheckedItemCountChanged()
        {
            EventHandler handler = CheckedItemCountChanged;
            if (handler != null)
            { handler(this, null); }
        }

        protected virtual void OnSavedStatusChanged()
        {
            EventHandler handler = SavedStatusChanged;
            if (handler != null)
            { handler(this, null); }
        }

        protected virtual void OnActiveItemChanged(SQImageListViewerItem previousItem)
        {
            ETL.TraceLoggerInstance.TraceInformation("ActiveItemChanged");
            //Display the newly activated image in the image viewer and reset the scaling.
            UpdateActiveImageViewer();
            ActiveImageViewer.ScaleToFit();

            if (ActiveItem != null)
            { EnsureThumbnailIsVisible(ActiveItem); }

            InvalidateThumbnailPanel();

            EventHandler handler = ActiveItemChanged;
            if (handler != null)
            { handler(this, null); }
        }

        protected virtual void OnActiveItemCheckedChanged()
        {
            UpdateActiveImageViewerBorder();

            EventHandler handler = ActiveItemCheckedChanged;
            if (handler != null)
            { handler(this, null); }
        }

        protected virtual void OnActiveItemSelectedChanged()
        {
            EventHandler handler = ActiveItemSelectedChanged;
            if (handler != null)
            { handler(this, null); }
        }

        protected virtual void OnActiveItemValueUpdated()
        {
            UpdateActiveImageViewer();

            EventHandler handler = ActiveItemValueUpdated;
            if (handler != null)
            { handler(this, null); }
        }

        #endregion Item Event Handlers

        #region Misc Events

        public event EventHandler ViewModeChanged;

        #endregion Misc Events

        #region Misc Event Handlers

        protected virtual void OnViewModeChanged(ImageThumbnailViewMode previousValue)
        {

            switch (ViewMode)
            {
                case ImageThumbnailViewMode.Image:
                    SplitContainer.Panel1Collapsed = true;
                    break;
                case ImageThumbnailViewMode.Thumbnails:
                    SplitContainer.Panel2Collapsed = true;
                    break;
                case ImageThumbnailViewMode.ThumbnailsAndImage:
                    SplitContainer.Panel1Collapsed = false;
                    SplitContainer.Panel2Collapsed = false;
                    break;
                default:
                    ETL.TraceLoggerInstance.TraceWarning("Unexpected ImageThumbnailViewMode " + ViewMode);
                    goto case ImageThumbnailViewMode.ThumbnailsAndImage;
            }
            UpdateThumbnailScrollBar();
            EventHandler handler = ViewModeChanged;
            if (handler != null)
            { handler(this, null); }
        }

        #endregion Misc Event Handlers

        #region Image Viewer Event Handlers

        private void UpdateActiveImageViewerBorder()
        {
            ETL.TraceLoggerInstance.TraceInformation("Updating active image viewer border");
            if ((HasActiveItem() == false) || (ActiveItem.Checked == true))
            { ActiveImageBorderPanel.BackColor = SystemColors.Control; }
            else
            { ActiveImageBorderPanel.BackColor = Color.DarkRed; }
        }

        private void UpdateActiveImageViewer()
        {
            ETL.TraceLoggerInstance.TraceInformation("Updating active image viewer");
            UpdateActiveImageViewerBorder();

            //Dispose the previous value if any.
            if (ActiveImageViewer.Image != null)
            {
                Image previousImage = ActiveImageViewer.Image;
                ActiveImageViewer.Image = null;
                previousImage.Dispose();
            }

            if (HasActiveItem())
            {
                Bitmap bitmap = ActiveImage.LatestRevision.GetOriginalImageBitmap();
                ActiveImageViewer.Image = bitmap;
                UndoRevisionButton.Enabled = ActiveImage.RevisionCount > 0;
            }
        }

        #endregion Image Viewer Event Handlers

        #region Thumbnail Display Event Handlers


        protected virtual void OnThumbnailSizeModeChanged(ThumbnailSizeMode previousValue)
        {
            if (previousValue == CurrentThumbnailSizeMode)
            { return; }

            InvalidateThumbnailPanel();
        }

        protected virtual void OnSmallThumbnailSizeChanged(Size previousValue)
        {
            if (previousValue == SmallThumbnailSize)
            { return; }

            InvalidateThumbnailPanel();
        }

        protected virtual void OnLargeThumbnailSizeChanged(Size previousValue)
        {
            if (previousValue == LargeThumbnailSize)
            { return; }

            InvalidateThumbnailPanel();
        }

        protected virtual void OnMediumThumbnailSizeChanged(Size previousValue)
        {
            if (previousValue == LargeThumbnailSize)
            { return; }

            InvalidateThumbnailPanel(); ;
        }

        #endregion Thumbnail Display Event Handlers

        #region Item Management

        /// <returns>True if the specified item exists in the current collection</returns>
        public bool Contains(SQImageListViewerItem item)
        { return _Items.Contains(item); }

        /// <returns>True if the specified image exists in the current collection</returns>
        public bool Contains(SQImage image)
        { return Images.Contains(image); }

        /// <summary>Throws an InvalidOperationException if the specified item does not exist in the current collection</summary>
        public void EnsureContains(SQImageListViewerItem item)
        {
            if (Contains(item) == false)
            {
                ETL.TraceLoggerInstance.TraceError("The requested operation requires that the specified item exists");
                throw new InvalidOperationException("The requested operation requires that the specified item exists");
            }
        }

        /// <summary>Throws an InvalidOperationException if the specified item exists in the current collection</summary>
        public void EnsureDoesNotContain(SQImageListViewerItem item)
        {
            if (Contains(item) == true)
            {
                ETL.TraceLoggerInstance.TraceError("The requested operation cannot be performed with an existing item");
                throw new InvalidOperationException("The requested operation cannot be performed with an existing item");
            }
        }

        /// <summary>Throws an InvalidOperationException if the specified item is not selected</summary>
        public void EnsureSelected(SQImageListViewerItem item)
        {
            if (item.Selected == false)
            {
                ETL.TraceLoggerInstance.TraceError("The requested operation requires the specified item to be selected");
                throw new InvalidOperationException("The requested operation requires the specified item to be selected");
            }
        }

        /// <summary>Throws an InvalidOperationException if the specified item is selected</summary>
        public void EnsureNotSelected(SQImageListViewerItem item)
        {
            if (item.Selected == true)
            {
                ETL.TraceLoggerInstance.TraceError("The requested operation cannot be performed on a selected item");
                throw new InvalidOperationException("The requested operation cannot be performed on a selected item");
            }
        }

        /// <summary>Throws an InvalidOperationException if the specified item is not checked</summary>
        public void EnsureChecked(SQImageListViewerItem item)
        {
            if (item.Checked == false)
            { throw new InvalidOperationException("The requested operation requires the specified item to be checked"); }
        }

        /// <summary>Throws an InvalidOperationException if the specified item is checked</summary>
        public void EnsureNotChecked(SQImageListViewerItem item)
        {
            if (item.Checked == true)
            {
                ETL.TraceLoggerInstance.TraceError("The requested operation cannot be performed on a checked item");
                throw new InvalidOperationException("The requested operation cannot be performed on a checked item");
            }
        }

        /// <returns>True if the specified item currently holds focus.</returns>
        public bool IsActive(SQImageListViewerItem item)
        { return item == ActiveItem; }

        /// <summary>Throws an InvalidOperationException if the specified item does not hold focus</summary>
        public void EnsureActive(SQImageListViewerItem item)
        {
            if (IsActive(item) == false)
            { throw new InvalidOperationException("The requested operation can only be performed on the active item"); }
        }

        /// <summary>Throws an InvalidOperationException if the specified item holds focus</summary>
        public void EnsureNotActive(SQImageListViewerItem item)
        {
            if (IsActive(item) == true)
            {
                ETL.TraceLoggerInstance.TraceError("The requested operation cannot be performed on the active item");
                throw new InvalidOperationException("The requested operation cannot be performed on the active item");
            }
        }

        /// <returns>True if the viewer has an active item.</returns>
        public bool HasActiveItem()
        { return ActiveItem != null; }

        /// <summary>Throws an InvalidOperationException if the collection does not contain an active item</summary>
        public void EnsureHasActiveItem()
        {
            if (HasActiveItem() == false)
            {
                ETL.TraceLoggerInstance.TraceError("The requested operation requires that an item be active");
                throw new InvalidOperationException("The requested operation requires that an item be active");
            }
        }

        /// <summary>Throws an InvalidOperationException if the collection contains an active item</summary>
        public void EnsureDoesNotHaveActiveItem()
        {
            if (HasActiveItem() == true)
            {
                ETL.TraceLoggerInstance.TraceError("The requested operation cannot be performed with an item activated");
                throw new InvalidOperationException("The requested operation cannot be performed with an item activated");
            }
        }

        /// <returns>True if the currently focused item is checked.</returns>
        public bool IsActiveItemChecked()
        {
            if (HasActiveItem() == true)
            { return ActiveItem.Checked; }
            else
            { return false; }
        }

        /// <summary>Throws an InvalidOperationException if the focused item is not checked.</summary>
        public void EnsureActiveItemIsChecked()
        {
            if (IsActiveItemChecked() == false)
            {
                ETL.TraceLoggerInstance.TraceError("The requested operation requires the active image to be checked");
                throw new InvalidOperationException("The requested operation requires the active image to be checked");
            }
        }

        /// <summary>Throws an InvalidOperationException if the focused item is checked.</summary>
        public void EnsureActiveItemIsNotChecked()
        {
            if (IsActiveItemChecked() == true)
            {
                ETL.TraceLoggerInstance.TraceError("The requested operation cannot be performed while the active item is checked");
                throw new InvalidOperationException("The requested operation cannot be performed while the active item is checked");
            }
        }

        /// <returns>True if the currently focused item is selected.</returns>
        public bool IsActiveItemSelected()
        {
            if (HasActiveItem() == true)
            { return ActiveItem.Checked; }
            else
            { return false; }
        }

        /// <summary>Throws an InvalidOperationException if the focused item is not selected.</summary>
        public void EnsureActiveItemIsSelected()
        {
            if (IsActiveItemSelected() == false)
            {
                ETL.TraceLoggerInstance.TraceError("The requested operation requires the active item to be selected");
                throw new InvalidOperationException("The requested operation requires the active item to be selected");
            }
        }

        /// <summary>Throws an InvalidOperationException if the focused item is selected.</summary>
        public void EnsureActiveItemIsNotSelected()
        {
            if (IsActiveItemSelected() == true)
            {
                ETL.TraceLoggerInstance.TraceError("The requested operation cannot be performed while the active item is selected");
                throw new InvalidOperationException("The requested operation cannot be performed while the active item is selected");
            }
        }

        /// <returns>The index of the specified item within the collection.</returns>
        public int IndexOf(SQImageListViewerItem item)
        { return _Items.IndexOf(item); }

        /// <summary>Insert a new image before the specified index</summary>
        public void _Insert(int index, SQImage image)
        {
            SQImageListViewerItem item = new SQImageListViewerItem(this, image);
            if ((index < 0) || (index > (ItemCount - 1)))
            { _Items.Add(item); }
            else
            { _Items.Insert(index, item); }
        }

        /// <summary>Insert a new image before the focused item.</summary>
        private void Insert(SQImage image)
        {
            int index = ActiveItemIndex;
            Insert(image);
        }

        /// <summary>Insert a new image before the specified index</summary>
        public void Insert(int index, SQImage image)
        { _Insert(index, image); }

        /// <summary>Asynchronously Insert multiple images before the specified index.</summary>
        public async Task Insert(int index, IEnumerable<SQImage> source)
        {
            BusyStatus.Set();
            try
            {
                await Task.Factory.StartNew(() =>
                {
                    foreach (SQImage image in source)
                    {
                        _Insert(index, image);
                        if (index >= 0)
                        { index++; }
                    }
                });
            }
            finally
            { BusyStatus.Clear(); }
        }

        /// <summary>Asynchronously Insert multiple images before the focused item.</summary>
        public async Task Insert(IEnumerable<SQImage> source)
        { await Insert(ActiveItemIndex, source); }

        /// <summary>Asynchronously add multiple images to the end of the collection.</summary>
        public async Task Add(IEnumerable<SQImage> source)
        { await Insert(-1, source); }

        /// <summary>Add an image to the end of the collection.</summary>
        public void Add(SQImage image)
        { Insert(-1, image); }

        /// <summary>Remove the specified item from the collection</summary>
        public void Remove(SQImageListViewerItem item)
        { _Items.Remove(item); }

        /// <summary>Remove the item located at the specified index from the collection</summary>
        public void Remove(int index)
        { _Items.RemoveAt(index); }

        /// <summary>Remove the specified items from the collection</summary>
        public void Remove(IEnumerable<SQImageListViewerItem> items)
        {
            BusyStatus.Set();
            try
            {
                //Reverse sort the items by index to avoid having to adjust indexs after each removal.
                items.OrderBy(item => IndexOf(item)).Reverse().ForEach(Remove);
            }
            finally
            { BusyStatus.Clear(); }
        }

        /// <summary>Change the specified item's checked status to true.</summary>
        public void Check(SQImageListViewerItem item)
        {
            if (item.Checked == false)
            { item.Checked = true; }
        }

        /// <summary>Change the specified items' checked status to true.</summary>
        public void Check(IEnumerable<SQImageListViewerItem> items)
        { items.ForEach(Check); }

        /// <summary>Change all items' checked status to true.</summary>
        public void CheckAll()
        { Check(Items); }

        /// <summary>Change all selected items' checked status to true.</summary>
        public void CheckSelected()
        { Check(Selected); }

        /// <summary>Change the specified items' checked status to false.</summary>
        public void UnCheck(SQImageListViewerItem item)
        {
            if (item.Checked == true)
            { item.Checked = false; }
        }

        /// <summary>Change the specified items' checked status to false.</summary>
        public void UnCheck(IEnumerable<SQImageListViewerItem> items)
        { items.ForEach(UnCheck); }

        /// <summary>Change all items' checked status to false.</summary>
        public void ClearChecked()
        { UnCheck(Checked); }

        /// <summary>Change all selected items' checked status to false.</summary>
        public void UnCheckSelected()
        { UnCheck(Selected); }

        /// <summary>Toggle the status of the checked state of the specified item.</summary>
        public void InvertCheck(SQImageListViewerItem item)
        { item.Checked = !item.Checked; }

        /// <summary>Toggle the status of the checked state of the specified items.</summary>
        public void InvertCheck(IEnumerable<SQImageListViewerItem> items)
        { items.ForEach(InvertCheck); }

        /// <summary>Toggle the status of the checked state of all selected items.</summary>
        public void InvertCheckSelected()
        { InvertCheck(Selected); }

        /// <summary>Change the selection state of the specified item to true.</summary>
        public void Select(SQImageListViewerItem item)
        {
            if (item.Selected == false)
            { item.Selected = true; }
        }

        /// <summary>Change the selection state of the specified items to true.</summary>
        public void Select(IEnumerable<SQImageListViewerItem> items)
        { items.ForEach(Select); }

        /// <summary>Change the selection state of all items to true.</summary>
        public void SelectAll()
        { Select(Items); }

        /// <summary>Change the selection state of the specified item to false.</summary>
        public void UnSelect(SQImageListViewerItem item)
        {
            if (item.Selected == true)
            { item.Selected = false; }
        }

        /// <summary>Change the selection state of the specified items to false.</summary>
        public void UnSelect(IEnumerable<SQImageListViewerItem> items)
        { items.ForEach(UnSelect); }

        /// <summary>Change the selection state of all items to false.</summary>
        public void ClearSelected()
        { UnSelect(Selected); }

        /// <summary>Toggle the selection state of the specified item.</summary>
        public void InvertSelect(SQImageListViewerItem item)
        { item.Selected = !item.Selected; }

        /// <summary>Toggle the selection state of the specified items.</summary>
        public void InvertSelect(IEnumerable<SQImageListViewerItem> items)
        { items.ForEach(InvertSelect); }

        /// <summary>Set focus on the specified item.</summary>
        public void ActivateItem(SQImageListViewerItem item)
        {
            if (IsActive(item) == false)
            { ActiveItem = item; }
        }

        /// <summary>Remove focus from the currently focused item (if any)</summary>
        public void ClearActive()
        { ActiveItem = null; }

        /// <returns>The items in the collection from index start to index end</returns>
        public IEnumerable<SQImageListViewerItem> GetRange(int start, int end)
        {
            if ((start < 0) || (start > (ItemCount - 1)))
            {
                ETL.TraceLoggerInstance.TraceError("Start index invalid");
                throw new ArgumentOutOfRangeException("start");
            }
            if ((end < 0) || (end > (ItemCount - 1)))
            {
                ETL.TraceLoggerInstance.TraceError("End index invalid");
                throw new ArgumentOutOfRangeException("end");
            }

            return IntExtensions.Range(start, end).Select(index => _Items[index]);
        }

        /// <returns>The items in the collection from item start to item end.</returns>
        public IEnumerable<SQImageListViewerItem> GetRange(SQImageListViewerItem start, SQImageListViewerItem end)
        { return GetRange(start.Index, end.Index); }

        /// <summary>Removes all items from the collection</summary>
        /// <param name="confirmWithUser">True to confirm with the user if the collection contains images and the saved flag is not set.</param>
        /// <param name="disposeImages">True if the image objects are no longer needed and can be disposed.</param>
        public void ClearAll(bool confirmWithUser, bool disposeImages)
        {
           
            //If it has items and is not saved and confirmWithUsers is true,
            //  ask the user and continue or fail.
            if ((Saved == false) && (ItemCount > 0) && (confirmWithUser == true))
            {
                DialogResult result = MessageBox.Show("Not saved, are you sure?", "Clear", MessageBoxButtons.YesNo);
                if (result != DialogResult.Yes)
                {
                    ETL.TraceLoggerInstance.TraceError("Found images don't clear images");
                    throw new OperationCanceledException();
                }
                ETL.TraceLoggerInstance.TraceError("Clear images list with out saving");
            }

            //If disposing, get a copy of all items
            //Wait untill the list is cleared before disposing to prevent null reference exceptions and excess events.
            SQImageListViewerItem[] items = new SQImageListViewerItem[0];
            if (disposeImages)
            { items = Items.ToArray(); }

            //Perform the clear.
            ClearActive();
            int numbereItem = _Items.Count;
            ETL.TraceLoggerInstance.TraceInformation("Clearing items");
            _Items.Clear();
            ETL.TraceLoggerInstance.TraceInformation($"Total Items {numbereItem.ToString()} cleared");

            //Finally dispose all images if requested
            if (disposeImages)
            {
                foreach (SQImageListViewerItem item in items)
                { item.Value.Dispose(); }
            }
            ETL.TraceLoggerInstance.TraceInformation("Items disposed");
        }

        public void ClearAll(bool disposeImages)
        {

            //If it has items and is not saved and confirmWithUsers is true,
            //  ask the user and continue or fail.
            

            //If disposing, get a copy of all items
            //Wait untill the list is cleared before disposing to prevent null reference exceptions and excess events.
            SQImageListViewerItem[] items = new SQImageListViewerItem[0];
            if (disposeImages)
            { items = Items.ToArray(); }

            //Perform the clear.
            ClearActive();
            int numbereItem = _Items.Count;
            ETL.TraceLoggerInstance.TraceInformation("Clearing items");
            _Items.Clear();
            ETL.TraceLoggerInstance.TraceInformation($"Total Items {numbereItem.ToString()} cleared");

            //Finally dispose all images if requested
            if (disposeImages)
            {
                foreach (SQImageListViewerItem item in items)
                { item.Value.Dispose(); }
            }
            ETL.TraceLoggerInstance.TraceInformation("Items disposed");
        }

        #endregion Item Management

        #region Thumbnail Navigation

        /// <summary>Set focus to the item at the specified index and ensure that it is visible.</summary>
        public void NavigateTo(int index)
        {
            SQImageListViewerItem item = _Items[index];
            switch (ModifierKeys)
            {
                case Keys.Control:
                    ThumbnailSelectionAnchor = index;
                    Select(item);
                    ActivateItem(item);
                    break;
                case Keys.Shift:
                    ThumbnailSelectionAnchor = ThumbnailSelectionAnchor >= 0 ? ThumbnailSelectionAnchor : 0;
                    ClearSelected();
                    Select(GetRange(ThumbnailSelectionAnchor, index));
                    ActivateItem(item);
                    break;
                case Keys.Shift | Keys.Control:
                    ThumbnailSelectionAnchor = ThumbnailSelectionAnchor >= 0 ? ThumbnailSelectionAnchor : 0;
                    Select(GetRange(ThumbnailSelectionAnchor, index));
                    ActivateItem(item);
                    break;
                default:
                    ThumbnailSelectionAnchor = index;
                    ClearSelected();
                    Select(item);
                    ActivateItem(item);
                    break;
            }
            EnsureThumbnailIsVisible(index);
        }

        /// <summary>Set focus to the item at the specified item and ensure that it is visible.</summary>
        public void NavigateTo(SQImageListViewerItem item)
        { NavigateTo(IndexOf(item)); }

        /// <summary>Set focus to the item in the specified direction from the current active item..</summary>
        public void Navigate(NavigationDirection direction)
        {
            if (ItemCount <= 0)
            { return; }

            // new active image index.
            // note: this will be constrained to the list bounds at the end, so don't worry about constraining now.			
            int activeItemIndex = HasActiveItem() ? ActiveItem.Index : 0;
            switch (direction)
            {
                case NavigationDirection.Up:
                    activeItemIndex -= MaxVisibleThumbnailColumns;
                    break;
                case NavigationDirection.Down:
                    activeItemIndex += MaxVisibleThumbnailColumns;
                    break;
                case NavigationDirection.Left:
                    //If the current active image is unchecked, just activate the previous image.
                    if (_Items[activeItemIndex].Checked == false)
                    { activeItemIndex -= 1; }

                    //Otherwise (active image is checked), find the previous checked image.
                    else
                    {
                        activeItemIndex -= 1;
                        while (activeItemIndex >= 0)
                        {
                            if (_Items[activeItemIndex].Checked == true)
                            { break; }
                            else { activeItemIndex -= 1; }
                        }
                    }
                    break;
                case NavigationDirection.Right:
                    //If the current active image is unchecked, just activate the next image.
                    if (_Items[activeItemIndex].Checked == false)
                    { activeItemIndex++; }

                    //Otherwise (active image is checked), find the next checked image
                    else
                    {
                        activeItemIndex++;
                        while ((activeItemIndex <= ItemCount - 1)
                            && (_Items[activeItemIndex].Checked == false))
                        { activeItemIndex++; }
                    }
                    break;
                case NavigationDirection.PageUp:
                    activeItemIndex -= MaxVisibleThumbnailColumns * MaxVisibleThumbnailRows;
                    break;
                case NavigationDirection.PageDown:
                    activeItemIndex += MaxVisibleThumbnailColumns * MaxVisibleThumbnailRows;
                    break;
                case NavigationDirection.Home:
                    activeItemIndex = 0;
                    break;
                case NavigationDirection.End:
                    activeItemIndex = ItemCount - 1;
                    break;
                default:
                    ETL.TraceLoggerInstance.TraceWarning("Unexpected NavigationDirection " + direction);
                    break;
            }

            activeItemIndex = activeItemIndex.ConstrainTo(0, ItemCount - 1);

            NavigateTo(activeItemIndex);
        }

        /// <param name="index">The index of the item relative to all visible items.</param>
        /// <returns>The position of the top left corner of the thumbnail at the specified index.</returns>
        protected Point GetThumbnailPosition(int index)
        {
            if (MaxVisibleThumbnailColumns == 0)
            {
                ETL.TraceLoggerInstance.TraceWarning("MaxVisibleThumbnailColumns is 0");
                return new Point(0, 0);
            }
            return new Point()
            {
                X = index % MaxVisibleThumbnailColumns,
                Y = (int)(Math.Floor(Decimal.Divide(index, MaxVisibleThumbnailColumns)))
            };
        }

        /// <returns>The position of the top left corner of the thumbnail at the specified item.</returns>
        protected Point GetThumbnailPosition(SQImageListViewerItem item)
        {
            EnsureContains(item);
            return GetThumbnailPosition(IndexOf(item));
        }

        /// <returns>The item (if any) that is located at the specified point.</returns>
        protected SQImageListViewerItem HitTest(Point p)
        {
            //TODO: Ensure the point is located in the thumbnail region?

            int col = (int)(Math.Floor(Decimal.Divide(p.X, ThumbnailControlSize.Width)));
            ETL.TraceLoggerInstance.TraceInformation("Col " + col);
            ETL.TraceLoggerInstance.TraceInformation("Max Visible Col: " + MaxVisibleThumbnailColumns);
            col.ConstrainTo(0, MaxVisibleThumbnailColumns - 1);
            ETL.TraceLoggerInstance.TraceInformation("Col " + col);
            int row = (int)(Math.Floor(Decimal.Divide(p.Y, ThumbnailControlSize.Height)));
            row += ThumbnailScrollBarValue;
            ETL.TraceLoggerInstance.TraceInformation("Row " + row);

            int index = (row * MaxVisibleThumbnailColumns) + col;
            ETL.TraceLoggerInstance.TraceInformation("Index " + index);

            if (index > ItemCount - 1)
            { return null; }

            return _Items[index];
        }

        protected void UpdateThumbnailScrollBar()
        { ThumbnailScrollBar.Maximum = MaxThumbnailScrollBarValue; }

        protected void ScrollToThumbnailRow(int row)
        { ThumbnailScrollBarValue = row.ConstrainTo(0, MaxThumbnailScrollBarValue); }

        /// <returns>True if the item at the specified collection index is currently visible (on-screen)</returns>
        protected bool IsThumbnailVisible(int index)
        {
            return ((index > IndexOf(FirstVisibleItem))
                && (index <= IndexOf(LastVisibleItem)));
        }

        /// <returns>True if the item at the specified item is currently visible (on-screen)</returns>
        protected bool IsThumbnailVisible(SQImageListViewerItem item)
        { return IsThumbnailVisible(item.Index); }

        /// <summary>Throws an InvalidOperationException if the item at the specified index collection is not visible (on-screen)</summary>
        protected void EnsureThumbnailIsVisible(int index)
        {
            ETL.TraceLoggerInstance.TraceInformation("EnsureThumbnailIsVisible start");
            int row = ThumbnailScrollBarValue;
            ETL.TraceLoggerInstance.TraceInformation("row = " + row);
            if (index < FirstVisibleThumbnailIndex)
            { row = GetThumbnailPosition(index).Y; }
            else if (index > LastVisibleThumbnailIndex)
            { row = GetThumbnailPosition(index).Y - (MaxVisibleThumbnailRows - 1); }
            ETL.TraceLoggerInstance.TraceInformation("Scrolling to row " + row);
            ScrollToThumbnailRow(row);
            ETL.TraceLoggerInstance.TraceInformation("EnsureThumbnailIsVisible end");
        }

        /// <summary>Throws an InvalidOperationException if the item at the specified item is not visible (on-screen)</summary>
        protected void EnsureThumbnailIsVisible(SQImageListViewerItem item)
        { EnsureThumbnailIsVisible(item.Index); }

        #endregion Thumbnail Navigation

        #region ThumbnailPanel Event Handlers


        protected void InvalidateThumbnailPanel()
        {
            if (ThumbnailPanel != null)
            { ThumbnailPanel.Invalidate(); }
            UpdateThumbnailScrollBar();
        }

        private void ThumbnailPanel_Click(object sender, EventArgs e)
        {
            //See if the click corresponds to an item, or just the panel.
            SQImageListViewerItem item = HitTest(ThumbnailPanel.PointToClient(Cursor.Position));
            //If not an item, just focus the control
            if (item == null)
            {
                ThumbnailPanel.Focus();
                return;
            }

            //Otherwise, an item was clicked...
            //If the control key is pressed and the images is already selected, de-select it.
            if ((ModifierKeys == Keys.Control) && (item.Selected == true))
            { UnSelect(item); }
            //Otherwise, navigate to the new item
            else
            { NavigateTo(item); }

            this.Focus();
        }

        private void ThumbnailPanel_Paint(object sender, PaintEventArgs e)
        {
            //If no images, or no room to draw images, just return
            if ((ItemCount == 0)
                || (MaxVisibleThumbnailRows == 0)
                || (MaxVisibleThumbnailColumns == 0))
            { return; }

            int currentItemIndex = FirstVisibleThumbnailIndex;

            //Loop through the visible rows (until we reach the end of the list)
            for (int row = 0; row < MaxVisibleThumbnailRows; row++)
            {
                if (currentItemIndex >= ItemCount)
                { break; }

                //Loop through the visible columns (until we reach the end of the list)
                for (int col = 0; col < MaxVisibleThumbnailColumns; col++)
                {
                    if (currentItemIndex >= ItemCount)
                    { break; }

                    //Calculate the boundary of the thumbnail control
                    Rectangle thumbnailControlRegion = new Rectangle()
                    {
                        X = (col * ThumbnailControlSize.Width),
                        Y = (row * ThumbnailControlSize.Height),
                        Width = ThumbnailControlSize.Width,
                        Height = ThumbnailControlSize.Height
                    };

                    //Calculate the boundary of the thumbnail border
                    Rectangle thumbnailBorderRegion = new Rectangle()
                    {
                        X = thumbnailControlRegion.X + ThumbnailMargin.Left,
                        Y = thumbnailControlRegion.Y + ThumbnailMargin.Top,
                        Width = thumbnailControlRegion.Width - ThumbnailMargin.Horizontal,
                        Height = thumbnailControlRegion.Height - ThumbnailMargin.Vertical
                    };

                    //Calculate the boundary of the thumbnail image
                    Rectangle thumbnailImageRegion = new Rectangle()
                    {
                        X = thumbnailBorderRegion.X + ThumbnailBorder.Left,
                        Y = thumbnailBorderRegion.Y + ThumbnailBorder.Top,
                        Width = ThumbnailImageSize.Width,
                        Height = ThumbnailImageSize.Height
                    };

                    //Calculate the boundary of the thumbnail caption
                    Rectangle thumbnailCaptionRegion = new Rectangle()
                    {
                        X = thumbnailImageRegion.X,
                        Y = thumbnailImageRegion.Y + thumbnailImageRegion.Height,
                        Width = thumbnailImageRegion.Width,
                        Height = ThumbnailCaptionFont.Height
                    };

                    //Calculate the boundary of the selection rectangle
                    Rectangle selectRegion = thumbnailControlRegion;
                    //Calculate the boundary of the focus rectangle
                    Rectangle activeRegion = thumbnailControlRegion;

                    SQImageListViewerItem currentItem = _Items[currentItemIndex];

                    //Draw the border, based on the iamge's checked state
                    Brush borderBrush = currentItem.Checked ? CheckedThumbnailBorderBrush : UnCheckedThumbnailBorderBrush;
                    e.Graphics.FillRectangle(borderBrush, thumbnailBorderRegion);

                    //Draw the image
                    e.Graphics.DrawImage(currentItem.Value.Thumbnail, thumbnailImageRegion);

                    //Draw a transparent selection rectangle if required
                    if (currentItem.Selected == true)
                    { e.Graphics.FillRectangle(SelectedThumbnailBrush, selectRegion); }

                    //Draw the caption
                    Brush captionBrush = currentItem.Selected ? SelectedCaptionBrush : UnSelectedCaptionBrush;

                    //Color captionColor = Images.IsItemSelected(currentImage) ? SystemColors.HighlightText : SystemColors.ControlText;
                    string captionText = (currentItemIndex + 1).ToString();

                    e.Graphics.DrawString(captionText, ThumbnailCaptionFont, captionBrush, thumbnailCaptionRegion, CaptionStringFormat);

                    //TextRenderer.DrawText(e.Graphics, captionText, ThumbnailCaptionFont, thumbnailCaptionRegion, captionColor, captionFormatFlags);

                    //Draw a dashed rectangle if the image is active
                    if (IsActive(currentItem))
                    { e.Graphics.DrawRectangle(ActiveImageBorderPen, activeRegion); }

                    //Draw a transparent disabled rectangle if this is not enabled
                    if (this.Enabled == false)
                    { e.Graphics.FillRectangle(DisabledThumbnailBrush, selectRegion); }

                    //increment the index
                    currentItemIndex++;
                }
            }
        }

        #endregion ThumbnailPanel Event Handlers

        #region ThumbnailScrollBar Event Handlers

        private void ThumbnailScrollBar_ValueChanged(object sender, EventArgs e)
        { InvalidateThumbnailPanel(); }

        #endregion ThumbnailScrollBar Event Handlers

        #region Control Event Handlers

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {

            switch (keyData)
            {
                case Keys.Shift | Keys.Right:
                case Keys.Control | Keys.Right:
                case Keys.Shift | Keys.Control | Keys.Right:
                case Keys.Right:
                    Navigate(NavigationDirection.Right);
                    return true;
                case Keys.Shift | Keys.Left:
                case Keys.Control | Keys.Left:
                case Keys.Shift | Keys.Control | Keys.Left:
                case Keys.Left:
                    Navigate(NavigationDirection.Left);
                    return true;
                case Keys.Shift | Keys.Up:
                case Keys.Control | Keys.Up:
                case Keys.Shift | Keys.Control | Keys.Up:
                case Keys.Up:
                    Navigate(NavigationDirection.Up);
                    return true;
                case Keys.Shift | Keys.Down:
                case Keys.Control | Keys.Down:
                case Keys.Shift | Keys.Control | Keys.Down:
                case Keys.Down:
                    Navigate(NavigationDirection.Down);
                    return true;
                case Keys.Shift | Keys.PageUp:
                case Keys.Control | Keys.PageUp:
                case Keys.Shift | Keys.Control | Keys.PageUp:
                case Keys.PageUp:
                    Navigate(NavigationDirection.PageUp);
                    return true;
                case Keys.Shift | Keys.PageDown:
                case Keys.Control | Keys.PageDown:
                case Keys.Shift | Keys.Control | Keys.PageDown:
                case Keys.PageDown:
                    Navigate(NavigationDirection.PageDown);
                    return true;
                case Keys.Shift | Keys.End:
                case Keys.Control | Keys.End:
                case Keys.Shift | Keys.Control | Keys.End:
                case Keys.End:
                    Navigate(NavigationDirection.End);
                    return true;
                case Keys.Shift | Keys.Home:
                case Keys.Control | Keys.Home:
                case Keys.Shift | Keys.Control | Keys.Home:
                case Keys.Home:
                    Navigate(NavigationDirection.Home);
                    return true;
                case Keys.Control | Keys.A:
                    SelectAll();
                    return true;
                case Keys.Control | Keys.T:
                    InvertCheckSelected();
                    return true;
                case Keys.Control | Keys.Y:
                    CheckSelected();
                    return true;
                case Keys.Control | Keys.U:
                    UnCheckSelected();
                    return true;
                case Keys.Control | Keys.R:
                    RotateSelectedImagesRightButton.PerformClick();
                    return true;
                case Keys.Control | Keys.E:
                    RotateSelectedImagesLeftButton.PerformClick();
                    return true;
                case Keys.Control | Keys.F:
                    RotateSelectedImages180DegreesButton.PerformClick();
                    return true;
                default: return base.ProcessCmdKey(ref msg, keyData);
            }
        }

        /// <summary>Scroll the thumbnail panel when the mouse-wheel is moved.</summary>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            if (e.Delta == 0) return;

            //TODO:Set as env variable
            int WHEEL_DELTA = 120;
            int newScrollValue = ThumbnailScrollBarValue - (int)(e.Delta / WHEEL_DELTA);
            ScrollToThumbnailRow(newScrollValue);
        }

        #endregion Control Event Handlers

        #region Thumbnail ToolStrip Event Handlers

        private void ChangeThumbnailSizeButton_Click(object sender, EventArgs e)
        {
            switch (CurrentThumbnailSizeMode)
            {
                case ThumbnailSizeMode.Small:
                    CurrentThumbnailSizeMode = ThumbnailSizeMode.Medium;
                    break;
                case ThumbnailSizeMode.Medium:
                    CurrentThumbnailSizeMode = ThumbnailSizeMode.Large;
                    break;
                case ThumbnailSizeMode.Large:
                    CurrentThumbnailSizeMode = ThumbnailSizeMode.Small;
                    break;
                default:
                    Trace.TraceWarning("Unexpected ThumbnailSizeMode " + CurrentThumbnailSizeMode);
                    goto case ThumbnailSizeMode.Medium;
            }
        }

        private void InvertCheckSelectedImagesButton_Click(object sender, EventArgs e)
        { InvertCheckSelected(); }

        private async void RotateSelectedImagesRightButton_Click(object sender, EventArgs e)
        { await RotateFlipSelected(RotateFlipType.Rotate90FlipNone); }

        private async void RotateSelectedImagesLeftButton_Click(object sender, EventArgs e)
        { await RotateFlipSelected(RotateFlipType.Rotate270FlipNone); }

        private async void RotateSelectedImages180DegreesButton_Click(object sender, EventArgs e)
        { await RotateFlipSelected(RotateFlipType.Rotate180FlipNone); }

        private async void FlipSelectedImagesHorizontalButton_Click(object sender, EventArgs e)
        { await RotateFlipSelected(RotateFlipType.RotateNoneFlipX); }

        private async void FlipSelectedImagesVerticalButton_Click(object sender, EventArgs e)
        { await RotateFlipSelected(RotateFlipType.RotateNoneFlipY); }

        #endregion Thumbnail ToolStrip Event Handlers

        #region Image Editing

        /// <summary>Rotate the specified image by the specified rotation type.</summary>
        protected void _RotateImage(SQImage image, RotateFlipType rotateFlipType)
        {
            using (SQImageEditLock edit = image.BeginEdit())
            {
                image.WorkingCopy.RotateFlipEx(rotateFlipType);
                image.Save(true);
            }

        }

        /// <summary>Rotate the specified image by the specified rotation type.</summary>
        protected async Task RotateImage(SQImage image, RotateFlipType rotateFlipType)
        {
            await Rotate(new SQImage[] { image }, rotateFlipType);
        }

        /// <summary>Asynchronously rotate the specified images by the specified rotation type.</summary>
        protected async Task _Rotate(IEnumerable<SQImage> images, RotateFlipType rotateFlipType)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (SQImage image in images)
                { _RotateImage(image, rotateFlipType); }
            });
        }

        /// <summary>Asynchronously rotate the specified images by the specified rotation type.</summary>
        protected async Task Rotate(IEnumerable<SQImage> images, RotateFlipType rotateFlipType)
        {
            BusyStatus.Set();
            try
            {
                await _Rotate(images, rotateFlipType);
            }
            finally
            { BusyStatus.Clear(); }
        }

        /// <summary>Asynchronously rotate all selected images by the specified rotation type.</summary>
        protected async Task RotateFlipSelected(RotateFlipType rotateFlipType)
        { await Rotate(SelectedImages, rotateFlipType); }

        /// <summary>Crop the specified image to the specified region.</summary>
        protected void _CropImage(SQImage image, Rectangle region)
        {
            using (SQImageEditLock edit = image.BeginEdit())
            {
                image.WorkingCopy = image.WorkingCopy.Copy(ActiveImageViewer.SelectedRectangle);
                image.Save();
            }
        }

        /// <summary>Crop the specified image to the specified region.</summary>
        protected void CropImage(SQImage image, Rectangle region)
        { BusyStatus.PerformBusyingAction(() => _CropImage(image, region)); }

        /// <summary>Crop the active image to the specified region.</summary>
        public void CropActive(Rectangle? region = null)
        {
            EnsureHasActiveItem();

            if (region.HasValue == false)
            {
                ActiveImageViewer.EnsureSelectionExists();
                region = ActiveImageViewer.SelectedRectangle;
            }

            CropImage(ActiveImage, region.Value);
        }

        /// <summary>Prompt the user to select the color to be used for future Fill operations.</summary>
        protected void SelectFillColor()
        {
            if (SelectFillColorDialog.ShowDialog() != DialogResult.OK)
            { return; }

            FillColor = SelectFillColorDialog.Color;
        }

        /// <summary>Fill the specified region of the specified image with the specified color.</summary>
        protected void _FillRectangle(SQImage image, Rectangle region, Color color)
        {
            using (SQImageEditLock edit = image.BeginEdit())
            {
                image.WorkingCopy.FillRectangle(region, color);
                image.Save();
            }

        }

        /// <summary>Fill the specified region of the specified image with the specified color.</summary>
        protected void FillRectangle(SQImage image, Rectangle region, Color color)
        { BusyStatus.PerformBusyingAction(() => _FillRectangle(image, region, color)); }

        /// <summary>Fill the specified region of the active image with the specified color.</summary>
        public void FillActiveImage(Rectangle? region = null, Color? color = null)
        {
            EnsureHasActiveItem();

            if (region.HasValue == false)
            {
                ActiveImageViewer.EnsureSelectionExists();
                region = ActiveImageViewer.SelectedRectangle;
            }

            if (color.HasValue == false)
            { color = FillColor; }

            FillRectangle(ActiveImage, region.Value, color.Value);
        }

        /// <summary>Deskew the specified image by the specified angle.</summary>
        protected void _Deskew(SQImage image, float angle)
        {
            using (SQImageEditLock edit = image.BeginEdit())
            {
                float hRes = image.WorkingCopy.HorizontalResolution;
                float vRes = image.WorkingCopy.VerticalResolution;

                using (Bitmap bmp = image.WorkingCopy.ToBitmap())
                {
                    using (Bitmap rotatedBitmap = bmp.Rotate(angle))
                    {
                        FreeImageBitmap fib = new FreeImageBitmap(rotatedBitmap);
                        fib.SetResolution(hRes, vRes);
                        image.WorkingCopy = fib;
                    }
                }

                image.Save();
            }
        }

        /// <summary>Deskew the specified image by the specified angle.</summary>
        public void Deskew(SQImage image, float angle)
        { BusyStatus.PerformBusyingAction(() => _Deskew(image, angle)); }

        /// <summary>Deskew the specified image to the right by the default deskew angle.</summary>
        protected void DeskewRight(SQImage image)
        { Deskew(image, DeskewAngle); }

        /// <summary>Deskew the active image to the right by the default deskew angle.</summary>
        protected void DeskewActiveItemRight()
        {
            if (HasActiveItem() == false)
            { return; }
            DeskewRight(ActiveImage);
        }

        /// <summary>Deskew the specified image to the left by the default deskew angle.</summary>
        protected void DeskewLeft(SQImage image)
        { Deskew(image, -DeskewAngle); }

        /// <summary>Deskew the active image to the right by the default deskew angle.</summary>
        protected void DeskewActiveItemLeft()
        {
            if (HasActiveItem() == false)
            { return; }
            DeskewLeft(ActiveImage);
        }

        /// <summary>Undo a single revision of the specified image.</summary>
        public void UndoLatestRevision(SQImage image)
        { image.RollbackSingleRevision(); }

        /// <summary>Undo a single revision of the active image.</summary>
        public void UndoActiveItemLatestRevision()
        {
            if (HasActiveItem() == false)
            { return; }
            UndoLatestRevision(ActiveImage);
        }


        #endregion Image Editing

        #region ImageViewer Toolstrip Event Handlers

        private void ZoomInButton_Click(object sender, EventArgs e)
        { ActiveImageViewer.ZoomIn(); }

        private void ZoomOutButton_Click(object sender, EventArgs e)
        { ActiveImageViewer.ZoomOut(); }

        private void ScaleToFitViewerButton_Click(object sender, EventArgs e)
        { ActiveImageViewer.ScaleToFit(); }

        private void ScaleToOriginalSizeButton_Click(object sender, EventArgs e)
        { ActiveImageViewer.ScaleToOriginalSize(); }

        private void ScaleToFitViewerHeightButton_Click(object sender, EventArgs e)
        { ActiveImageViewer.ScaleToFitHeight(); }

        private void ScaleFitViewerWidthButton_Click(object sender, EventArgs e)
        { ActiveImageViewer.ScaleToFitWidth(); }

        /// <summary>
        /// Display a dialog containing information about the active dialog.
        /// </summary>
        private void DisplayImageInfoButton_Click(object sender, EventArgs e)
        {
            if (HasActiveItem() == false)
            { return; }

            StringBuilder infoMessageBuilder = new StringBuilder();

            using (SQImageEditLock edit = ActiveImage.BeginEdit())
            {
                int widthPx = ActiveImage.WorkingCopy.Width;
                int heightPx = ActiveImage.WorkingCopy.Height;
                float hRes = ActiveImage.WorkingCopy.HorizontalResolution;
                float vRes = ActiveImage.WorkingCopy.VerticalResolution;
                float widthIn = (float)(Math.Round((widthPx / hRes), 2));
                float heightIn = (float)(Math.Round((heightPx / vRes), 2));
                string pixelFormat = ActiveImage.WorkingCopy.PixelFormat.ToString();

                infoMessageBuilder.AppendLine("Dimensions (Pixels):\t\t" + widthPx + "x" + heightPx);
                infoMessageBuilder.AppendLine("Resolution (DPI):\t\t" + hRes + "x" + vRes);
                infoMessageBuilder.AppendLine("Dimensions (Inches):\t\t" + widthIn + "x" + heightIn);
                infoMessageBuilder.AppendLine("Pixel Format:\t\t" + pixelFormat);
            }

            MessageBox.Show(infoMessageBuilder.ToString(), "Image Summary");
        }

        private void ActiveImageToolStripTabControl_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPage == EditToolStripTabPage)
            { ActiveImageViewer.ToolMode = ImageViewer.MouseToolMode.Select; }
            else
            { ActiveImageViewer.ToolMode = ImageViewer.MouseToolMode.Pan; }
        }

        private void CropSelectedRegionButton_Click(object sender, EventArgs e)
        {
            try
            {
                CropActive();
            }
            
            catch (Exception ex)
            {
                MessageBox.Show($"Error Crop {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
}

        private void FillSelectedRegionButton_Click(object sender, EventArgs e)
        {
            try
            {
                FillActiveImage();
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Error Fill Active Image {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SelectFillColorButton_Click(object sender, EventArgs e)
        {
            try
            {
                SelectFillColor();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error Fill Color {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void DeskewLeftButton_Click(object sender, EventArgs e)
        {
            try
            {
                DeskewActiveItemLeft();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error Deskew Left {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void DeskewRightButton_Click(object sender, EventArgs e)
        {
            try
            {
                DeskewActiveItemRight();
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Error Deskwe Right {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UndoRevisionButton_Click(object sender, EventArgs e)
        {
            try
            { 
            UndoActiveItemLatestRevision();
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Error LatestRevision {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion ImageViewer Toolstrip Event Handlers

        private void ThumbnailPanel_Resize(object sender, EventArgs e)
        {
            try
            {
                InvalidateThumbnailPanel();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error Thumbnail {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

      

    }

    /// <summary>A warpper for an SQImage to be added to an SQImageViewer</summary>
    public class SQImageListViewerItem : INotifyPropertyChanged
    {
        #region Properties

        private bool _Checked = true;
        public bool Checked
        {
            get { return _Checked; }
            set 
            { 
                _Checked = value;
                OnCheckedChanged();
                OnPropertyChanged("Checked");
            }
        }

        private bool _Selected = false;
        public bool Selected
        {
            get { return _Selected; }
            set 
            { 
                _Selected = value;
                OnSelectedChanged();
                OnPropertyChanged("Selected");
            }
        }

        private readonly SQImage _Value;
        public SQImage Value
        { get { return _Value; } }

        private readonly SQImageListViewer _Owner;
        /// <summary>The SQImageListViewer containing this item.</summary>
        public SQImageListViewer Owner
        { get { return _Owner; } }

        /// <summary>The collection index of this item within it's owning SQImageListViewer.</summary>
        public int Index
        { get { return Owner.IndexOf(this); } }

        #endregion Properties

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler CheckedChanged;

        public event EventHandler SelectedChanged;

        public event EventHandler ValueUpdated;

        #endregion Events

        #region Constructors

        public SQImageListViewerItem(SQImageListViewer owner, SQImage value)
        {
            this._Owner = owner;
            this._Value = value;
            this.Value.RevisionsChanged += Value_RevisionsChanged;
            this.Value.PropertyChanged += Value_PropertyChanged;
        }

        void Value_PropertyChanged(object sender, PropertyChangedEventArgs e)
        { OnPropertyChanged("Value"); }

        #endregion Constructors

        #region Event Handlers

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            { handler(this, new PropertyChangedEventArgs(propertyName)); }
        }

        protected void OnCheckedChanged()
        {
            EventHandler handler = CheckedChanged;
            if (handler != null)
            { handler(this, null); }
        }

        protected void OnSelectedChanged()
        {
            EventHandler handler = SelectedChanged;
            if (handler != null)
            { handler(this, null); }
        }

        protected virtual void Value_RevisionsChanged(object sender, EventArgs e)
        {
            EventHandler handler = ValueUpdated;
            if (handler != null)
            { handler(this, null); }
        }

        #endregion Event Handlers
    }

}
