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
    /// <summary>
    /// Specialized barcode command reader for NYP Lab Reqs. The documents may contain one or more index field barcodes,
    /// many of which can be ignored. For each index field, there may be multiple matching barcodes, 
    /// but only one value can be selected, so this uses a prioritized list of regex patterns to match against.  
    /// Only the highest priority distinct value will be selected.
    /// </summary>
    public class LabReqsCommandReader : ISQCommandReader
    {
        private string _BarcodeReaderName = "DEFAULT";
        public virtual string BarcodeReaderName
        {
            get { return _BarcodeReaderName; }
            set { _BarcodeReaderName = value; }
        }

        private Dictionary<string, string[]> _IndexFieldDefinitions = new Dictionary<string, string[]>()
        {
            {JsonsFieldConstants.JsonFieldIndexNumber, new string[] { @"^(r|R|\d)\d{8}$", @"^\d{15}$"} },
            {JsonsFieldConstants.JsonFieldRequisitionNumber, new string[] { @"^3\d{9}$",@"^6\d{7}$",@"^4\d{7}$" }}
        };

        /// <summary>Mapping of index field names to a list of regex patterns ordered by priority.</summary>
        public Dictionary<string, string[]> IndexFieldDefinitions
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

            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Reading lab req barcodes for image:{image.LatestRevision.OriginalImageFilePath} document:{documentNumber.ToString()} page:{pageNumber.ToString()}");
            return Task.Factory.StartNew<IList<ISQCommand>>(() =>
            {
                using (SQImageEditLock editLock = image.BeginEdit())
                {
                    string[] barcodeValues = BarcodeReader.ReadText(image, cToken).Result;
                    Trace.TraceInformation("Found " + barcodeValues.Length + " barcodes");
                    return ReadIndexFields(this.IndexFieldDefinitions, barcodeValues);
                }
            });
        }

        public static IList<ISQCommand> ReadIndexFields(Dictionary<string, string[]> indexFieldDefinitions, string[] barcodeValues)
        {
            List<ISQCommand> commands = new List<ISQCommand>();

            EDL.TraceLogger.TraceLoggerInstance.TraceInformation("Processing " + barcodeValues.Length + " barcodes");
            foreach (KeyValuePair<string, string[]> indexFieldDefinition in indexFieldDefinitions)
            { commands.AddRange(ReadIndexField(indexFieldDefinition, barcodeValues)); }

            return commands;
        }

        public static IList<ISQCommand> ReadIndexField(KeyValuePair<string, string[]> indexFieldDefinition, string[] barcodeValues)
        {
            List<ISQCommand> commands = new List<ISQCommand>();
            if (barcodeValues.Length > 0)
            {
                Task.Factory.StartNew(() =>
                {
                    foreach (string s in barcodeValues)
                    {
                        EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Found barcode:{s}");
                    }
                }).Wait();
            }
            else
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceWarning("No Found barcode found");
                return commands;
            }

            foreach (string pattern in indexFieldDefinition.Value)
            {
                EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Looking for patteren:{pattern} in found barcodes");
                //List of all barcode values that match the current pattern
                string[] matches = barcodeValues.Where
                    (candidate => Regex.IsMatch(candidate, pattern)).ToArray();

                //If no matches, go to the next priority
                if (matches.Length == 0)
                {
                    EDL.TraceLogger.TraceLoggerInstance.TraceWarning("No barcode matches found");
                    continue;
                }

                //Remove duplicate matches
                string[] distinctMatches = matches.Distinct(StringComparer.OrdinalIgnoreCase).ToArray();

                //If multiple distinct matches, quit looking (unable to continue)
                if ((distinctMatches.Length == 1) && (string.Compare(indexFieldDefinition.Key, BatchHelper.JsonSettingsCsnNumber, true) == 0) && distinctMatches[0].IndexOf("-") > 0)
                {
                    int indexDash = distinctMatches[0].IndexOf("-") + 2;
                    distinctMatches[0] = distinctMatches[0].Substring(indexDash);

                }
                else
                {


                    if (distinctMatches.Length > 1)
                    {
                        string csnNumbers = string.Empty;
                        if (string.Compare(indexFieldDefinition.Key, BatchHelper.JsonSettingsCsnNumber, true) == 0)
                        {



                            Task.Factory.StartNew(() =>
                            {
                                // distinctMatches[0] = distinctMatches[1];
                                foreach (string s in distinctMatches)
                                {

                                    EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Found Csn barcode:{s}");
                                    if (string.IsNullOrWhiteSpace(s))
                                        continue;
                                    if (s.Length > 10)
                                        continue;
                                    if (string.IsNullOrWhiteSpace(csnNumbers))
                                        csnNumbers = $"{s}";
                                    else
                                        csnNumbers = $"{csnNumbers}-{s}";


                                }
                                string[] orderid = csnNumbers.Split('-');
                                if (orderid[0].Length == orderid[1].Length)
                                    csnNumbers = orderid[1];
                                distinctMatches[0] = csnNumbers.Trim();
                            }).Wait();
                        }
                        else
                        {
                            EDL.TraceLogger.TraceLoggerInstance.TraceError("Found dup matches");
                            Task.Factory.StartNew(() =>
                            {
                                foreach (string s in distinctMatches)
                                {
                                    EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"Dup barcode matches:{s}");
                                }
                            }).Wait();
                            break;
                        }

                    }
                }
                //if(string.Compare(indexFieldDefinition.Key,BatchHelper.JsonSettingsCsnNumber,true)==0)
                //{

                //}
                //Otherwise, we have a single distinct match, so add it.
                if (string.Compare(indexFieldDefinition.Key, BatchHelper.JsonSettingsIndexNumber, true) == 0)
                {
                    // distinctMatches[0] = "001111400382343";
                    distinctMatches[0] = BatchHelper.RemoveClientID(distinctMatches[0]);
                }


                Trace.TraceInformation("Matched " + indexFieldDefinition.Key + " with " + distinctMatches[0]);
                commands.Add(new SQCommand_Document_IndexField(indexFieldDefinition.Key, distinctMatches[0]));
                break;
            }

            return commands;
        }
    }
}
