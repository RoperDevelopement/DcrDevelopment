using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using EDL = EdocsUSA.Utilities.Logging;
namespace Scanquire.Public
{
    /// <summary>The default barcode encoder.  Barcode commands are encoded to SQBC{Numeric CommandId}{Value Separator}{Command Text}
    /// </summary>
    public class SQBarcodeEncoder : ISQBarcodeEncoder
    {
        /// <summary>Mapping of commands to a numeric command id</summary>
        public enum CommandId
        {
            TerminateDocument = 001,
            Page_Bookmark = 002,
            Page_Exclude = 003,
            Document_IndexField = 004,
            Document_Password = 005,
            Document_OwnerPassword = 006,
            Document_UserPassword = 007,
            Page_Size = 008,
            Page_DrawImage = 009,
            Page_DrawText = 010,            
        }

        private string _CommandPrefix = "SQBC";
        /// <summary>Prefix to apply to barcode values to differentiate between barcode sources.</summary>
        public string CommandPrefix
        {
            get { return _CommandPrefix; }
            set { _CommandPrefix = value; }
        }

        private int _CommandIdPadding = 3;
        /// <summary>Number of characters to pad the command id to.</summary>
        public int CommandIdPadding
        {
            get { return _CommandIdPadding; }
            set { _CommandIdPadding = value; }
        }

        private char _ValueSeparator = ':';
        /// <summary>Character separator to apply between the commandid and the command value.</summary>
        public char ValueSeparator 
        { 
            get { return _ValueSeparator; }
            set { _ValueSeparator = value; }
        }

        private char _IndexFieldSeparator = ':';
        /// <summary>For index field commands, the character to apply between the field namd and field value.</summary>
        public char IndexFieldSeparator
        {
            get { return _IndexFieldSeparator; }
            set { _IndexFieldSeparator = value; }
        }

        #region Encoding

        protected virtual string EncodeValue(CommandId commandId, string value)
        { return EncodeValue((int)commandId, value); }

        /// <summary>Base encoding method.</summary>
        /// <param name="commandId">Mapped command id of the command type to encode.</param>
        /// <param name="value">Encoded value to apply.</param>
        /// <returns>The full encoded barcode text.</returns>
        protected virtual string EncodeValue(int commandId, string value)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(CommandPrefix);
            sb.Append(commandId.ToString().PadLeft(CommandIdPadding, '0'));
            sb.Append(ValueSeparator);
            sb.Append(value);
            return sb.ToString();
        }

        /// <summary>Attempt to encode a command to a barcode value.</summary>
        /// <param name="command">The command to encode.</param>
        /// <param name="value">If successful, the encoded barcode text.</param>
        /// <param name="caption">If successful, the encoded caption text.</param>
        /// <returns>True on success, false on failure.</returns>
        public virtual bool TryEncode(ISQCommand command, out string value, out string caption)
        {
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Attempt to encode a command to a barcode value");
            //Delegate the encoding out based on the type of command provided.
            if (command is SQCommand_Document_IndexField)
            { return TryEncode_Document_IndexField((SQCommand_Document_IndexField)command, out value, out caption); }
            else if (command is SQCommand_Document_Password)
            { return TryEncode_Document_Password((SQCommand_Document_Password)command, out value, out caption);  }
            else if (command is SQCommand_Document_OwnerPassword)
            { return TryEncode_Document_OwnerPassword((SQCommand_Document_OwnerPassword)command, out value, out caption); }
            else if (command is SQCommand_Document_UserPassword)
            { return TryEncode_Document_UserPassword((SQCommand_Document_UserPassword)command, out value, out caption); }
            else if (command is SQCommand_Page_Exclude)
            { return TryEncode_Page_Exclude((SQCommand_Page_Exclude)command, out value, out caption); }
            else if (command is SQCommand_Page_Bookmark)
            { return TryEncode_Page_Bookmark((SQCommand_Page_Bookmark)command, out value, out caption); }
            else if (command is SQCommand_TerminateDocument)
            { return TryEncode_TerminateDocument((SQCommand_TerminateDocument)command, out value, out caption); }
            else
            { return TryEncodeCustom(command, out value, out caption); }
        }

        /// <summary>Placeholder for sub-classes to handle encoding of commands not accounted for.</summary>
        /// <param name="command">The command to encode</param>
        /// <param name="value">If successful, the encoded barcode text.</param>
        /// <param name="caption">If successful, the encoded caption text.</param>
        /// <returns>True on success, false on failure.</returns>
        protected virtual bool TryEncodeCustom(ISQCommand command, out string value, out string caption)
        {
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Custom encoding not supported");
            value = null;
            caption = null;
            return false;
        }

