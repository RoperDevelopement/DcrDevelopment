using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MDT.OCT.Projects.Interfaces;
using Newtonsoft.Json;

namespace MDT.OCT.Projects.Models
{
    internal class MDTOCRModel:IMDTOCRRecs
    {
        [JsonProperty("ID")]
        public int ID { get; set; }
        [JsonProperty("NumberDocsScanned")]
        public int NumberDocsScanned
        { get; set; }
        [JsonProperty("NumberDocsUploaded")]
        public int NumberDocsUploaded
        { get; set; }
        [JsonProperty("FileName")]
        public string FileName
        { get; set; }
    }
}
