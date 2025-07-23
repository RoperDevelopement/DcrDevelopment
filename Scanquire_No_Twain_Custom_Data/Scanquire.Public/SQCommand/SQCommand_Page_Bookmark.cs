/*
 * User: Sam Brinly
 * Date: 2/6/2013
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Scanquire.Public
{
    /// <summary>For document types that support bookmarks, specify a bookmark be applied to the specified page.</summary>
	public class SQCommand_Page_Bookmark : ISQCommand_Page
	{
        private readonly string _Title;
        /// <summary>Title of the bookmark to apply to the specified page.</summary>
        public string Title { get { return _Title; } }

        public SQCommand_Page_Bookmark(string title)
		{ this._Title = title; }
	}
}
