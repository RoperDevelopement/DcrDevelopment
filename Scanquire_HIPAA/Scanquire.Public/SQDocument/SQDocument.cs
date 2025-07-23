using System;
using System.Collections.Generic;
using System.Linq;

namespace Scanquire.Public
{
    /// <summary>A collection of SQPages and ISQCommands that apply to the document level.</summary>
    public class SQDocument
    {
        private List<ISQCommand_Document> _Commands = new List<ISQCommand_Document>();
        public List<ISQCommand_Document> Commands
        { get { return _Commands; } }

        private List<SQPage> _Pages = new List<SQPage>();
        public List<SQPage> Pages
        { get { return _Pages; } }

        public SQDocument(IEnumerable<ISQCommand_Document> commands, IEnumerable<SQPage> pages)
        { 
            this._Commands = commands.ToList();
            this._Pages = pages.ToList();
            
        }

        public SQDocument(IEnumerable<ISQCommand_Document> commands)
        { this._Commands = commands.ToList(); }

        public SQDocument(IEnumerable<SQPage> pages)
        { this._Pages = pages.ToList(); }

        public SQDocument()
        { }
    }
}
