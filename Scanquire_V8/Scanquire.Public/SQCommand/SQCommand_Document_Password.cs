using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scanquire.Public
{
    /// <summary>A command to specify the password of a document.</summary>
    /// <remarks>For document types using owner/user password combinations use the more specific commands.</remarks>
    public class SQCommand_Document_Password : ISQCommand_Document
    {
        private readonly string _Password;
        public string Password { get { return _Password; } }

        public SQCommand_Document_Password(string password)
        { this._Password = password; }
    }
}
