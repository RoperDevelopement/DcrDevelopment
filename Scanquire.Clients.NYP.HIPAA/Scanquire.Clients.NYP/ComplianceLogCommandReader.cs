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
    public class RegexIndexFieldCommand
    {
        public string MatchPattern { get; set; }
        public string ReplacePattern { get; set; }
        public string ReplaceValue { get; set; }

        public RegexIndexFieldCommand()
        { }

        public RegexIndexFieldCommand(string matchPattern, string replacePattern, string replaceValue)
        {
            this.MatchPattern = matchPattern;
            this.ReplacePattern = replacePattern;
            this.ReplaceValue = replaceValue;
        }
    }

    /// <summary>
    /// Specialized barcode command reader for NYP Compliance Logs. The documents may contain one or more index field barcodes,
    /// For each index field, there may be multiple matching barcodes
    /// but only one value can be selected, so this uses a prioritized list of regex patterns to match against.  
    /// Only the highest priority distinct value will be selected.
    /// </summary>
    public class ComplianceLogCommandReader : ISQCommandReader
    {


        private string _BarcodeReaderName = "DEFAULT";
        public virtual string BarcodeReaderName
        {
            get { return _BarcodeReaderName; }
            set { _BarcodeReaderName = value; }
        }

        private Dictionary<string, RegexIndexFieldCommand[]> _IndexFieldDefinitions = new Dictionary<string, RegexIndexFieldCommand[]>()
        {
            {JsonsFieldConstants.JsonFieldLogStation, new RegexIndexFieldCommand[] { new RegexIndexFieldCommand(@"^LGS:", @"^LGS:", string.Empty) }},
            {JsonsFieldConstants.JsonFieldLogDate, new RegexIndexFieldCommand[] { new RegexIndexFieldCommand(@"^LGD:", @"^LGD:", string.Empty) }}
        };

        /// <summary>Mapping of index field names to a list of regex patterns ordered by priority.</summary>
        public Dictionary<string, RegexIndexFieldCommand[]> IndexFieldDefinitions
        {
            get { return _IndexFieldDefinitions; }
            set { _IndexFieldDefinitions = value; }
        }

        private ISQBarcodeReader _BarcodeReader = null;
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
                    commands.AddRange(ReadIndexFields(this.IndexFieldDefinitions, barcodeValues));
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
            if(barcodeValues.Length == 0)
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
