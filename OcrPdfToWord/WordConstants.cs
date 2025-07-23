//------------------------------------------------------------------
// <copyright file="MsWord.cs" company="Microsoft">
//     Copyright (C) Microsoft Corporation.  All rights reserved.
// </copyright>
//
// <author email="a-darope">
//     Dan Roper
// </author>
//
// <Description>
//     Defines WordConstants Class
//     
// <Description>
//------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Ms.Word
{
    /// <summary>
    /// Const used for build a word document
    /// </summary>
  public class WordConstants
    {
      
        public static Object WORDENDOFDOC = "\\endofdoc"; /* \endofdoc is a predefined bookmark */
        public const string CARRIAGERETURN = "\r";
        public const string WINWORDPROCESS = "WINWORD";
        public const string BOOKMARKTABLECONTENTS = "BookMark_TOC";
        public const string BOOKMARKHEADER = "BookMark_Header";
        public const string NEWBOOKMARK = "BookMark";
        public const string STYLEHEADING1 = "Heading1";
        public const string STYLEHEADING1WORDDOC = "Heading 1";
        public const string STYLEHEADING2 = "Heading2";
        public const string STYLEHEADING3 = "Heading3";
        public const string STYLEHEADING5 = "Heading5";
        public const string STYLEHEADING4 = "Heading4";
        public const string STYLEHEADING6 = "Heading6";
        public const string STYLEHEADING7 = "Heading7";
        public const string STYLEHEADING9 = "Heading 7";
        public const string STYLEHEADINGNORMAL = "Normal";
        public const string STYLEHEADINGSTRONG = "Strong";
        public const string STYLETITLE = "Title";
        public const string STYLETITLENOHEDING = "TitleNoHeading";
        public const string STYLESUBTITLE = "Subtitle";
        public const string STYLESUBTITLEEMPHASIS = "Subtle Emphasis";
        public const string STYLESUBTITLEREFERENCE = "Subtle Reference";
        public const string HEADERTABLEOFCONTENTS = "Contents";
        public static Object OMSWORDMISSING = Missing.Value;
        public static Object OMSWORDTRUE = (bool)true;
        public static Object OMSWORDFALSE = (bool)false;
        public const string WORDFONTCALIBRIBODY = "Segoe UI";
        public const string WORDFONTPLUSBODY = "+Body";
        public const string WORDFONTPLUSHEADINGS = "+Headings";
        public const string WORDFONTARIAL = "Arial";
        public const string WORDFONTARIALBLACK = "Arial Black";
        public const string WORDFONTCALIBRI = "Segoe UI";
        public const string WORDFONTCOURIERNEW = "Segoe UI";
        public const string WORDFONTCOURIER = "Segoe UI";
        public const string WORDFONTCONSOLAS = "Lucida Console";
        public int MAXBOOKMARKS = 12000;
        public const string WORDRCARRIAGERETURN = "^p"; //word carriage return
        public const string EMPTYTABLECELL = "\r\a";//empty table celll
        public const string HEADING = "Heading ";
        public const int HEADING1FONTSIZE = 20;
        public const int HEADING2FONTSIZE = 18;
        public const int HEADING3FONTSIZE = 12;
        public const int TITLEFONTSIZE = 26;
        public const string INSERTPARA = "{InsertPara}";
        public const string CHANGEHITORY = "Change History";
        public const string WORDDOCPAGEBREAK = "\f\r";
        public const string WORDIMAGE = "/\r"; //definds a word image in a document
        public const string STYLETITLECOVERPAG = "DSTOC1-0";
        public const string STYLETITLECP = "Text Indented";
        public const string STYLETITLEABSTRACT = "DSTOC3-0";
        public const string STYLETITLEFIGURE = "Figure";
        public const string STYLETTEXTINDENTED = "Text Indented,ti";
        public const string STYLETOCHEADING = "TOC Heading";
        public const string STYLETOC = "TOC1";
    }
}