        /// <summary>Attempt to encode a Document_IndexField command.</summary>
        /// <param name="command">The command to encode</param>
        /// <param name="value">If successful, the encoded barcode text.</param>
        /// <param name="caption">If successful, the encoded caption text.</param>
        /// <returns>True on success, false on failure.</returns>
        protected virtual bool TryEncode_Document_IndexField(SQCommand_Document_IndexField command, out string value, out string caption)
        {
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Attempt to encode a Document_IndexField command.");
            value = command.Name + IndexFieldSeparator + command.Value;
            value = EncodeValue(CommandId.Document_IndexField, value);
            caption = "Index Field: " + command.Name + ":" + command.Value;
            return true;
        }

        /// <summary>Attempt to encode a Document_OwnerPassword command.</summary>
        /// <param name="command">The command to encode</param>
        /// <param name="value">If successful, the encoded barcode text.</param>
        /// <param name="caption">If successful, the encoded caption text.</param>
        /// <returns>True on success, false on failure.</returns>
        protected virtual bool TryEncode_Document_OwnerPassword(SQCommand_Document_OwnerPassword command, out string value, out string caption)
        {
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Attempt to encode a Document_OwnerPassword command.");
            value = EncodeValue(CommandId.Document_OwnerPassword, command.Password);
            caption = "Owner Password: " + command.Password;
            return true;
        }

        /// <summary>Attempt to encode a Document_Password command.</summary>
        /// <param name="command">The command to encode</param>
        /// <param name="value">If successful, the encoded barcode text.</param>
        /// <param name="caption">If successful, the encoded caption text.</param>
        /// <returns>True on success, false on failure.</returns>
        protected virtual bool TryEncode_Document_Password(SQCommand_Document_Password command, out string value, out string caption)
        {
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Attempt to encode a Document_Password command.");
            value = EncodeValue(CommandId.Document_Password, command.Password);
            caption = "Password: " + command.Password;
            return true;
        }

        /// <summary>Attempt to encode a Document_UserPassword command.</summary>
        /// <param name="command">The command to encode</param>
        /// <param name="value">If successful, the encoded barcode text.</param>
        /// <param name="caption">If successful, the encoded caption text.</param>
        /// <returns>True on success, false on failure.</returns>
        protected virtual bool TryEncode_Document_UserPassword(SQCommand_Document_UserPassword command, out string value, out string caption)
        {
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Attempt to encode a Document_UserPassword command.");
            value = EncodeValue(CommandId.Document_UserPassword, command.Password);
            caption = "User Password: " + command.Password;
            return true;
        }

        /// <summary>Attempt to encode a Page_Bookmark command.</summary>
        /// <param name="command">The command to encode</param>
        /// <param name="value">If successful, the encoded barcode text.</param>
        /// <param name="caption">If successful, the encoded caption text.</param>
        /// <returns>True on success, false on failure.</returns>
        protected virtual bool TryEncode_Page_Bookmark(SQCommand_Page_Bookmark command, out string value, out string caption)
        {
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Attempt to encode a Page_Bookmark command.");
            value = EncodeValue(CommandId.Page_Bookmark, command.Title);
            caption = "Bookmark: " + command.Title;
            return true;
        }

        /// <summary>Attempt to encode a Page_Exclude command.</summary>
        /// <param name="command">The command to encode</param>
        /// <param name="value">If successful, the encoded barcode text.</param>
        /// <param name="caption">If successful, the encoded caption text.</param>
        /// <returns>True on success, false on failure.</returns>
        protected virtual bool TryEncode_Page_Exclude(SQCommand_Page_Exclude command, out string value, out string caption)
        {
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Attempt to encode a Page_Exclude command.");
            value = EncodeValue(CommandId.Page_Exclude, string.Empty);
            caption = "Exclude Page";
            return true;
        }

        /// <summary>Attempt to encode a TerminateDocument command.</summary>
        /// <param name="command">The command to encode</param>
        /// <param name="value">If successful, the encoded barcode text.</param>
        /// <param name="caption">If successful, the encoded caption text.</param>
        /// <returns>True on success, false on failure.</returns>
        protected virtual bool TryEncode_TerminateDocument(SQCommand_TerminateDocument command, out string value, out string caption)
        {
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Attempt to encode a TerminateDocument command.");
            value = EncodeValue(CommandId.TerminateDocument, string.Empty);
            caption = "Terminate Document";
            return true;
        }

