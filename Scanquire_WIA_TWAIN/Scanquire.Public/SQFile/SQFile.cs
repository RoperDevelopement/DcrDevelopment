/*
 * User: Sam Brinly
 * Date: 2/6/2013
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using EdocsUSA.Utilities;
using EdocsUSA.Utilities.Extensions;
using ETL = EdocsUSA.Utilities.Logging.TraceLogger;

namespace Scanquire.Public
{
    /// <summary>
    /// Description of SQFile.
    /// </summary>
    public class SQFile
    {
        public string MimeType { get; set; }

        private List<ISQCommand_Document> _Commands = new List<ISQCommand_Document>();
        /// <summary>List of document based commands applied to the file.</summary>
        public List<ISQCommand_Document> Commands
        {
            get { return _Commands; }
            set { _Commands = value; }
        }

        private Dictionary<string, object> _IndexFields = new Dictionary<string, object>();
        /// <summary>Metadata properties of the file.</summary>
		public Dictionary<string, object> IndexFields
        {
            get { return _IndexFields; }
            set { _IndexFields = value; }
        }

        /// <summary>Number of pages in the file (optional).</summary>
        public int PageCount { get; set; }

        /// <summary>Binary contents of the file.</summary>
        public byte[] Data { get; set; }

        private string _Checksum = null;
        /// <summary>MD5 checksum of the Data.</summary>
        public string Checksum
        {
            get
            {
                if (_Checksum == null) _Checksum = StringTools.CalculateMD5(Data);
                return _Checksum;
            }
        }

        //public SQCommandList Commands = new SQCommandList();

        private string _FileExtension;
        /// <summary>Windows file extension.</summary>
        /// <remarks>Accepts both dotted and undotted extensions ("pdf or .pdf")</remarks>
        public string FileExtension
        {
            get { return _FileExtension; }
            set { _FileExtension = PathExtensions.DotExtension(value); }
        }

        public static bool DecriptImage
        { get; set; }
        /// <summary>Create a new instance from a local file.</summary>
		public static SQFile FromFile(string path)
        {
            ETL.TraceLoggerInstance.TraceInformation($"Create a new instance from a local file:{path}");

            SQFile file = new SQFile();

            if (Edocs_Utilities.EdocsUtilitiesInstance.DecriptImage)
            {
                ETL.TraceLoggerInstance.TraceWarning($"Recovering image: {path}");
                file.Data = TemporaryFile.ReadAllBytes(path);
                var encoders = ImageCodecInfo.GetImageEncoders();
                var imageCodecInfo = encoders.FirstOrDefault(encoder => encoder.MimeType == "image/tiff") ;

                using (var memoryStreamImg = new MemoryStream(file.Data))
                {
                    Image img = Image.FromStream(memoryStreamImg, true,true);
                    using (var memoryStream = new MemoryStream())
                    {

                        var imageEncoderParams = new EncoderParameters();
                        imageEncoderParams.Param[0] = new EncoderParameter(Encoder.Quality, 100L);
                        img.Save(memoryStream, imageCodecInfo, imageEncoderParams);
                        file.Data = memoryStream.ToArray();
                        memoryStream.Flush();
                    }
                }
            }
            else
                file.Data = File.ReadAllBytes(path);
            file.FileExtension = Path.GetExtension(path);
                return file;
        }
    }


}
