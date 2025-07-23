/*
 * User: Sam Brinly
 * Date: 2/12/2013
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Scanquire.Public
{
    /// <summary>A single logical page containing a single image in a document.</summary>
    public class SQPage
    {
        private readonly SQImage _Image;
        /// <summary>The image to be drawn on this page</summary>
        public SQImage Image
        { get { return _Image; } }

        private readonly List<ISQCommand_Page> _Commands = new List<ISQCommand_Page>();
        /// <summary>Any page commands associated with the page.</summary>
        public List<ISQCommand_Page> Commands
        { get { return _Commands; } }

        public SQPage(SQImage image)
        { _Image = image; }

        public SQPage(SQImage image, IEnumerable<ISQCommand_Page> commands)
            : this(image)
        { _Commands.AddRange(commands); }  
    }

    /*
	public class SQPage
	{		
		private SQCommandList _Commands;
		public SQCommandList Commands
		{
			get 
			{ 
				if (_Commands == null) _Commands = new SQCommandList();
				return _Commands;
			}
			set { _Commands = value; }
		}
		
		public SQPage()
		{  }
		
		public SQPage(ISQCommandList commands)
		{ Commands = commands; }
	}
    */
/*
	{
		private List<SQBookmarkCommand> _Bookmarks = new List<SQBookmarkCommand>();
		public List<SQBookmarkCommand> Bookmarks
		{
			get { return _Bookmarks; }
			set { _Bookmarks = value; }
		}
		
		private SQCommandList _Commands = new SQCommandList();
		public SQCommandList Commands
		{
			get { return _Commands; }
			set { _Commands = value; }
		}
		
		public SQPage() {}
		
		public SQPage(IEnumerable<ISQCommand> commands)
		{ Commands.AddRange(commands); }
	}
	*/
	/*
	public class SQImagePage : SQPage
	{
		public SQImage Image { get; set; }
		
		public SQImagePage(SQImage image)
		{
			Image = image;
		}
		
		public SQImagePage(SQImage image, SQCommandList commands) : this(image)
		{ base.Commands = commands; }
	}
	
	public class SQTextPage : SQPage
	{
		private List<SQPageTextCommand> _PageTextCommands = new List<SQPageTextCommand>();
		public List<SQPageTextCommand> PageTextCommands
		{
			get { return _PageTextCommands; }
			set { _PageTextCommands = value; }
		}
		
		private Size _Size = new Size((int)(72 * 8.5), (int)(72 * 11));
		public Size Size
		{
			get { return _Size; }
			set { _Size = value; }
		}
		
	}
	*/
	/*
	public class SQPage
	{
		private List<SQPageText> _PageText = new List<SQPageText>();
		public List<SQPageText> PageText
		{
			get { return _PageText; }
			set { _PageText = value; }
		}
		
		private List<string> _Bookmarks = new List<string>();
		public List<string> Bookmarks
		{
			get { return _Bookmarks; }
			set { _Bookmarks = value; }
		}
		
		/// <summary>Physical size of the page (in Points)</summary>
		public Size Size { get; set; }
		
		private SQCommandList _Commands;
		public SQCommandList Commands
		{
			get 
			{ 
				if (_Commands == null) _Commands = new SQCommandList();
				return _Commands;
			}
			set { _Commands = value; }
		}
		
		public SQImage Image { get; set; }
		
		public SQPage()
		{  }
		
		public SQPage(ISQCommandList commands)
		{ Commands = commands; }
		
		public SQPage(SQImage image)
		{
			Commands = new SQCommandList();
			Image = image;
		}
		
		public SQPage(SQImage image, SQCommandList commands)
		{
			Commands = commands;
			Image = image;
		}
	}
	*/
}
