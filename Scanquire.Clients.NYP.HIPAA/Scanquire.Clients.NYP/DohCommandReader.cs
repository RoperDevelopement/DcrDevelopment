using Scanquire.Public;
using Scanquire.Public.ArchivesConstants;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using EDL = EdocsUSA.Utilities.Logging;
namespace Scanquire.Clients.NYP
{
   public class DohCommandReader:ISQCommandReader
    {
        private ISQBarcodeReader _BarcodeReader = null;
        private string _BarcodeReaderName = "DEFAULT";

        private Dictionary<string, RegexIndexFieldCommand[]> _AccessionNumber = new Dictionary<string, RegexIndexFieldCommand[]>()
        {
             {JsonsFieldConstants.JsonFieldAccessionNumber, new RegexIndexFieldCommand[] {new RegexIndexFieldCommand(@"\d+",string.Empty, string.Empty)} }

        };

        /// <summary>Mapping of index field names to a list of regex patterns ordered by priority.</summary>
        public Dictionary<string, RegexIndexFieldCommand[]> AccessionNumber
        {
            get { return _AccessionNumber; }
            set { _AccessionNumber = value; }
        }

        /// <summary>Mapping of index field names to a list of regex patterns ordered by priority.</summary>
       
        public virtual string BarcodeReaderName
        {
            get { return _BarcodeReaderName; }
            set { _BarcodeReaderName = value; }
        }
        protected ISQBarcodeReader BarcodeReader
        {
            get
            {
                if (_BarcodeReader == null)
                { _BarcodeReader = SQBarcodeReaders.Instance[BarcodeReaderName]; }
                return _BarcodeReader;
            }
        }
        public Task<IList<ISQCommand>> Read(SQImage image, int documentNumber, int pageNumber, CancellationToken cToken)
        {
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation(string.Format("Reading barcodes from document {0} page {1}", documentNumber, pageNumber));
            return Task.Factory.StartNew<IList<ISQCommand>>(() =>
            {
                using (SQImageEditLock editLock = image.BeginEdit())
                {
                    string[] barcodeValues = BarcodeReader.ReadText(image, cToken).Result;
                    EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Found " + barcodeValues.Length + " barcodes");
                    List<ISQCommand> commands = new List<ISQCommand>();
                    commands.AddRange(ReadIndexFields(this.AccessionNumber, barcodeValues));
                    if (pageNumber == 2)
                    {
                        EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Setting separator");
                        commands.Add(new SQCommand_TerminateDocument());
                    }
                    return commands;
                }
            });
        }

        public static IList<ISQCommand> ReadIndexFields(Dictionary<string, RegexIndexFieldCommand[]> indexFieldDefinitions, string[] barcodeValues)
        {
            List<ISQCommand> commands = new List<ISQCommand>();

            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Processing " + barcodeValues.Length + " barcodes");
            foreach (KeyValuePair<string, RegexIndexFieldCommand[]> indexFieldDefinition in indexFieldDefinitions)
            { commands.AddRange(ReadIndexField(indexFieldDefinition, barcodeValues)); }

            return commands;
        }

        public static IList<ISQCommand> ReadIndexField(KeyValuePair<string, RegexIndexFieldCommand[]> indexFieldDefinition, string[] barcodeValues)
        {
            List<ISQCommand> commands = new List<ISQCommand>();
            if (barcodeValues.Length == 0)
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceWarning($"No barcodes found");
                return commands;
            }
            foreach (RegexIndexFieldCommand indexDef in indexFieldDefinition.Value)
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Looking for match {indexDef} with pattern {indexDef.MatchPattern}");
                //List of all barcode values that match the current pattern
                string[] matches = barcodeValues.Where
                    (candidate => Regex.IsMatch(candidate, indexDef.MatchPattern)).ToArray();

                //If no matches, go to the next priority
                if (matches.Length == 0)
                {
                    EDL.TraceLogger.TraceLoggerInstance.TraceInformation("No match");
                    continue;
                }

                //Remove duplicate matches
                string[] distinctMatches = matches.Distinct(StringComparer.OrdinalIgnoreCase).ToArray();

                //If multiple distinct matches, quit looking (unable to continue)
                if (distinctMatches.Length > 1)
                { break; }

                //Otherwise, we have a single distinct match, so add it.
                EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Matched " + indexFieldDefinition.Key + " with " + distinctMatches[0]);
                string fieldValue = distinctMatches[0];
                if (string.IsNullOrEmpty(indexDef.ReplacePattern) == false)
                { fieldValue = Regex.Replace(fieldValue, indexDef.ReplacePattern, indexDef.ReplaceValue); }

                commands.Add(new SQCommand_Document_IndexField(indexFieldDefinition.Key, fieldValue));
                break;
            }

            return commands;
        }

    }
}