        #endregion Encoding

        #region Decoding

        /// <summary>Attempt to separate an encoded barcode into a commnandID and encoded value string</summary>
        /// <param name="barcodeText">The barcode string to decode.</param>
        /// <param name="commandID">If successful, the comand id specified by the barcode text.</param>
        /// <param name="value">If sucessful, the encoded value specified by the barcode text.</param>
        /// <returns>True on success, false on failure.</returns>
        public bool TryParse(string barcodeText, out int commandID, out string value)
        {
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Parsing " + barcodeText); ;
            
            //Initialize the defaults for the output variables
            commandID = -1;
            value = null;

            //Ensure that a non-empty string was provided
            if (string.IsNullOrWhiteSpace(barcodeText))
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceWarning("Null or empty barcodeText provided");
                return false;
            }

            //Prefix is first
            int prefixLocation = 0;
            int prefixLength = CommandPrefix.Length;
            if (barcodeText.Length < prefixLocation + prefixLength)
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceWarning("barcodeText is too short to decode");
                return false;
            }
            string prefix = barcodeText.Substring(prefixLocation, prefixLength);
            if (string.Compare(CommandPrefix, prefix, true) != 0)
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceWarning("prefix does not match");
                return false;
            }

            //Id number is after the prefix
            int idNumberLocation = prefixLocation + prefixLength;
            int idNumberLength = CommandIdPadding;
            if (barcodeText.Length < (idNumberLocation + idNumberLength))
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceWarning("barcodeText is too short to decode");
                return false;
            }
            string idNumberText = barcodeText.Substring(idNumberLocation, idNumberLength);
            //Temp variable for the id number, don't set commandID until the end.
            int idNumber;
            if (int.TryParse(idNumberText, out idNumber) == false)
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceWarning("unable to parse command id number");
                return false;
            }

            //Separator is after the id number
            int separatorLocation = idNumberLocation + idNumberLength;
            int separatorLength = ValueSeparator.ToString().Length;
            if (barcodeText.Length < (separatorLocation + separatorLength))
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceWarning("barcodeText is too short to decode");
                return false;
            }
            string separator = barcodeText.Substring(separatorLocation, separatorLength);
            if (string.Compare(ValueSeparator.ToString(), separator, true) != 0)
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceWarning("separator does not match");
                return false;
            }
            Trace.TraceInformation("Parsed " + value + commandID);
            //Value is everything after the separator
            int valueLocation = separatorLocation + separatorLength;
            string valueText = barcodeText.Substring(valueLocation);

            //Set the return values and return true
            value = valueText;
            commandID = idNumber;

            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Parsed " + value + commandID);
            return true;
        }

        /// <summary>
        /// Attempt to decode an encoded barcode string into a series of ISQCommands
        /// </summary>
        /// <param name="barcodeText">The encoded barcode text to decode.</param>
        /// <param name="commands">If successful, the decoded commands.</param>
        /// <returns>True on success, false on failure.</returns>
        public virtual bool TryParse(string barcodeText, out IEnumerable<ISQCommand> commands)
        {
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Attempt to decode an encoded barcode string into a series of ISQCommands");
            commands = null;
            int commandId;
            string barcodeValue;
            //If able to decode to commandid/{encoded  value}, delegate parsing based on command type.
            if (TryParse(barcodeText, out commandId, out barcodeValue))
            {
                switch (commandId)
                {
                    case (int)(CommandId.Document_IndexField):
                        return TryParse_Document_IndexField(barcodeValue, out commands);
                    case (int)(CommandId.Document_OwnerPassword):
                        return TryParse_Document_OwnerPassword(barcodeValue, out commands);
                    case (int)(CommandId.Document_Password):
                        return TryParse_Document_Password(barcodeValue, out commands);
                    case (int)(CommandId.Document_UserPassword):
                        return TryParse_Document_UserPassword(barcodeValue, out commands);
                    case (int)(CommandId.Page_Bookmark):
                        return TryParse_Page_Bookmark(barcodeValue, out commands);
                    default:
                        return TryParseCustom(commandId, barcodeValue, out commands);
                }
            }
            //If unable to decode to commandid/{encoded value}, try custom parsing
            else return TryParseCustom(barcodeText, out commands);
        }

        /// <summary>Placeholder for subclasses to handle unacounted for decoding.</summary>
        /// <param name="commandId">Decoded command id.</param>
        /// <param name="barcodeValue">Encoded value string.</param>
        /// <param name="commands">If successful, list of commands decoded from the barcode.</param>
        /// <returns>True on success, False on failure.</returns>
        protected virtual bool TryParseCustom(int commandId, string barcodeValue, out IEnumerable<ISQCommand> commands)
        {
            EDL.TraceLogger.TraceLoggerInstance.TraceWarning("TryParseCustom is not supported"); ;
            
            commands = null;
            return false;
        }

        /// <summary>Placeholder for subclasses to handle unacounted for decoding.</summary>
        /// <param name="barcodeText">Encoded barcode string.</param>
        /// <param name="commands">If successful, list of commands decoded from the barcode.</param>
        /// <returns>True on success, False on failure.</returns>
        protected virtual bool TryParseCustom(string barcodeText, out IEnumerable<ISQCommand> commands)
        {
            EDL.TraceLogger.TraceLoggerInstance.TraceWarning("TryParseCustom is not supported"); 
            
            commands = null;
            return false;
        }

        /// <summary>Attempt to decode a Page_Bookmark command.</summary>
        /// <param name="barcodeValue">Encoded value portion of a barcode text.</param>
        /// <param name="commands">If successful, list of commands decoded from the barcode</param>
        /// <returns>True on success, False on failure.</returns>
        protected virtual bool TryParse_Page_Bookmark(string barcodeValue, out IEnumerable<ISQCommand> commands)
        {
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Attempt to decode a Page_Bookmark command.");
            commands = new ISQCommand[] { new SQCommand_Page_Bookmark(barcodeValue) };
            return true;
        }

        /// <summary>Attempt to decode a Document_OwnerPassword command.</summary>
        /// <param name="barcodeValue">Encoded value portion of a barcode text.</param>
        /// <param name="commands">If successful, list of commands decoded from the barcode</param>
        /// <returns>True on success, False on failure.</returns>
        protected virtual bool TryParse_Document_OwnerPassword(string barcodeValue, out IEnumerable<ISQCommand> commands)
        {
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Attempt to decode a Document_OwnerPassword command.");
            commands = new ISQCommand[] { new SQCommand_Document_OwnerPassword(barcodeValue) };
            return true;
        }

        /// <summary>Attempt to decode a Document_UserPassword command.</summary>
        /// <param name="barcodeValue">Encoded value portion of a barcode text.</param>
        /// <param name="commands">If successful, list of commands decoded from the barcode</param>
        /// <returns>True on success, False on failure.</returns>
        protected virtual bool TryParse_Document_UserPassword(string barcodeValue, out IEnumerable<ISQCommand> commands)
        {
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Attempt to decode a Document_UserPassword command.");
            commands = new ISQCommand[] { new SQCommand_Document_UserPassword(barcodeValue) };
            return true;
        }

        /// <summary>Attempt to decode a Document_Password command.</summary>
        /// <param name="barcodeValue">Encoded value portion of a barcode text.</param>
        /// <param name="commands">If successful, list of commands decoded from the barcode</param>
        /// <returns>True on success, False on failure.</returns>
        protected virtual bool TryParse_Document_Password(string barcodeValue, out IEnumerable<ISQCommand> commands)
        {
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Attempt to decode a Document_Password command.");
            commands = new ISQCommand[] { new SQCommand_Document_Password(barcodeValue) };
            return true;
        }

        /// <summary>Attempt to decode a Document_IndexField command</summary>
        /// <param name="barcodeValue">Encoded value portion of a barcode text.</param>
        /// <param name="commands">If successful, list of commands decoded from the barcode</param>
        /// <returns>True on success, False on failure.</returns>
        protected virtual bool TryParse_Document_IndexField(string barcodeValue, out IEnumerable<ISQCommand> commands)
        {
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Attempt to decode a Document_IndexField command");
            commands = null;

            string[] args = barcodeValue.Split(IndexFieldSeparator);
            if (args.Length != 2)
            {
                Debug.WriteLine("Invalid number of arguments for index field command - " + barcodeValue);
                return false; 
            }

            SQCommand_Document_IndexField cmd = new SQCommand_Document_IndexField(args[0], args[1]);
            commands = new ISQCommand[] { cmd };
            return true;
        }

        #endregion Decoding
    }   
}