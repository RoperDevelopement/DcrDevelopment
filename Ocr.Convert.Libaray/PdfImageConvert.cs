//used code for ocr google from there github
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.IO;
using System.Drawing;
using DebenuPDFLibraryDLL0915;
using Tesseract;
using Newtonsoft.Json;
using OCRAPIModel;
//using Aspose.OCR;
using TL = EdocsUSA.Utilities.Logging.TraceLogger;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.AxHost;

namespace Edocs.Ocr.Convert.Libaray
{
    public static class PdfImageConvert
    {
        static readonly string tessPath = (@"C:\Program Files (x86)\Tesseract-OCR\tessdata");
        //private static string getSelectedLanguage(string strLang)
        //{

        //    //https://ocr.space/OCRAPI#PostParameters

        //    //Czech = cze; Danish = dan; Dutch = dut; English = eng; Finnish = fin; French = fre; 
        //    //German = ger; Hungarian = hun; Italian = ita; Norwegian = nor; Polish = pol; Portuguese = por;
        //    //Spanish = spa; Swedish = swe; ChineseSimplified = chs; Greek = gre; Japanese = jpn; Russian = rus;
        //    //Turkish = tur; ChineseTraditional = cht; Korean = kor


        //    switch (strLang)
        //    {
        //        case 0:
        //            strLang = "chs";
        //            break;

        //        case 1:
        //            strLang = "cht";
        //            break;
        //        case 2:
        //            strLang = "cze";
        //            break;
        //        case 3:
        //            strLang = "dan";
        //            break;
        //        case 4:
        //            strLang = "dut";
        //            break;
        //        case 5:
        //            strLang = "eng";
        //            break;
        //        case 6:
        //            strLang = "fin";
        //            break;
        //        case 7:
        //            strLang = "fre";
        //            break;
        //        case 8:
        //            strLang = "ger";
        //            break;
        //        case 9:
        //            strLang = "gre";
        //            break;
        //        case 10:
        //            strLang = "hun";
        //            break;
        //        case 11:
        //            strLang = "jap";
        //            break;
        //        case 12:
        //            strLang = "kor";
        //            break;
        //        case 13:
        //            strLang = "nor";
        //            break;
        //        case 14:
        //            strLang = "pol";
        //            break;
        //        case 15:
        //            strLang = "por";
        //            break;
        //        case 16:
        //            strLang = "spa";
        //            break;
        //        case 17:
        //            strLang = "swe";
        //            break;
        //        case 18:
        //            strLang = "tur";
        //            break;

        //    }
        //    return strLang;

