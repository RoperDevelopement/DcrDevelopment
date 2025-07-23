using Edocs.ITS.AppService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edocs.ITS.AppService.Models
{
    public class EdocsITSProjectNameNumber : IEdocsCustID, IUserEmailAddress, ITotalRecords, IDateUploaded, ITrackingIDS,IDocSent
    {
        public int EdocsCustomerID
        { get; set; }
        public string EdocsCustomerName { get; set; }
        public string TrackingIDS
        {

            get; set;
        }
        public string EmailAddress
        { get; set; }
        public int TotalScanned
        { get; set; }
        public int TotalRecordsUploaded
        { get; set; }

        public string TrackingID
        { get; set; }
        public string FileName
        { get; set; }
        public DateTime ScannDate
        { get; set; }
        public int ID
        { get; set; }
        public int TotalDocsOCR
        { get; set; }
        public int TotalCharTyped
        { get; set; }
        public float TotalOcrCost
        { get; set; }
        public float TotalCostPerDoc
        { get; set; }
        public float TotalCostPerChar
        { get; set; }
        public bool DocSent
        { get; set; }

    }
}
