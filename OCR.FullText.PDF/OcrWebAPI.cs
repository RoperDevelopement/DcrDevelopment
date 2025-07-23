using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Edocs.OCR.FullText.PDF.Models;
using System.Net.Http;
using System.Text.RegularExpressions;
using HU = Edocs.HelperUtilities;
using System.Diagnostics;
using System.Net.NetworkInformation;

namespace Edocs.OCR.FullText.PDF
{
    class OcrWebAPI
    {
        private static OcrWebAPI instance = null;

        public string OCRAPIKey
        {
            get { return HU.Utilities.GetAppConfigSetting(OCRConstants.AppKeyOCRAPIKey); }
        }
        public string OCRAPIFreeKey
        {
            get { return HU.Utilities.GetAppConfigSetting(OCRConstants.AppKeyOCRAPIFreeKey); }
        }
        public string RegxSkip
        {
            get { return HU.Utilities.GetAppConfigSetting(OCRConstants.AppKeyRegxSkip); }
        }
        public Uri WebUri
        { get { return new Uri(HU.Utilities.GetAppConfigSetting(OCRConstants.AppKeyWebUri)); } }


        public string AzureBlobStorageConnectionString
        { get { return HU.Utilities.GetAppConfigSetting(OCRConstants.AppKeyAzureBlobStorageConnectionString); } }

        public string AzureBlobContanierAuditShare
        {
            get { return HU.Utilities.GetAppConfigSetting(OCRConstants.AppKeyAzureBlobContanierAuditShare); }
        }
        public string AzureBlobAccountName
        { get { return HU.Utilities.GetAppConfigSetting(OCRConstants.AppKeyAzureBlobAccountName); } }
        public string AzureBlobAccountKey
        { get { return HU.Utilities.GetAppConfigSetting(OCRConstants.AppKeyAzureBlobAccountKey); } }

        public string WorkingFolder
        { get { return HU.Utilities.GetAppConfigSetting(OCRConstants.AppKeyWorkingFolder); } }
        public string SqlConnectionStr
        { get { return HU.Utilities.GetAppConfigSetting(OCRConstants.AppKeySqlConnectionStr); } }

        public string RequisitionNumberRegx
        { get { return HU.Utilities.GetAppConfigSetting(OCRConstants.AppKeyRequisitionNumberRegx).Trim(); } }

        public string IndexNumbersChanged
        { get { return HU.Utilities.GetAppConfigSetting(OCRConstants.AppKeyIndexNumbersChanged).Trim(); } }
        public string FinIndexNumberRegx
        { get { return HU.Utilities.GetAppConfigSetting(OCRConstants.AppKeyFinIndexNumberRegx).Trim(); } }

        public int ImageDpi
        { get { return HU.Utilities.ParseInt(HU.Utilities.GetAppConfigSetting(OCRConstants.AppKeyImageDpi).Trim()); } }
        public int CleanWF
        { get { return HU.Utilities.ParseInt(HU.Utilities.GetAppConfigSetting(OCRConstants.AppKeyCleanWF).Trim()); } }
        public int MaxOcrErrors
        { get { return HU.Utilities.ParseInt(HU.Utilities.GetAppConfigSetting(OCRConstants.AppKeyMaxOcrErrors).Trim()); } }

        public bool UseWebApi
        { get; set; }

        public string RunProcess
        {
            get
            {
                return HU.Utilities.GetAppConfigSetting(OCRConstants.AppKeyRunProcess).Trim();
            }
        }
        public string ProcessWorkingFolder
        {
            get
            {
                return HU.Utilities.GetAppConfigSetting(OCRConstants.AppKeyProcessWorkingFolder).Trim();
            }
        }

        public string ProcessArgs
        {
            get
            {
                return HU.Utilities.GetAppConfigSetting(OCRConstants.AppKeyProcessArgs).Trim();
            }
        }


        public int SendTextOnError
        {
            get
            {
                return int.Parse(HU.Utilities.GetAppConfigSetting(OCRConstants.AppKeySendTextOnError).Trim());
            }
        }