        //}
        public static async Task<string> OCRASPOSE(string imagePDFPath, bool imagePdf, bool jsonResults, string apiKey)
        {
            //AsposeOcr api = new AsposeOcr();
            //RecognitionResult recognition = api.RecognizeImage(imagePDFPath, new RecognitionSettings { ThresholdValue = 160 });
            //string retSet = recognition.RecognitionText;
            //return retSet;
            return string.Empty;
        }
        public static async Task<string> OCRSrace(string imagePDFPath, bool imagePdf, bool jsonResults, string apiKey)
        {
            string resultTextString = string.Empty;
            string resultJsonString = string.Empty;
            try
            {

                if (!(File.Exists(imagePDFPath)))
                {
                    throw new Exception($"OCR File not found {imagePDFPath}");
                }
                HttpClient httpClient = new HttpClient();
                httpClient.Timeout = new TimeSpan(1, 1, 1);
                //Removed the api key from headers
                //httpClient.DefaultRequestHeaders.TryAddWithoutValidation("apikey", "5a64d478-9c89-43d8-88e3-c65de9999580");

                MultipartFormDataContent form = new MultipartFormDataContent();
                //5a64d478-9c89-43d8-88e3-c65de9999580
                form.Add(new StringContent(apiKey), "apikey"); //The "helloworld" key works, but it has a very low rate limit. You can get your won free api key at https://ocr.space/OCRAPI
                form.Add(new StringContent("eng"), "language");
                //   form.Add(new StringContent("2"), "OCREngine");
                form.Add(new StringContent("2"), "OCREngine");
                form.Add(new StringContent("false"), "isTable");
                //  form.Add(new StringContent("detectOrientation"), "true");
                //isSearchablePdfHideTextLayer
                //  form.Add(new StringContent("TextOverlay"),"true");




                if (imagePdf)
                {
                    TL.TraceLoggerInstance.TraceInformationConsole($"Getting text from image {imagePDFPath}");
                    System.Drawing.Image image = System.Drawing.Image.FromFile(imagePDFPath);
                    byte[] imageData = File.ReadAllBytes(imagePDFPath);
                    //  byte[] imageData = ImageToBase64(image,System.Drawing.Imaging.ImageFormat.Tiff);
                    //  form.Add(new StringContent("filetype"), "JPG");
                    form.Add(new ByteArrayContent(imageData, 0, imageData.Length), "image", "image.tif");
                }
                else
                {
                    byte[] imageData = File.ReadAllBytes(imagePDFPath);
                    TL.TraceLoggerInstance.TraceInformationConsole($"Getting text from pdf file {imagePDFPath}");
                    // form.Add(new StringContent("filetype"), "PDF");
                    form.Add(new ByteArrayContent(imageData, 0, imageData.Length), "PDF", "pdf.pdf");
                }

                //   HttpResponseMessage response = await httpClient.PostAsync("https://api.ocr.space/Parse/Image", form);
                HttpResponseMessage response = await httpClient.PostAsync("https://apipro1.ocr.space/parse/image", form);

                resultJsonString = await response.Content.ReadAsStringAsync();



                Rootobject ocrResult = JsonConvert.DeserializeObject<Rootobject>(resultJsonString);
                TL.TraceLoggerInstance.TraceInformationConsole($"Ocr results exit code {ocrResult.OCRExitCode}");

                if (ocrResult.OCRExitCode == 1)
                {
                    TL.TraceLoggerInstance.TraceInformationConsole($"Getting ocr results");
                    for (int i = 0; i < ocrResult.ParsedResults.Count(); i++)
                    {
                        resultTextString += ocrResult.ParsedResults[i].ParsedText;
                    }


                    if (jsonResults)
                        resultTextString = resultJsonString.Replace("\\r", "").Replace("\\n", "");

                }
                else
                {
                    throw new Exception("ERROR: " + resultJsonString + ocrResult.ErrorMessage + ocrResult.ErrorDetails);
                }




            }
            catch (Exception exception)
            {
                TL.TraceLoggerInstance.TraceError($"Ocr results exit code {resultJsonString} {exception.Message} for file {imagePDFPath}");
                throw new Exception($"GoogleOcr return string {resultJsonString} {exception.Message}");
            }
            return resultTextString;
        }
        public static async Task<string> OCRSrace(string imagePDFPath, bool imagePdf, bool jsonResults, string apiKey, bool createSearchablePdf = false)
        {
            string resultTextString = string.Empty;
            string resultJsonString = string.Empty;
            try
            {

                if (!(File.Exists(imagePDFPath)))
                {
                    throw new Exception($"OCR File not found {imagePDFPath}");
                }
                HttpClient httpClient = new HttpClient();
                httpClient.Timeout = new TimeSpan(1, 1, 1);
                //Removed the api key from headers
                //httpClient.DefaultRequestHeaders.TryAddWithoutValidation("apikey", "5a64d478-9c89-43d8-88e3-c65de9999580");

                MultipartFormDataContent form = new MultipartFormDataContent();
                //5a64d478-9c89-43d8-88e3-c65de9999580
                form.Add(new StringContent(apiKey), "apikey"); //The "helloworld" key works, but it has a very low rate limit. You can get your won free api key at https://ocr.space/OCRAPI
                form.Add(new StringContent("eng"), "language");
                //   form.Add(new StringContent("2"), "OCREngine");
                form.Add(new StringContent("2"), "OCREngine");
                form.Add(new StringContent("false"), "isTable");
                form.Add(new StringContent(createSearchablePdf.ToString()), "isCreateSearchablePdf");
                //isSearchablePdfHideTextLayer
                //form.Add(new StringContent("isOverlayRequired"), "true");
                 form.Add(new StringContent("true"),"isSearchablePdfHideTextLayer");




                if (imagePdf)
                {
                    TL.TraceLoggerInstance.TraceInformationConsole($"Getting text from image {imagePDFPath}");
                    System.Drawing.Image image = System.Drawing.Image.FromFile(imagePDFPath);
                    byte[] imageData = File.ReadAllBytes(imagePDFPath);
                    //  byte[] imageData = ImageToBase64(image,System.Drawing.Imaging.ImageFormat.Tiff);
                    //  form.Add(new StringContent("filetype"), "JPG");
                    form.Add(new ByteArrayContent(imageData, 0, imageData.Length), "image", "image.tif");
                }
                else
                {
                    byte[] imageData = File.ReadAllBytes(imagePDFPath);
                    TL.TraceLoggerInstance.TraceInformationConsole($"Getting text from pdf file {imagePDFPath}");
                    // form.Add(new StringContent("filetype"), "PDF");
                    form.Add(new ByteArrayContent(imageData, 0, imageData.Length), "PDF", "pdf.pdf");
                }

                //   HttpResponseMessage response = await httpClient.PostAsync("https://api.ocr.space/Parse/Image", form);
                HttpResponseMessage response = await httpClient.PostAsync("https://apipro1.ocr.space/parse/image", form);

                resultJsonString = await response.Content.ReadAsStringAsync();



                RootobjectPDF ocrResult = JsonConvert.DeserializeObject<RootobjectPDF>(resultJsonString);
                TL.TraceLoggerInstance.TraceInformationConsole($"Ocr results exit code {ocrResult.OCRExitCode}");

                if (ocrResult.OCRExitCode == 1)
                {
                    return ocrResult.SearchablePDFURL;
                    //TL.TraceLoggerInstance.TraceInformationConsole($"Getting ocr results");
                    //for (int i = 0; i < ocrResult.ParsedResults.Count(); i++)
                    //{
                    //    resultTextString += ocrResult.ParsedResults[i].ParsedText;
                    //}


                    //if (jsonResults)
                    //    resultTextString = resultJsonString.Replace("\\r", "").Replace("\\n", "");

                }
                else
                {
                    throw new Exception("ERROR: " + resultJsonString + ocrResult.ErrorMessage + ocrResult.ErrorDetails);
                }




            }
            catch (Exception exception)
            {
                TL.TraceLoggerInstance.TraceError($"Ocr results exit code {resultJsonString} {exception.Message} for file {imagePDFPath}");
                throw new Exception($"GoogleOcr return string {resultJsonString} {exception.Message}");
            }
            return resultTextString;
        }

