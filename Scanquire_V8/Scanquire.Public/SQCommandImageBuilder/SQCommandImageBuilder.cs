using EdocsUSA.Controls;
using EdocsUSA.Utilities;
using EdocsUSA.Utilities.Extensions;
using FreeImageAPI;
using Polenter.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using EdocsUSA.Utilities.Logging;
namespace Scanquire.Public
{
    /// <summary>
    /// SQCommandImageBuilder that prompts the user to enter a header, footer, comments and bookmarks and applys them to the page.
    /// </summary>
    public class SQCommandImageBuilder : SQCommandImageBuilderBase
    {
        protected class ImageBuilderInputDialog : TableLayoutPanelInputDialog
        {
            public ValidatingTextBox<string> BookmarksTextBox;
            
            public ValidatingTextBox<string> HeaderTextBox;
            public ValidatingTextBox<string> CommentsTextBox;
            public ValidatingTextBox<string> FooterTextBox;

            private void InitializeComponent()
            {
                
                this.Size = new Size(640, 480);

                this.Caption = "Fill in the command page details\nUse [ctrl][Enter] to add new lines";

                AddControl(new CaptionLabel("Bookmarks\n(one per line)"), 0, 0);
                BookmarksTextBox = new ValidatingTextBox<string>()
                {
                    Multiline = true,
                    Size = new Size(500, 75),
                    RequiresValue = false
                };
                AddControl(BookmarksTextBox, 1, 0);

                AddControl(new CaptionLabel("Header"), 0, 1);
                HeaderTextBox = new ValidatingTextBox<string>()
                {
                    Multiline = true,
                    Size = new Size(500, 75),
                    RequiresValue = false
                };
                AddControl(HeaderTextBox, 1, 1);

                AddControl(new CaptionLabel("Comments"), 0, 2);
                CommentsTextBox = new ValidatingTextBox<string>()
                {
                    Multiline = true,
                    Size = new Size(500, 75),
                    RequiresValue = false
                };
                AddControl(CommentsTextBox, 1, 2);

                AddControl(new CaptionLabel("Footer"), 0, 3);
                FooterTextBox = new ValidatingTextBox<string>()
                {
                    Multiline = true,
                    Size = new Size(500, 75),
                    RequiresValue = false
                };
                AddControl(FooterTextBox, 1, 3);

            }

            public ImageBuilderInputDialog()
                : base(2, 4)
            {
                InitializeComponent();
            }

            public override void ClearInput()
            {
                //TODO:Only clear comments?
                HeaderTextBox.Clear();
                CommentsTextBox.Clear();
                FooterTextBox.Clear();
                BookmarksTextBox.Clear();                
            }

            protected override void OnShown(EventArgs e)
            {
                base.OnShown(e);
                CommentsTextBox.Focus();
                CommentsTextBox.SelectAll();
            }
        }

        #region Properties

        protected ImageBuilderInputDialog InputDialog = new ImageBuilderInputDialog();

        //TODO: Configure no prompt generation.

        private string _BarcodeEncoderName = "DEFAULT";
        /// <summary>Identifier of the barcode encoder to use for any commands requiring barcodes.</summary>
        public string BarcodeEncoderName
        {
            get { return _BarcodeEncoderName; }
            set { _BarcodeEncoderName = value; }
        }

        private ISQBarcodeEncoder _BarcodeEncoder = null;
        /// <summary>ISQBarcodeEncoder defined by BarcodeEncoderName</summary>
        [ExcludeFromSerialization]
        public ISQBarcodeEncoder BarcodeEncoder
        {
            get
            {
                if (_BarcodeEncoder == null)
                { _BarcodeEncoder = SQBarcodeEncoders.Instance[BarcodeEncoderName]; }
                return _BarcodeEncoder;
            }
            set { _BarcodeEncoder = value; }
        }

        private string[] _Bookmarks = new string[0];
        /// <summary>List of all bookmark titles to apply to the command image.</summary>
        public string[] Bookmarks
        {
            get { return _Bookmarks; }
            set { _Bookmarks = value; }
        }

        private Font _DefaultTextFont = new Font(GenericFontFamilies.SansSerif.ToString(), ImageTools.POINTS_PER_QUARTER_INCH, FontStyle.Regular, GraphicsUnit.Point);
        /// <summary>Default font to apply to the image for drawing text.</summary>
        [ExcludeFromSerialization]
        public Font DefaultTextFont
        {
            get { return _DefaultTextFont; }
            set { _DefaultTextFont = value; }
        }

        private Brush _DefaultTextBrush = Brushes.Black;
        /// <summary>Default brush to apply when drawing text.</summary>
        [ExcludeFromSerialization]
        public Brush DefaultTextBrush
        {
            get { return _DefaultTextBrush; }
            set { _DefaultTextBrush = value; }
        }

        /// <summary>Text to apply to the header section of the image</summary>
        public string Header { get; set; }

        /// <summary>Text to apply to the footer section of the image.</summary>
        public string Footer { get; set; }
                