        public string RunDate
        {
            get
            {
                return HU.Utilities.GetAppConfigSetting(OCRConstants.AppKeyRunDate).Trim();
            }
        }


        public string AzureImageContanier
        {
            get
            {
                return HU.Utilities.GetAppConfigSetting(OCRConstants.AppKeyAzureImageContanier).Trim();
            }
        }
        public string LogFile
        {
            get
            {
                return HU.Utilities.GetAppConfigSetting(OCRConstants.AppKeyLogFile).Trim();
            }
        }
        public string HtmlFile
        {
            get
            {

                string htmlf = HU.Utilities.GetAppConfigSetting(OCRConstants.AppKeyHtmlFile).Trim();
                htmlf = HU.Utilities.ReplaceString(htmlf, OCRConstants.RepStrApplicationDir, HU.Utilities.GetApplicationDir());
                htmlf = $"{HU.Utilities.CheckFolderPath(htmlf)}HtmlChanges_{DateTime.Now.ToString("MM_dd_yyyy_HH_mm_ss")}.html";
                return htmlf = HU.Utilities.ReplaceString(htmlf, OCRConstants.RepStrApplicationDir, HU.Utilities.GetApplicationDir());
            }
        }
        public string HTMLtemplate
        {
            get
            {
                string htmlT = HU.Utilities.CheckFolderPath(HU.Utilities.GetApplicationDir());
                return $"{htmlT}{HU.Utilities.GetAppConfigSetting(OCRConstants.AppKeyHTMLtemplate).Trim()}";
            }
        }

        public static string LogFolder
        {
            get
            {

                string folderLog;
                folderLog = HU.Utilities.GetAppConfigSetting(OCRConstants.AppKeyLogFile);
                folderLog = HU.Utilities.ReplaceString(folderLog, OCRConstants.RepStrApplicationDir, HU.Utilities.GetApplicationDir());
                folderLog = $"{HU.Utilities.CheckFolderPath(folderLog)}OCRPDF_{DateTime.Now.ToString("MM_dd_yyyy_HH_mm_ss")}.log";
                return folderLog;
            }
        }

        public void CLeanUpFolder(string folder, int numdays)
        {
            try
            {
                HU.Utilities.DeleteFiles(folder, numdays);
            }
            catch { }
        }
        public static OcrWebAPI OCRApisInctance
        {
            get
            {
                if (instance == null)
                    instance = new OcrWebAPI();
                return instance;
            }
        }
        private OcrWebAPI()
        {
        }
        //public async Task<Match> RegxMatch(string regxStr,string regMatchStr)
        //{


        //    return Regex.Match(regxStr, regMatchStr, RegexOptions.IgnoreCase);
        //}
        //public async Task<Match> RegxMatch(string regxStr)
        //{

        //    return Regex.Match(regxStr, RequisitionNumberRegx, RegexOptions.IgnoreCase);
        //}
        public async Task<MatchCollection> RegxMatchCollection(string regxStr)
        {
            // regxStr = regxStr.Replace("/r/n", " ");
            //   regxStr = regxStr.Replace("/t", "");
            //  regxStr = regxStr.Replace("/r", " ");
            //  regxStr = regxStr.Replace("/n", " ");
            Regex regex = new Regex(@"\d+");
            // Regex regex = new Regex(@"^63\d{6}$");
            //    Regex regex = new Regex(RequisitionNumberRegx);
            MatchCollection match = regex.Matches(regxStr.Trim());
            //MatchCollection match = Regex.Matches(regxStr, RequisitionNumberRegx, RegexOptions.IgnoreCase);
            if (match.Count > 0)

                foreach (Match m in match)
                {
                    Console.WriteLine(m);
                }
            return match;
        }
        public async Task<IList<string>> RegxMatchCollectionList(string regxStr)
        {
            IList<string> lMathes = new List<string>();
            Regex regex = new Regex(@"\d+");

            MatchCollection match = regex.Matches(regxStr.Trim());

            if (match.Count > 0)

                foreach (Match foundMatch in match)
                {
                    if (foundMatch.Value.Length >= 6)
                        lMathes.Add(foundMatch.Value);
                }
            return lMathes;
        }
        public async Task<Match> RegxMatch(string regxStr, string rexPattern)
        {
            return Regex.Match(regxStr, RequisitionNumberRegx, RegexOptions.IgnoreCase);
        }

