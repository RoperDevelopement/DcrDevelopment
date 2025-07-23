using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Scanquire.Public;
using System.Text.RegularExpressions;

namespace DemoArchivers
{
    public class NYPBarcodeEncoder : ISQBarcodeEncoder
    {
        //Array of regex barcode definitions, ordered by highest priority to lowest.
        private string[] _ValidBarcodeDefinitions = new string[]
        {   
            "^.{14}$",
            "^.{15}$",
            "^.{18}$",
            "^.{9}$"
        };
        public string[] ValidBarcodeDefinitions
        {
            get { return _ValidBarcodeDefinitions; }
            set { _ValidBarcodeDefinitions = value; }
        }

        private string _IndexFieldName = "Index_x0020_Number";
        public string IndexFieldName
        {
            get { return _IndexFieldName; }
            set { _IndexFieldName = value; }
        }

        public bool TryEncode(ISQCommand command, out string barcodeText, out string caption)
        { throw new NotImplementedException(); }

        public bool TryParse(string text, out IEnumerable<ISQCommand> commands)
        {
            foreach (string pattern in _ValidBarcodeDefinitions)
            {
                if (Regex.IsMatch(text, pattern))
                {
                    commands = new ISQCommand[] { new SQCommand_Document_IndexField(IndexFieldName, text) };
                    return true;
                }
            }
            commands = new ISQCommand[0];
            return false;
        }
    }
}