        //https://ocr.space/OCRAPI
        public static async Task<string> OCRSraceCreateSearchablePdf(string imagePDFPath,string apiKey)
        {
            string resultTextString = string.Empty;
            string resultJsonString = string.Empty;
            try
            {

                if (!(File.Exists(imagePDFPath)))
                {
                    throw new Exception($"OCR File not found {imagePDFPath}");
                }
                HttpClient httpClient = new HttpClient();
                httpClient.Timeout = new TimeSpan(1, 1, 1);
                //Removed the api key from headers
                //httpClient.DefaultRequestHeaders.TryAddWithoutValidation("apikey", "5a64d478-9c89-43d8-88e3-c65de9999580");

                MultipartFormDataContent form = new MultipartFormDataContent();
                
                form.Add(new StringContent(apiKey), "apikey"); //The "helloworld" key works, but it has a very low rate limit. You can get your won free api key at https://ocr.space/OCRAPI
                form.Add(new StringContent("eng"), "language");
            //    form.Add(new StringContent("true"), "scale");
                
                form.Add(new StringContent("2"), "OCREngine");
                form.Add(new StringContent("true"), "isTable");
                form.Add(new StringContent("true"), "isCreateSearchablePdf");
                form.Add(new StringContent("false"), "isOverlayRequired");
                form.Add(new StringContent("true"), "isSearchablePdfHideTextLayer");





                byte[] imageData = File.ReadAllBytes(imagePDFPath);
                    TL.TraceLoggerInstance.TraceInformationConsole($"Getting text from pdf file {imagePDFPath}");
                    // form.Add(new StringContent("filetype"), "PDF");
                    form.Add(new ByteArrayContent(imageData, 0, imageData.Length), "PDF", "pdf.pdf");
                 

                //   HttpResponseMessage response = await httpClient.PostAsync("https://api.ocr.space/Parse/Image", form);
                HttpResponseMessage response = await httpClient.PostAsync("https://apipro1.ocr.space/parse/image", form);

                resultJsonString = await response.Content.ReadAsStringAsync();



                RootobjectPDF ocrResult = JsonConvert.DeserializeObject<RootobjectPDF>(resultJsonString);
                TL.TraceLoggerInstance.TraceInformationConsole($"Ocr results exit code {ocrResult.OCRExitCode}");

                if (ocrResult.OCRExitCode == 1)
                {
                    resultTextString= ocrResult.SearchablePDFURL;
                    //TL.TraceLoggerInstance.TraceInformationConsole($"Getting ocr results");
                    //for (int i = 0; i < ocrResult.ParsedResults.Count(); i++)
                    //{
                    //    resultTextString += ocrResult.ParsedResults[i].ParsedText;
                    //}


                    //if (jsonResults)
                    //    resultTextString = resultJsonString.Replace("\\r", "").Replace("\\n", "");

                }
                else
                {
                    throw new Exception("ERROR: " + resultJsonString + ocrResult.ErrorMessage + ocrResult.ErrorDetails);
                }




            }
            catch (Exception exception)
            {
                TL.TraceLoggerInstance.TraceError($"Ocr results exit code {resultJsonString} {exception.Message} for file {imagePDFPath}");
                throw new Exception($"GoogleOcr return string {resultJsonString} {exception.Message}");
            }
            return resultTextString;
        }
        public static async Task<string> OCRSrace(string imagePDFPath, bool imagePdf, bool jsonResults, string apiKey, string saveImagename)
        {
            string resultTextString = string.Empty;
            string resultJsonString = string.Empty;
            try
            {

                if (!(File.Exists(imagePDFPath)))
                {
                    throw new Exception($"OCR File not found {imagePDFPath}");
                }
                HttpClient httpClient = new HttpClient();
                httpClient.Timeout = new TimeSpan(1, 1, 1);
                //Removed the api key from headers
                //httpClient.DefaultRequestHeaders.TryAddWithoutValidation("apikey", "5a64d478-9c89-43d8-88e3-c65de9999580");

                MultipartFormDataContent form = new MultipartFormDataContent();
                //5a64d478-9c89-43d8-88e3-c65de9999580
                form.Add(new StringContent(apiKey), "apikey"); //The "helloworld" key works, but it has a very low rate limit. You can get your won free api key at https://ocr.space/OCRAPI
                form.Add(new StringContent("eng"), "language");
                if (!(string.IsNullOrWhiteSpace(saveImagename)))
                    form.Add(new StringContent("2"), "OCREngine");
                else
                {
                    form.Add(new StringContent("1"), "OCREngine");
                }
                form.Add(new StringContent("false"), "isTable");
                //  form.Add(new StringContent("detectOrientation"), "true");
                //isSearchablePdfHideTextLayer
                //  form.Add(new StringContent("TextOverlay"),"true");




                if (imagePdf)
                {
                    TL.TraceLoggerInstance.TraceInformationConsole($"Getting text from image {imagePDFPath}");
                    System.Drawing.Image image = System.Drawing.Image.FromFile(imagePDFPath);
                    byte[] imageData = File.ReadAllBytes(imagePDFPath);
                    //  byte[] imageData = ImageToBase64(image,System.Drawing.Imaging.ImageFormat.Tiff);
                    //  form.Add(new StringContent("filetype"), "JPG");
                    form.Add(new ByteArrayContent(imageData, 0, imageData.Length), "image", saveImagename);
                }
                else
                {
                    byte[] imageData = File.ReadAllBytes(imagePDFPath);
                    TL.TraceLoggerInstance.TraceInformationConsole($"Getting text from pdf file {imagePDFPath}");
                    // form.Add(new StringContent("filetype"), "PDF");
                    form.Add(new ByteArrayContent(imageData, 0, imageData.Length), "PDF", "pdf.pdf");
                }

                //   HttpResponseMessage response = await httpClient.PostAsync("https://api.ocr.space/Parse/Image", form);
                HttpResponseMessage response = await httpClient.PostAsync("https://apipro1.ocr.space/parse/image", form);

                resultJsonString = await response.Content.ReadAsStringAsync();


                form.Dispose();
                Rootobject ocrResult = JsonConvert.DeserializeObject<Rootobject>(resultJsonString);
                TL.TraceLoggerInstance.TraceInformationConsole($"Ocr results exit code {ocrResult.OCRExitCode}");

                if (ocrResult.OCRExitCode == 1)
                {
                    TL.TraceLoggerInstance.TraceInformationConsole($"Getting ocr results");
                    for (int i = 0; i < ocrResult.ParsedResults.Count(); i++)
                    {
                        resultTextString += ocrResult.ParsedResults[i].ParsedText;
                    }


                    if (jsonResults)
                        resultTextString = resultJsonString.Replace("\\r", "").Replace("\\n", "");

                }
                else
                {
                    throw new Exception("ERROR: " + resultJsonString + ocrResult.ErrorMessage + ocrResult.ErrorDetails);
                }




            }
            catch (Exception exception)
            {
                TL.TraceLoggerInstance.TraceError($"Ocr results exit code {resultJsonString} {exception.Message} for file {imagePDFPath}");
                throw new Exception($"GoogleOcr return string {resultJsonString} {exception.Message}");
            }
            return resultTextString;
        }
        public static async Task<string> OCRSrace(string imagePDFPath, byte[] imgeDataByte, bool imagePdf, bool jsonResults, string apiKey, string ocrEng, bool isTable, string saveImageType)
        {
            string resultTextString = string.Empty;
            string resultJsonString = string.Empty;
            try
            {

                if (!(File.Exists(imagePDFPath)))
                {
                    throw new Exception($"OCR File not found {imagePDFPath}");
                }
                HttpClient httpClient = new HttpClient();
                httpClient.Timeout = new TimeSpan(1, 1, 1);
                //Removed the api key from headers
                //httpClient.DefaultRequestHeaders.TryAddWithoutValidation("apikey", "5a64d478-9c89-43d8-88e3-c65de9999580");

                MultipartFormDataContent form = new MultipartFormDataContent();
                //5a64d478-9c89-43d8-88e3-c65de9999580
                form.Add(new StringContent(apiKey), "apikey"); //The "helloworld" key works, but it has a very low rate limit. You can get your won free api   https://ocr.space/OCRAPI
                form.Add(new StringContent("eng"), "language");
                 
                    if (string.Compare(ocrEng, "2", true) == 0)
                        form.Add(new StringContent("2"), "OCREngine");
                    else
                    {
                        form.Add(new StringContent("1"), "OCREngine");
                    }
                
                
                    form.Add(new StringContent(isTable.ToString()), "isTable");
                    
                //  form.Add(new StringContent("detectOrientation"), "true");
                //isSearchablePdfHideTextLayer
                //  form.Add(new StringContent("TextOverlay"),"true");




                if (imagePdf)
                {
                    TL.TraceLoggerInstance.TraceInformationConsole($"Getting text from image {imagePDFPath}");
                    if (!(string.IsNullOrWhiteSpace(imagePDFPath)))
                    {
                        System.Drawing.Image image = System.Drawing.Image.FromFile(imagePDFPath);
                        byte[] imageData = File.ReadAllBytes(imagePDFPath);
                        //  byte[] imageData = ImageToBase64(image,System.Drawing.Imaging.ImageFormat.Tiff);
                        //  form.Add(new StringContent("filetype"), "JPG");
                        form.Add(new ByteArrayContent(imageData, 0, imageData.Length), "image", saveImageType);
                    }
                    else
                        form.Add(new ByteArrayContent(imgeDataByte, 0, imgeDataByte.Length), "image", saveImageType);
                }
                else
                {
                    if (!(string.IsNullOrWhiteSpace(imagePDFPath)))
                    {
                        byte[] imageData = File.ReadAllBytes(imagePDFPath);
                        TL.TraceLoggerInstance.TraceInformationConsole($"Getting text from pdf file {imagePDFPath}");
                        // form.Add(new StringContent("filetype"), "PDF");
                        form.Add(new ByteArrayContent(imageData, 0, imageData.Length), "PDF", saveImageType);
                    }
                    else
                        form.Add(new ByteArrayContent(imgeDataByte, 0, imgeDataByte.Length), "PDF", saveImageType);
                }

                //   HttpResponseMessage response = await httpClient.PostAsync("https://api.ocr.space/Parse/Image", form);
                HttpResponseMessage response = await httpClient.PostAsync("https://apipro1.ocr.space/parse/image", form);

                resultJsonString = await response.Content.ReadAsStringAsync();


                form.Dispose();
                Rootobject ocrResult = JsonConvert.DeserializeObject<Rootobject>(resultJsonString);
                TL.TraceLoggerInstance.TraceInformationConsole($"Ocr results exit code {ocrResult.OCRExitCode}");

                if (ocrResult.OCRExitCode == 1)
                {
                     
                    TL.TraceLoggerInstance.TraceInformationConsole($"Getting ocr results");
                    for (int i = 0; i < ocrResult.ParsedResults.Count(); i++)
                    {
                        resultTextString += ocrResult.ParsedResults[i].ParsedText;
                    }


                    if (jsonResults)
                        resultTextString = resultJsonString.Replace("\\r", "").Replace("\\n", "");
                   
                 

                }
                else
                {
                    throw new Exception("ERROR: " + resultJsonString + ocrResult.ErrorMessage + ocrResult.ErrorDetails);
                }




            }
            catch (Exception exception)
            {
                TL.TraceLoggerInstance.TraceError($"Ocr results exit code {resultJsonString} {exception.Message} for file {imagePDFPath}");
                throw new Exception($"GoogleOcr return string {resultJsonString} {exception.Message}");
            }
            return resultTextString;
        }
        public static Bitmap ResizeBitmap(Bitmap bmp, int width, int height)
        {
            Bitmap result = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.DrawImage(bmp, 0, 0, width, height);
            }