        public async Task<IDictionary<int, LabReqsModel>> GetLabReqs(string scanSTDate, string scanEndDate, Uri webUri, string controllerName)
        {
            IDictionary<int, LabReqsModel> valuePairs = new Dictionary<int, LabReqsModel>();
            try
            {

                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromMinutes(10);
                    client.BaseAddress = webUri;
                    scanSTDate = scanSTDate.Replace("/", "-");
                    scanEndDate = scanEndDate.Replace("/", "-");
                    // DateTime dts = DateTime.Parse(scanSTDate);
                    // DateTime dte = DateTime.Parse(scanEndDate);

                    var responseTask = client.GetAsync($"{controllerName}{scanSTDate}/{scanEndDate}");
                    responseTask.Wait();
                    var results = responseTask.Result;

                    if (results.IsSuccessStatusCode)
                    {
                        //    var s = results.Content.ReadAsStringAsync();
                        //  s.Wait();
                        var readTask = results.Content.ReadAsAsync<LabReqsModel[]>();
                        readTask.Wait();
                        if (readTask.Result.Length > 0)
                            valuePairs = readTask.Result.ToDictionary(p => p.LabReqID);



                    }
                    else
                    {

                        throw new Exception($"Getting lab recsd for webAPiUri {webUri} controller {controllerName} scanstartdate {scanSTDate} scanenddate {scanEndDate} IsSuccessStatusCode {results.StatusCode.ToString()}");

                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Getting lab recsd for webAPiUri {webUri} controller {controllerName} scanstartdate {scanSTDate} scanenddate {scanEndDate}");

            }
            return valuePairs;
        }

        public async Task UpLoadPDFFullTextGetLabReqs(PDFFullTextModel pDFFullText, Uri webUri, string controllerName)
        {

            try
            {

                using (var client = new HttpClient())
                {
                    var ser = JsonConvert.SerializeObject(pDFFullText);
                    var content = new StringContent(ser, Encoding.UTF8, "application/json");
                    client.Timeout = TimeSpan.FromMinutes(10);
                    client.BaseAddress = webUri;

                    // DateTime dts = DateTime.Parse(scanSTDate);
                    // DateTime dte = DateTime.Parse(scanEndDate);

                    var responseTask = client.PostAsync($"{controllerName}", content);
                    responseTask.Wait();
                    var results = responseTask.Result;

                    if (!(results.IsSuccessStatusCode))
                    {
                        //    var s = results.Content.ReadAsStringAsync();
                        //  s.Wait();
                        throw new Exception($"Posting lab recs IsSuccessStatusCode {results.StatusCode.ToString()}");

                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Posting lab recs for webAPiUri {webUri} controller {controllerName} lab req id {pDFFullText.PDFFullText}");

            }

        }

        public async Task UpDateNypLabReqNumber(int labReqID, string reqNumber, Uri webUri, string controllerName)
        {

            try
            {

                using (var client = new HttpClient())
                {
                    //  var ser = JsonConvert.SerializeObject(pDFFullText);
                    // var content = new StringContent(ser, Encoding.UTF8, "application/json");
                    client.Timeout = TimeSpan.FromMinutes(10);
                    client.BaseAddress = webUri;

                    // DateTime dts = DateTime.Parse(scanSTDate);
                    // DateTime dte = DateTime.Parse(scanEndDate);

                    var responseTask = client.PutAsync($"{controllerName}{labReqID}/{reqNumber}", null);
                    responseTask.Wait();
                    var results = responseTask.Result;

                    if (!(results.IsSuccessStatusCode))
                    {
                        //    var s = results.Content.ReadAsStringAsync();
                        //  s.Wait();
                        throw new Exception($"Posting lab recs IsSuccessStatusCode {results.StatusCode.ToString()}");

                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Posting lab recs for webAPiUri {webUri} controller {controllerName} lab req id {labReqID} req number {reqNumber}");

            }

        }
    }
}
