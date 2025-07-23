using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Net.Http;
using System.IO;
using System.Reflection;

using Newtonsoft.Json;
using OCRAPIModel;
//the solution is to convert the image to PNG and then use engine2!
namespace Edocs.Ocr.Convert.Libaray.Img.PDF
{

    public class ConvertImagePDF
    {
        public static async Task<string> OCRSrace(string imagePDFPath, bool isImage, bool jsonResults, string apiKey, string ocrEngine, bool isTable, Uri ocrWebApi)
        {
            string resultTextString = string.Empty;
            string errResults ="No Results";
            // string resultJsonString = string.Empty;
            try
            {

                if (!(File.Exists(imagePDFPath)))
                {
                    throw new Exception($"OCR File not found {imagePDFPath}");
                }
                HttpClient httpClient = new HttpClient();
                httpClient.Timeout = new TimeSpan(0, 3, 1);
                //Removed the api key from headers
                //httpClient.DefaultRequestHeaders.TryAddWithoutValidation("apikey", "5a64d478-9c89-43d8-88e3-c65de9999580");

                MultipartFormDataContent form = new MultipartFormDataContent();
                //5a64d478-9c89-43d8-88e3-c65de9999580
                form.Add(new StringContent(apiKey), "apikey"); //The "helloworld" key works, but it has a very low rate limit. You can get your won free api key at https://ocr.space/OCRAPI
                form.Add(new StringContent("eng"), "language");
                //   form.Add(new StringContent("2"), "OCREngine");
                //  form.Add(new StringContent("1"), "OCREngine");
                form.Add(new StringContent(ocrEngine), "OCREngine");
                form.Add(new StringContent(isTable.ToString()), "isTable");
                //  form.Add(new StringContent("detectOrientation"), "true");
                //isSearchablePdfHideTextLayer
                //  form.Add(new StringContent("TextOverlay"),"true");




                if (isImage)
                {

                    System.Drawing.Image image = System.Drawing.Image.FromFile(imagePDFPath);
                    byte[] imageData = File.ReadAllBytes(imagePDFPath);
                    //  byte[] imageData = ImageToBase64(image,System.Drawing.Imaging.ImageFormat.Tiff);
                    //  form.Add(new StringContent("filetype"), "JPG");
                    form.Add(new ByteArrayContent(imageData, 0, imageData.Length), "image", "image.png");
                }
                else
                {
                    byte[] imageData = File.ReadAllBytes(imagePDFPath);

                    // form.Add(new StringContent("filetype"), "PDF");
                    form.Add(new ByteArrayContent(imageData, 0, imageData.Length), "PDF", "pdf.pdf");
                }

                //   HttpResponseMessage response = await httpClient.PostAsync("https://api.ocr.space/Parse/Image", form);
                //   HttpResponseMessage response =  await httpClient.PostAsync("https://apipro1.ocr.space/parse/image", form);
                var response = httpClient.PostAsync(ocrWebApi, form);
                response.Wait();
                var results = response.Result;

                var resultJsonString = results.Content.ReadAsStringAsync();

                errResults = resultJsonString.ToString();

                Rootobject ocrResult = JsonConvert.DeserializeObject<Rootobject>(resultJsonString.ToString());


                if (ocrResult.OCRExitCode == 1)
                {

                    for (int i = 0; i < ocrResult.ParsedResults.Count(); i++)
                    {
                        resultTextString += ocrResult.ParsedResults[i].ParsedText;
                    }


                    if (jsonResults)
                        resultTextString = resultJsonString.ToString().Replace("\\r", "").Replace("\\n", "");

                }
                else
                {
                    throw new Exception("ERROR: " + resultJsonString + ocrResult.ErrorMessage + ocrResult.ErrorDetails);
                }




            }
            catch (Exception exception)
            {

                throw new Exception($"GoogleOcr return string {errResults} {exception.Message}");
            }
            return resultTextString;
        }
        public static async Task<string> OCRSrace(byte[] imageBytes, bool isImage, bool jsonResults, string apiKey, string ocrEngine, bool isTable, Uri ocrWebApi,string imageExtension)
        {
            string resultTextString = string.Empty;
            string errResults = "No Results";
           
            try
            {

                if (imageBytes.Length == 0)
                {
                    throw new Exception($"No bytes for image data");
                }
                HttpClient httpClient = new HttpClient();
                httpClient.Timeout = new TimeSpan(0, 3, 0);
                //Removed the api key from headers
                //httpClient.DefaultRequestHeaders.TryAddWithoutValidation("apikey", "5a64d478-9c89-43d8-88e3-c65de9999580");

                MultipartFormDataContent form = new MultipartFormDataContent();
                //5a64d478-9c89-43d8-88e3-c65de9999580
                form.Add(new StringContent(apiKey), "apikey"); //The "helloworld" key works, but it has a very low rate limit. You can get your won free api key at https://ocr.space/OCRAPI
                form.Add(new StringContent("eng"), "language");
                // form.Add(new StringContent("2"), "OCREngine");
                //    form.Add(new StringContent("1"), "OCREngine");
                form.Add(new StringContent(ocrEngine), "OCREngine");
                form.Add(new StringContent(isTable.ToString()), "isTable");
                //  form.Add(new StringContent("detectOrientation"), "true");
                //isSearchablePdfHideTextLayer
                //  form.Add(new StringContent("TextOverlay"),"true");




                if (isImage)
                {
                    //  TL.TraceLoggerInstance.TraceInformationConsole($"Getting text from image {imagePDFPath}");
                    //   System.Drawing.Image image1 = System.Drawing.Image.FromStream(new MemoryStream(imageBytes));
                    //  byte[] imageData = File.ReadAllBytes(imagePDFPath);
                    //  byte[] imageData = ImageToBase64(image,System.Drawing.Imaging.ImageFormat.Tiff);
                    //  form.Add(new StringContent("filetype"), "JPG");
                    form.Add(new ByteArrayContent(imageBytes, 0, imageBytes.Length), "image", imageExtension);
                }
                else
                {
                    //  byte[] imageData = File.ReadAllBytes(imagePDFPath);
                    // TL.TraceLoggerInstance.TraceInformationConsole($"Getting text from pdf file {imagePDFPath}");
                    // form.Add(new StringContent("filetype"), "PDF");
                    form.Add(new ByteArrayContent(imageBytes, 0, imageBytes.Length), "PDF", "pdf.pdf");
                }

                //   HttpResponseMessage response = await httpClient.PostAsync("https://api.ocr.space/Parse/Image", form);
                  HttpResponseMessage response = await httpClient.PostAsync(ocrWebApi, form);
                //  var response = httpClient.PostAsync(ocrWebApi, form);
                //  response.Wait();
                // var results = response.Result;
                resultTextString = response.Content.ReadAsStringAsync().ConfigureAwait(true).GetAwaiter().GetResult();
                //  var resultJsonString = results.Content.ReadAsStringAsync();

                // errResults = resultJsonString.ToString();

                Rootobject ocrResult = JsonConvert.DeserializeObject<Rootobject>(resultTextString);


              

                if (ocrResult.OCRExitCode == 1)
                {
                    if (jsonResults)
                        resultTextString = resultTextString.Replace("\\r", "").Replace("\\n", "");
                    if (ocrResult.ParsedResults.Count() > 1)
                    {
                        for (int i = 0; i < ocrResult.ParsedResults.Count(); i++)
                        {
                            resultTextString += ocrResult.ParsedResults[i].ParsedText;
                        }
                    }
                    else
                    {
                        Parsedresult[] parsedresults = ocrResult.ParsedResults;
                        resultTextString = parsedresults[0].ParsedText;

                    }




                }
                else
                {
                    throw new Exception("ERROR: " + resultTextString + ocrResult.ErrorMessage + ocrResult.ErrorDetails);
                }




            }
            catch (Exception exception)
            {
                //    TL.TraceLoggerInstance.TraceError($"Ocr results exit code {resultJsonString} {exception.Message} for file {imagePDFPath}");
                throw new Exception($"GoogleOcr return string {resultTextString} {exception.Message}");
            }
            return resultTextString;
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