            return result;
        }
        public static async Task<string> OCRSrace(byte[] imageBytes, bool imagePdf, bool jsonResults, string apiKey)
        {
            string resultTextString = string.Empty;
            string resultJsonString = string.Empty;
            try
            {

                if (imageBytes.Length == 0)
                {
                    throw new Exception($"No bytes for image data");
                }
                HttpClient httpClient = new HttpClient();
                httpClient.Timeout = new TimeSpan(0, 5, 0);
                //Removed the api key from headers
                //httpClient.DefaultRequestHeaders.TryAddWithoutValidation("apikey", "5a64d478-9c89-43d8-88e3-c65de9999580");

                MultipartFormDataContent form = new MultipartFormDataContent();
                //5a64d478-9c89-43d8-88e3-c65de9999580
                form.Add(new StringContent(apiKey), "apikey"); //The "helloworld" key works, but it has a very low rate limit. You can get your won free api key at https://ocr.space/OCRAPI
                form.Add(new StringContent("eng"), "language");
                // form.Add(new StringContent("2"), "OCREngine");
                form.Add(new StringContent("1"), "OCREngine");
                form.Add(new StringContent("true"), "isTable");
                //  form.Add(new StringContent("detectOrientation"), "true");
                //isSearchablePdfHideTextLayer
                //  form.Add(new StringContent("TextOverlay"),"true");




                if (imagePdf)
                {
                    //  TL.TraceLoggerInstance.TraceInformationConsole($"Getting text from image {imagePDFPath}");
                    //   System.Drawing.Image image1 = System.Drawing.Image.FromStream(new MemoryStream(imageBytes));
                    //  byte[] imageData = File.ReadAllBytes(imagePDFPath);
                    //  byte[] imageData = ImageToBase64(image,System.Drawing.Imaging.ImageFormat.Tiff);
                    //  form.Add(new StringContent("filetype"), "JPG");
                    form.Add(new ByteArrayContent(imageBytes, 0, imageBytes.Length), "image", "image.png");
                }
                else
                {
                    //  byte[] imageData = File.ReadAllBytes(imagePDFPath);
                    // TL.TraceLoggerInstance.TraceInformationConsole($"Getting text from pdf file {imagePDFPath}");
                    // form.Add(new StringContent("filetype"), "PDF");
                    form.Add(new ByteArrayContent(imageBytes, 0, imageBytes.Length), "PDF", "pdf.pdf");
                }

                //   HttpResponseMessage response = await httpClient.PostAsync("https://api.ocr.space/Parse/Image", form);
                HttpResponseMessage response = await httpClient.PostAsync("https://apipro1.ocr.space/parse/image", form);

                resultJsonString = await response.Content.ReadAsStringAsync();



                Rootobject ocrResult = JsonConvert.DeserializeObject<Rootobject>(resultJsonString);
                TL.TraceLoggerInstance.TraceInformationConsole($"Ocr results exit code {ocrResult.OCRExitCode}");

                if (ocrResult.OCRExitCode == 1)
                {
                    TL.TraceLoggerInstance.TraceInformationConsole($"Getting ocr results");
                    for (int i = 0; i < ocrResult.ParsedResults.Count(); i++)
                    {
                        resultTextString += ocrResult.ParsedResults[i].ParsedText;
                    }


                    if (jsonResults)
                        resultTextString = resultJsonString.Replace("\\r", "").Replace("\\n", "");

                }
                else
                {
                    throw new Exception("ERROR: " + resultJsonString + ocrResult.ErrorMessage + ocrResult.ErrorDetails);
                }




            }
            catch (Exception exception)
            {
                //    TL.TraceLoggerInstance.TraceError($"Ocr results exit code {resultJsonString} {exception.Message} for file {imagePDFPath}");
                throw new Exception($"GoogleOcr return string {resultJsonString} {exception.Message}");
            }
            return resultTextString;
        }
        public static IEnumerable<byte[]> GetPdfImages(string pdfFile, string pdfPassWord)
        {
            PDFLibrary pdf = new PDFLibrary();
            pdf.SetOrigin(0);
            byte[] b = File.ReadAllBytes(pdfFile);
            pdf.LoadFromString(b, pdfPassWord);
            int pageCount = pdf.PageCount();
            for (int currentPageIndex = 1; currentPageIndex <= pageCount; currentPageIndex++)
            {
                pdf.SelectPage(currentPageIndex);
                int imageListID = pdf.GetPageImageList(0);
                int imageCount = pdf.GetImageListCount(imageListID);
                if (imageCount > 1)
                    throw new Exception($"More then one image count {imageCount} for pdffile {pdfFile}");

                int imageIndex = 1;
                byte[] imageData = pdf.GetImageListItemDataToString(imageListID, imageIndex, 0);

                Bitmap bitmap = new Bitmap(new MemoryStream(imageData));

                string fileName = @"D:\PDFIMage\test.png";
                bitmap.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);
                int h = bitmap.Height;
                int w = bitmap.Width;
                if ((h > 10000) && (w > 10000))
                    bitmap = ResizeBitmap(bitmap, 10000, 10000);
                if (h > 10000)
                    bitmap = ResizeBitmap(bitmap, bitmap.Width, 10000);
                if (w > 10000)
                    bitmap = ResizeBitmap(bitmap, 10000, bitmap.Height);
                using (MemoryStream ms = new MemoryStream())
                {
                    // Convert Image to byte[]
                    bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] imageBytes = ms.ToArray();
                    ms.FlushAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                    ms.Close();
                    yield return imageBytes;
                }

