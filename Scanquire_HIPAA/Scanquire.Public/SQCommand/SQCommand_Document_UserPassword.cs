using System;
using EdocsUSA.Utilities.Extensions;

namespace Scanquire.Public
{
    /// <summary>A command to specify the user password of a document.</summary>
    /// <remarks>For document types only needing a single password, use the more generic command type.</remarks>
    public class SQCommand_Document_UserPassword : ISQCommand_Document
    {
        private readonly string _Password;
        public string Password { get { return _Password; } }

        public SQCommand_Document_UserPassword(string password)
        { this._Password = password; }
    }
}
