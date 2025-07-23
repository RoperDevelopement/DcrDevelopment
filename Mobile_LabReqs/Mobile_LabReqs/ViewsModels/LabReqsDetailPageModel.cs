using System;
using System.Collections.Generic;
using System.Text;
using Mobile_LabReqs.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Mobile_LabReqs.ViewsModels;
using Newtonsoft.Json;
using System.Xml.Linq;

namespace Mobile_LabReqs.ViewsModels
{
    [QueryProperty(nameof(JsonStr),"JsonStr")]
    class LabReqsDetailPageModel : BaseViewModel
    {
           string itemId = string.Empty;
      
        public string JsonStr
        {
            get => itemId;

            set
            {
                itemId = Uri.UnescapeDataString(value ?? string.Empty);
                GetLRecsMode(itemId);
                OnPropertyChanged();


            }
        }
        private string fileUrl { get; set; }
        public string FileUrl
        { 
            get { return fileUrl; }
            set { fileUrl = value;
                OnPropertyChanged("FileUrl");
            }
        }
        private string financialNumber { get; set; }
        public string FinancialNumber
        {
            get { return financialNumber; }
            set
            {
                financialNumber = value;
                OnPropertyChanged("FinancialNumber");
            }
        }

        private string dateOfService { get; set; }
        public string DateOfService
        {
            get { return dateOfService; }
            set
            {
                dateOfService = value;
                OnPropertyChanged("DateOfService");
            }
        }

        private string drCode { get; set; }
        public string DrCode
        {
            get { return drCode; }
            set
            {
                drCode = value;
                OnPropertyChanged("DrCode");
            }
        }

        private string drName { get; set; }
        public string DrName
        {
            get { return drName; }
            set
            {
                drName = value;
                OnPropertyChanged("DrName");
            }
        }

        private string indexNumber { get; set; }
        public string IndexNumber
        {
            get { return indexNumber; }
            set
            {
                indexNumber = value;
                OnPropertyChanged("IndexNumber");
            }
        }

        private string mrn { get; set; }
        public string MRN
        {
            get { return mrn; }
            set
            {
                mrn = value;
                OnPropertyChanged("MRN");
            }
        }
         private string patientID { get; set; }
        public string PatientID
        {
            get { return patientID; }
            set
            {
                patientID = value;
                OnPropertyChanged("PatientID");
            }
        }



              private string patientName { get; set; }
        public string PatientName
        {
            get { return patientName; }
            set
            {
                patientName = value;
                OnPropertyChanged("PatientName");
            }
        }

        private string requisitionNumber { get; set; }
        public string RequisitionNumber
        {
            get { return requisitionNumber; }
            set
            {
                requisitionNumber = value;
                OnPropertyChanged("RequisitionNumber");
            }
        }

        private string scanOperator { get; set; }
        public string ScanOperator
        {
            get { return scanOperator; }
            set
            {
                scanOperator = value;
                OnPropertyChanged("ScanOperator");
            }
        }
        

        public void GetLRecsMode(string jStr)
        {
             
          
              var reqsModel= JsonConvert.DeserializeObject<LabReqsModel>(jStr);
             FileUrl = reqsModel.FileUrl;
            FinancialNumber = reqsModel.FinancialNumber;
            DateOfService =   reqsModel.DateOfService.ToString("MM/dd/yyyy");
            if(string.IsNullOrWhiteSpace(reqsModel.DrCode))
                DrCode = "N/A";
            else
                DrCode = reqsModel.DrCode;
            if (string.IsNullOrWhiteSpace(reqsModel.DrName))
                DrName = "N/A";
            else
                DrName = reqsModel.DrName;
            IndexNumber = reqsModel.IndexNumber;
           
            if(string.IsNullOrWhiteSpace(reqsModel.PatientID))
            {
                PatientID = "N/A";
                MRN = "N/A";
            }
            else
            {
                PatientID = reqsModel.PatientID;
                MRN = reqsModel.MRN;
            }


            if (string.IsNullOrWhiteSpace(reqsModel.PatientName))
                patientName = "N/A";
            else
                PatientName = reqsModel.PatientName;
            if (string.IsNullOrWhiteSpace(reqsModel.RequisitionNumber))
                RequisitionNumber = "N/A";
            else
                RequisitionNumber = reqsModel.RequisitionNumber;
            if (string.IsNullOrWhiteSpace(reqsModel.ScanOperator))
                ScanOperator = "N/A";
            else
                ScanOperator = reqsModel.ScanOperator;

        }
       
        public LabReqsDetailPageModel( )
        {
            Title = "LabReqs Detail Page";
        }


    }
}