        /// <summary>Path to the log file (if any) to apply to the image.</summary>
        public string LogoPath { get; set; }
        
        private SizeF _LogoSize = SizeF.Empty;
        /// <summary>Size of the logo to apply to the image.</summary>
        public SizeF LogoSize 
        {
            get { return _LogoSize; }
            set { _LogoSize = value; }
        }

        private bool _PromptForComments = true;
        /// <summary>Specify wether to ask the user for input prior to generation.</summary>
        public bool PromptForComments
        {
            get { return _PromptForComments; }
            set { _PromptForComments = value; }
        }

        #endregion Properties


        public override IEnumerable<Task<SQImage>> Build(IProgress<ProgressEventArgs> progress, CancellationToken cToken)
        {
            TraceLogger.TraceLoggerInstance.TraceInformation("Building image for barcode");
            //Populate the input dialog with the default values.
            InputDialog.BookmarksTextBox.Clear();
            System.Collections.IEnumerator bookmarksEnumerator = Bookmarks.GetEnumerator();
            if (bookmarksEnumerator.MoveNext())
            { InputDialog.BookmarksTextBox.Value += bookmarksEnumerator.Current; }
            while (bookmarksEnumerator.MoveNext())
            { InputDialog.BookmarksTextBox.Value += Environment.NewLine + bookmarksEnumerator.Current; }

            InputDialog.HeaderTextBox.Value = Header;
            InputDialog.FooterTextBox.Value = Footer;

            //If requested, prompt the user to update the details
            if (PromptForComments == true)
            { InputDialog.TryShowDialog(DialogResult.OK); }

            List<List<ISQCommand_Image>> commandLists = new List<List<ISQCommand_Image>>();
            List<ISQCommand_Image> commandList = new List<ISQCommand_Image>();

            //If a logo file was specified, create the associated DrawImage command.
            if (string.IsNullOrWhiteSpace(LogoPath) == false)
            {
                using (FreeImageBitmap fib = new FreeImageBitmap(LogoPath))
                {
                    SQImage image = new SQImage(fib);
                    commandList.Add(new SQCommand_Image_DrawImage(image));
                }
            }

            //If any bookmarks were specified, add each as a DrawBookmark command.
            if (string.IsNullOrWhiteSpace(InputDialog.BookmarksTextBox.Value) == false)
            {
                string[] bookmarks = InputDialog.BookmarksTextBox.Value.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                foreach (string bookmarkTitle in bookmarks)
                {
                    SQCommand_Page_Bookmark bookmarkCommand = new SQCommand_Page_Bookmark(bookmarkTitle);
                    string barcodeText, barcodeCaption;
                    if (BarcodeEncoder.TryEncode(bookmarkCommand, out barcodeText, out barcodeCaption) == false)
                    { Trace.TraceWarning("Error encoding bookmark command " + bookmarkTitle); }
                    else
                    {
                        SQCommand_Image_DrawBarcode drawBarcodeCommand = new SQCommand_Image_DrawBarcode(barcodeText, barcodeCaption);
                        commandList.Add(drawBarcodeCommand);
                    }
                }
            }

            //If a header was specified, add the associated DrawText command
            if (string.IsNullOrWhiteSpace(InputDialog.HeaderTextBox.Value) == false)
            { commandList.Add(new SQCommand_Image_DrawText(InterpolateString(InputDialog.HeaderTextBox.Value))); }

            //If a comment was specified, add the associated DrawText command
            if (string.IsNullOrWhiteSpace(InputDialog.CommentsTextBox.Value) == false)
            { commandList.Add(new SQCommand_Image_DrawText(InterpolateString(InputDialog.CommentsTextBox.Value))); }

            //If a footer was specified, add the associated DrawText command
            if (string.IsNullOrWhiteSpace(InputDialog.FooterTextBox.Value) == false)
            { commandList.Add(new SQCommand_Image_DrawText(InterpolateString(InputDialog.FooterTextBox.Value))); }

            commandLists.Add(commandList);

            return Build(commandLists, progress, cToken);
        }

        /// <summary>Map any keywords to there defined values.</summary>
        protected virtual string InterpolateString(string value)
        {
            DateTime currentTime = DateTime.Now;
            value = value.Replace("%UTCDATE%", currentTime.ToUniversalTime().ToShortDateString() + " UTC");
            value = value.Replace("%UTCDATETIME%", currentTime.ToUniversalTime().ToString() + " UTC");
            value = value.Replace("%DATE%", currentTime.ToShortDateString() + " (UTC " + TimeZone.CurrentTimeZone.GetUtcOffset(currentTime).ToString() + ")");
            value = value.Replace("%DATETIME%", currentTime.ToString() + " (UTC " + TimeZone.CurrentTimeZone.GetUtcOffset(currentTime).ToString() + ")");
            value = value.Replace("%USER%", Environment.UserName);
            value = value.Replace("%DUSER%", Environment.UserDomainName);
            TraceLogger.TraceLoggerInstance.TraceInformation($"Image builder Map any keywords to there defined values:{value}");
            return value;
        }
    }
}