                //   

                //bitmap.Save($"D:\\PDFIMage\\{GetNewGuid()}.png", System.Drawing.Imaging.ImageFormat.Png);

            }
            pdf.ReleaseLibrary();

            // int imageCound = pdf.GetImagePageCount(pdfFile)
        }
        public static IEnumerable<byte[]> GetPdfImages(string pdfFile, string pdfPassWord, string saveFileName, System.Drawing.Imaging.ImageFormat imageFormat)
        {
            PDFLibrary pdf = new PDFLibrary();
            pdf.SetOrigin(0);
            byte[] b = File.ReadAllBytes(pdfFile);
            pdf.LoadFromString(b, pdfPassWord);
            int pageCount = pdf.PageCount();



            for (int currentPageIndex = 1; currentPageIndex <= pageCount; currentPageIndex++)
            {
                pdf.SelectPage(currentPageIndex);
                int imageListID = pdf.GetPageImageList(0);
                int imageCount = pdf.GetImageListCount(imageListID);
                if (imageCount > 1)
                    throw new Exception($"More then one image count {imageCount} for pdffile {pdfFile}");

                int imageIndex = 1;
                byte[] imageData = pdf.GetImageListItemDataToString(imageListID, imageIndex, 0);

                Bitmap bitmap = new Bitmap(new MemoryStream(imageData));


                bitmap.Save(saveFileName, System.Drawing.Imaging.ImageFormat.Png);
                int h = bitmap.Height;
                int w = bitmap.Width;
                if ((h > 10000) && (w > 10000))
                    bitmap = ResizeBitmap(bitmap, 10000, 10000);
                if (h > 10000)
                    bitmap = ResizeBitmap(bitmap, bitmap.Width, 10000);
                if (w > 10000)
                    bitmap = ResizeBitmap(bitmap, 10000, bitmap.Height);
                bitmap.Save(saveFileName, imageFormat);
                using (MemoryStream ms = new MemoryStream())
                {
                    // Convert Image to byte[]
                    bitmap.Save(ms, imageFormat);
                    byte[] imageBytes = ms.ToArray();
                    ms.FlushAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                    ms.Close();
                    yield return imageBytes;
                }

                //   

                //bitmap.Save($"D:\\PDFIMage\\{GetNewGuid()}.png", System.Drawing.Imaging.ImageFormat.Png);

            }



            pdf.ReleaseLibrary();



            // int imageCound = pdf.GetImagePageCount(pdfFile)
        }
        public static IEnumerable<string> GetPdfImagesByFileName(string pdfFile, string pdfPassWord)
        {
            PDFLibrary pdf = new PDFLibrary();
            pdf.SetOrigin(0);
            byte[] b = File.ReadAllBytes(pdfFile);
            pdf.LoadFromString(b, pdfPassWord);
            int pageCount = pdf.PageCount();
            for (int currentPageIndex = 1; currentPageIndex <= pageCount; currentPageIndex++)
            {
                pdf.SelectPage(currentPageIndex);
                int imageListID = pdf.GetPageImageList(0);
                int imageCount = pdf.GetImageListCount(imageListID);
                if (imageCount > 1)
                    throw new Exception($"More then one image count {imageCount} for pdffile {pdfFile}");

                int imageIndex = 1;
                byte[] imageData = pdf.GetImageListItemDataToString(imageListID, imageIndex, 0);

                Bitmap bitmap = new Bitmap(new MemoryStream(imageData));
                int h = bitmap.Height;
                int w = bitmap.Width;
                if ((h > 10000) && (w > 10000))
                    bitmap = ResizeBitmap(bitmap, 10000, 10000);
                if (h > 10000)
                    bitmap = ResizeBitmap(bitmap, bitmap.Width, 10000);
                if (w > 10000)
                    bitmap = ResizeBitmap(bitmap, 10000, bitmap.Height);
                string fileName = $"D:\\PDFIMage\\{GetNewGuid()}.png";
                bitmap.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);

                yield return fileName;


                //   

                //bitmap.Save($"D:\\PDFIMage\\{GetNewGuid()}.png", System.Drawing.Imaging.ImageFormat.Png);

            }
            pdf.ReleaseLibrary();

            // int imageCound = pdf.GetImagePageCount(pdfFile)
        }
        public static string GetNewGuid()
        {
            return Guid.NewGuid().ToString();
        }
        public static async Task<string> SearchablePdf(string pdfFile, string apiKey, bool jsonResults)
        {
            string resultTextString = string.Empty;
            string resultJsonString = string.Empty;
            try
            {

                if (!(File.Exists(pdfFile)))
                {
                    throw new Exception($"PDF File not found {pdfFile}");
                }
                HttpClient httpClient = new HttpClient();
                httpClient.Timeout = new TimeSpan(1, 1, 1);
                //Removed the api key from headers
                //httpClient.DefaultRequestHeaders.TryAddWithoutValidation("apikey", "5a64d478-9c89-43d8-88e3-c65de9999580");

                MultipartFormDataContent form = new MultipartFormDataContent();
                //5a64d478-9c89-43d8-88e3-c65de9999580
                form.Add(new StringContent(apiKey), "apikey"); //The "helloworld" key works, but it has a very low rate limit. You can get your won free api key at https://ocr.space/OCRAPI
                form.Add(new StringContent("eng"), "language");
                form.Add(new StringContent("1"), "OCREngine");
                form.Add(new StringContent("false"), "isTable");
                form.Add(new StringContent("true"), "isCreateSearchablePdf");
                form.Add(new StringContent("true"), "isSearchablePdfHideTextLayer");

                //  form.Add(new StringContent("detectOrientation"), "true");

                //  form.Add(new StringContent("TextOverlay"),"true");





                byte[] imageData = File.ReadAllBytes(pdfFile);
                TL.TraceLoggerInstance.TraceInformationConsole($"Creating searchable pdf file {pdfFile}");
                // form.Add(new StringContent("filetype"), "PDF");
                form.Add(new ByteArrayContent(imageData, 0, imageData.Length), "PDF", "pdf.pdf");


                //   HttpResponseMessage response = await httpClient.PostAsync("https://api.ocr.space/Parse/Image", form);
                HttpResponseMessage response = await httpClient.PostAsync("https://apipro1.ocr.space/parse/image", form);

                resultJsonString = await response.Content.ReadAsStringAsync();



                RootobjectPDF ocrResult = JsonConvert.DeserializeObject<RootobjectPDF>(resultJsonString);
                TL.TraceLoggerInstance.TraceInformationConsole($"Ocr results exit code {ocrResult.OCRExitCode}");

                if (ocrResult.OCRExitCode == 1)
                {
                    resultTextString = ocrResult.SearchablePDFURL;
                    //TL.TraceLoggerInstance.TraceInformationConsole($"Getting ocr results");
                    //for (int i = 0; i < ocrResult.ParsedresultPDF.Count(); i++)
                    //{
                    //    resultTextString += ocrResult.ParsedresultPDF[i].SearchablePDFURL;
                    //}


                    //if (jsonResults)
                    //    resultTextString = resultJsonString.Replace("\\r", "").Replace("\\n", "");

                }
                else
                {
                    throw new Exception("ERROR: " + resultJsonString + ocrResult.ErrorMessage + ocrResult.ErrorDetails);
                }




            }
            catch (Exception exception)
            {
                TL.TraceLoggerInstance.TraceError($"Searchable PDF results exit code {resultJsonString} {exception.Message} for file {pdfFile}");
                throw new Exception($"Ocr return string {resultJsonString} {exception.Message}");
            }
            return resultTextString;
        }
        public static async Task<string> TessOcrPDF(string pdfFileName, string workingDir, string pdfPassWord, int imagedpi)
        {
            //string retOctText = string.Empty;
            PDFLibrary pdf = new PDFLibrary();
            pdf.SetOrigin(0);
            pdf.LoadFromFile(pdfFileName, pdfPassWord);
            int iNumPages = pdf.PageCount();
            pdfFileName = Path.GetFileNameWithoutExtension(pdfFileName);
            string bmpSaveFileName = $"{workingDir}{pdfFileName}.tif";
            pdf.RenderDocumentToFile(imagedpi, 1, iNumPages, 0, bmpSaveFileName);
            pdf.ReleaseLibrary();

            return GetImageText(workingDir, pdfFileName).ConfigureAwait(false).GetAwaiter().GetResult();

        }
        public static async Task SavePDFAsImage(string pdfFileName, string saveImgDir, string pdfPassWord, int imagedpi, string extension)
        {
            //string retOctText = string.Empty;
            if (!(File.Exists(pdfFileName)))
                throw new Exception($"Error Converting pdf file to image file {pdfFileName} not found");
            PDFLibrary pdf = new PDFLibrary();
            pdf.SetOrigin(0);
            pdf.LoadFromFile(pdfFileName, pdfPassWord);
            int iNumPages = pdf.PageCount();
            pdfFileName = Path.GetFileNameWithoutExtension(pdfFileName);

            //    string bmpSaveFileName = Path.Combine(saveImgDir, $"{pdfFileName}.tif");
            string bmpSaveFileName = Path.Combine(saveImgDir, $"{pdfFileName}.{extension}");


            pdf.RenderDocumentToFile(imagedpi, 1, iNumPages, 0, bmpSaveFileName);
            pdf.ReleaseLibrary();



        }
        private static async Task<string> GetImageText(string wf, string pdfFileName)
        {
            string retOctText = string.Empty;
            foreach (string imgFiles in Directory.GetFiles(wf, $"{pdfFileName}*.tif"))
            {

                Bitmap ocrBitMap = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromFile(imgFiles);
                using (var engine = new TesseractEngine(tessPath, "eng", EngineMode.Default))
                {
                    using (var img = PixConverter.ToPix((System.Drawing.Bitmap)ocrBitMap))
                    {

                        using (var page = engine.Process(img, PageSegMode.AutoOnly)) //was (img, pageSegMode))
                        {
                            retOctText += page.GetText();

                        }
                    }

                }

            }
            return retOctText.Trim();
        }
        public static byte[] ImageToBase64(System.Drawing.Image image, System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();
                ms.FlushAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                ms.Close();
                return imageBytes;
            }
        }
    }
}
