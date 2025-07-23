using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Edocs.ITS.AppService.Models;
using Edocs.Libaray.Upload.Archive.Batches.Models;
using MySql.Data.MySqlClient;

namespace Edocs.Inventory.Tracking.System.RestFul.Api.ApisConst
{
    public class EdocsITSApi
    {
        private static EdocsITSApi instance = null;

        private EdocsITSApi() { }

        public static EdocsITSApi EdocsITSInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EdocsITSApi();
                }
                return instance;
            }
        }
        public async Task EdocsNewCustomer(EdocsITSCustomersModel customersModel, string MySqlConnectionStr)
        {
            try
            {
                EdocsITSJsonBasicApis.JsonInstance.UploadJsonFile(customersModel, MySqlConnectionStr, EdocsITSConstants.SpAddUpdateEdocsCustomers).ConfigureAwait(true).GetAwaiter().GetResult();
            }
            catch(Exception ex)
            {
                throw new Exception($"Error adding new customer {customersModel.EdocsCustomerName} {ex.Message}");
            }
          }
        public async Task<JsonResult> MDTGetRecsToOCT(string MySqlConnectionStr)
        {
            try
            {
               return EdocsITSJsonBasicApis.JsonInstance.GetJsonResults(MySqlConnectionStr, EdocsITSConstants.SPGetMDTRecordsOCR).ConfigureAwait(true).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting MDT Records to OCT {ex.Message}");
            }
        }

        public async Task<JsonResult> EdocsAddInventoryTrackingID(EdocsITSInventoryTransfer edocsITSInventory, string MySqlConnectionStr,string storedProcedueName)
        {
            try
            {
                using (MySqlConnection MySqlConnection = new MySqlConnection(MySqlConnectionStr))
                {
                    using (MySqlCommand sqlCmd = new MySqlCommand(storedProcedueName, MySqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaTrackingID,edocsITSInventory.TrackingID));
                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaEdocsCustomerName, edocsITSInventory.EdocsCustomerName));
                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaLoginName, edocsITSInventory.UserName));
                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaDateSent, edocsITSInventory.DateSent));
                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaNumberDocsSent, edocsITSInventory.NumberDocsSent));
                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaScanType, edocsITSInventory.ScanType));
                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaDeliveryMethod, edocsITSInventory.DeliveryMethod));
                        return EdocsITSJsonBasicApis.JsonInstance.GetJsonResults(MySqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding new customer {edocsITSInventory.TrackingID} {ex.Message}");
            }
        }

        public async Task<JsonResult> UpLoadInvoiceHtml(HtmlFileModel htmlFile, string MySqlConnectionStr, string storedProcedueName)
        {
            try
            {
                using (MySqlConnection MySqlConnection = new MySqlConnection(MySqlConnectionStr))
                {
                    using (MySqlCommand sqlCmd = new MySqlCommand(storedProcedueName, MySqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaInvoiceNumber, htmlFile.InvoiceNum));
                        if(!(string.IsNullOrWhiteSpace(htmlFile.HtmlData)))
                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaHtmlData, htmlFile.HtmlData));
                        
                        return EdocsITSJsonBasicApis.JsonInstance.GetJsonResults(MySqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding Invoice html data {htmlFile.InvoiceNum} {ex.Message}");
            }
        }

        public static async Task<JsonResult> GetInvNumDateSent(int custID, string MySqlConnectionStr, string storedProcedueName)
        {
            try
            {
                using (MySqlConnection MySqlConnection = new MySqlConnection(MySqlConnectionStr))
                {
                    using (MySqlCommand sqlCmd = new MySqlCommand(storedProcedueName, MySqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaCustomerID,custID));
                        

                        return EdocsITSJsonBasicApis.JsonInstance.GetJsonResults(MySqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding Invoice html data {custID} {ex.Message}");
            }
        }
        public async Task<JsonResult> EdocsInventoryTrackingGenerateInvoice(InvoiceModel invoiceModel, string MySqlConnectionStr, string storedProcedueName)
        {
            try
            {
                using (MySqlConnection MySqlConnection = new MySqlConnection(MySqlConnectionStr))
                {
                    using (MySqlCommand sqlCmd = new MySqlCommand(storedProcedueName, MySqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaCustomerID, invoiceModel.EdocsCustomerID));
                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaInvoiceStartDate, invoiceModel.InvoiceStartDate));
                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaInvoiceEndDate, invoiceModel.InvoiceEndDate));
                       sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaInvoiceTotalAmount, invoiceModel.InvoiceTotalAmount));
                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaFileName, invoiceModel.FileName));
                        
                        return EdocsITSJsonBasicApis.JsonInstance.GetJsonResults(MySqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding new customer {invoiceModel.EdocsCustomerID} {ex.Message}");
            }
        }
      
        public async Task<JsonResult> GetCustomerUnPaidInvoices(string MySqlConnectionStr, string storedProcedueName)
        {
            try
            {



                return EdocsITSJsonBasicApis.JsonInstance.GetJsonResults(MySqlConnectionStr,storedProcedueName).ConfigureAwait(false).GetAwaiter().GetResult();

            }

             
            catch (Exception ex)
            {
                throw new Exception($"Error getting customer invoices {ex.Message}");
            }
        }

        public async Task<JsonResult> EdocsInventoryTrackingGetDocuments(int custID, string MySqlConnectionStr, string storedProcedueName)
        {
            try
            {
                using (MySqlConnection MySqlConnection = new MySqlConnection(MySqlConnectionStr))
                {
                    using (MySqlCommand sqlCmd = new MySqlCommand(storedProcedueName, MySqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                       // sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaID, 0));
                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaCustomerID,custID));
                        

                        return EdocsITSJsonBasicApis.JsonInstance.GetJsonResults(MySqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding new customer {custID} {ex.Message}");
            }
        }

        public async Task MDTUpDateOCRTotals(string MySqlConnectionStr,string storedProcedueName,int id,int totDocsOCT)
        {
            try
            {
                using (MySqlConnection MySqlConnection = new MySqlConnection(MySqlConnectionStr))
                {
                    using (MySqlCommand sqlCmd = new MySqlCommand(storedProcedueName, MySqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaID,id));
                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaNumberDocsOCR,totDocsOCT));

                        EdocsITSJsonBasicApis.JsonInstance.ExecSP(MySqlConnection,sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating OCR Totals for id {id} totals {totDocsOCT} {ex.Message}");
            }
        }
        public async Task<JsonResult> UploadMDTTracking(UpLoadMDTrackingModel upLoadMDTracking, string MySqlConnectionStr,string spName)
        {
            try
            {
                using (MySqlConnection MySqlConnection = new MySqlConnection(MySqlConnectionStr))
                {
                    using (MySqlCommand sqlCmd = new MySqlCommand(spName, MySqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaTrackingID, upLoadMDTracking.InventoryTrackingID));
                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaBatchID, upLoadMDTracking.ScanBatchID));
                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaCustomerID, upLoadMDTracking.EdocsCustomerID));
                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaNumberDocsScanned,upLoadMDTracking.TotalScanned));
                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaNumberDocsUploaded,upLoadMDTracking.TotalPageCount));
                         sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpNumberTypedPerDoc, upLoadMDTracking.TotalType));
                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpNumberDocOCR, upLoadMDTracking.TotalOCR));
                        
                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaScanOperator,upLoadMDTracking.ScanOperator));
                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaFileName, upLoadMDTracking.FileName));
                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaScanMachine, upLoadMDTracking.ScanMachine));
                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaStandardLargeDoc, upLoadMDTracking.StandardLargeDocument));
                        
                        return EdocsITSJsonBasicApis.JsonInstance.GetJsonResults(MySqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding new customer {upLoadMDTracking.InventoryTrackingID}");
            }
        }
        public async Task<JsonResult> UpDateAcceptRejectDocs(AcceptRejectDocumentsModel acceptReject, string MySqlConnectionStr, string spName)
        {
            try
            {
                using (MySqlConnection MySqlConnection = new MySqlConnection(MySqlConnectionStr))
                {
                    using (MySqlCommand sqlCmd = new MySqlCommand(spName, MySqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaID, acceptReject.ID));
                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaAcceptRject, acceptReject.AcceptRejectDoc));
                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaComments, acceptReject.Comments));
                        

                        return EdocsITSJsonBasicApis.JsonInstance.GetJsonResults(MySqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating accept reject documents {acceptReject.ID} {ex.Message}");
            }
        }



        public async Task<JsonResult> EdocsUpInventoryTrackingID(EdocsITSScanningManModel iTSScanningManModel, string MySqlConnectionStr, string storedProcedueName)
        {
            try
            {
                using (MySqlConnection MySqlConnection = new MySqlConnection(MySqlConnectionStr))
                {
                    using (MySqlCommand sqlCmd = new MySqlCommand(storedProcedueName, MySqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaTrackingID, iTSScanningManModel.IDTracking));
                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaNumberDocsRecived, iTSScanningManModel.NumberDocumentsReceived));
                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaDateDocumentsReceived,iTSScanningManModel.DateDocumentsReceived));
                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaDateScanningStarted,iTSScanningManModel.DateScanningStarted));
                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaLoginName,iTSScanningManModel.UserName));
                        return EdocsITSJsonBasicApis.JsonInstance.GetJsonResults(MySqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding new customer {iTSScanningManModel.TrackingID} {ex.Message}");
            }
        }


        public async Task<JsonResult> ReportDocSent(string MySqlConnectionStr, string repType, string stDate, string endDate, string storedProcedueName, string custName)
        {
            try
            {
                using (MySqlConnection MySqlConnection = new MySqlConnection(MySqlConnectionStr))
                {
                    using (MySqlCommand sqlCmd = new MySqlCommand(storedProcedueName, MySqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaEdocsCustomerName,custName));
                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaRepType,repType));
                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaRepStDate,stDate));
                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaRepEndDate,endDate));
                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaTrackingID,DBNull.Value));
                        return EdocsITSJsonBasicApis.JsonInstance.GetJsonResults(MySqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception( ex.Message);
            }
        }
        public async Task<JsonResult> ScanningCost(string MySqlConnectionStr,string stDate, string endDate,int custID)
        {
            try
            {
                using (MySqlConnection MySqlConnection = new MySqlConnection(MySqlConnectionStr))
                {
                    using (MySqlCommand sqlCmd = new MySqlCommand(EdocsITSConstants.SPRunReportInvoiceByCustIDProjectNamePrice, MySqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        string corrForStr = stDate.ToUpper();
                      corrForStr= corrForStr.Replace("AM", "").Replace("PM", "").Replace("-","/").Trim();
                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaRepStDate, corrForStr));
                          corrForStr = endDate.ToUpper();
                        corrForStr = corrForStr.Replace("AM", "").Replace("PM", "").Replace("-", "/").Trim();
                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaRepEndDate, endDate.Replace("-", "/")));
                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaCustomerID,custID));
                        return EdocsITSJsonBasicApis.JsonInstance.GetJsonResults(MySqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<JsonResult> ReportProjectNameNum(string MySqlConnectionStr,string stDate, string endDate, string storedProcedueName)
        {
            try
            {
                using (MySqlConnection MySqlConnection = new MySqlConnection(MySqlConnectionStr))
                {
                    using (MySqlCommand sqlCmd = new MySqlCommand(storedProcedueName, MySqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        
                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaRepStDate, stDate));
                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaRepEndDate, endDate));
                        return EdocsITSJsonBasicApis.JsonInstance.GetJsonResults(MySqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<JsonResult> GetProjectMinMaxDate(string MySqlConnectionStr,int cusID)
        {
            try
            {
                using (MySqlConnection MySqlConnection = new MySqlConnection(MySqlConnectionStr))
                {
                    using (MySqlCommand sqlCmd = new MySqlCommand(EdocsITSConstants.SPGetMaxMinDateByProjectName, MySqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaCustomerID,cusID));
                        return EdocsITSJsonBasicApis.JsonInstance.GetJsonResults(MySqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<JsonResult> GetInvoices(string MySqlConnectionStr, int cusID)
        {
            try
            {
                using (MySqlConnection MySqlConnection = new MySqlConnection(MySqlConnectionStr))
                {
                    using (MySqlCommand sqlCmd = new MySqlCommand(EdocsITSConstants.SPGetInvoiceNotPaid, MySqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaCustomerID, cusID));
                        return EdocsITSJsonBasicApis.JsonInstance.GetJsonResults(MySqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<JsonResult> EdocsITSGetTrackingId(string trackingID,string custName, string MySqlConnectionStr, string storedProcedueName)
        {
            try
            {
                using (MySqlConnection MySqlConnection = new MySqlConnection(MySqlConnectionStr))
                {
                    using (MySqlCommand sqlCmd = new MySqlCommand(storedProcedueName, MySqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaTrackingID,trackingID));
                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaEdocsCustomerName, custName));
                        return EdocsITSJsonBasicApis.JsonInstance.GetJsonResults(MySqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting tracking id {trackingID} for customer {custName} {ex.Message}");
            }
        }

        public async Task<JsonResult> EdocsITSGetFileName(int ID, string MySqlConnectionStr, string storedProcedueName)
        {
            try
            {
                using (MySqlConnection MySqlConnection = new MySqlConnection(MySqlConnectionStr))
                {
                    using (MySqlCommand sqlCmd = new MySqlCommand(storedProcedueName, MySqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaCustomerID,ID));
                        
                        return EdocsITSJsonBasicApis.JsonInstance.GetJsonResults(MySqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting report for id {ID} for {ex.Message}");
            }
        }
        public async Task<JsonResult> EdocsITSGetCustomer(string MySqlConnectionStr, string storedProcedueName,int custID)
        {
            try
            {
                using (MySqlConnection MySqlConnection = new MySqlConnection(MySqlConnectionStr))
                {
                    using (MySqlCommand sqlCmd = new MySqlCommand(storedProcedueName, MySqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        if(custID > 0)
                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaCustomerID, custID));
                        return EdocsITSJsonBasicApis.JsonInstance.GetJsonResults(MySqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting EdocsITS customers {ex.Message}");
            }
        }

        public async Task<JsonResult> EdocsITSAddDocsScanned(string MySqlConnectionStr, string storedProcedueName, EdocsITSAddNumberDocsScannedModel edocsITSAddNumberDocsScannedModel)
        {
            try
            {
                using (MySqlConnection MySqlConnection = new MySqlConnection(MySqlConnectionStr))
                {
                    using (MySqlCommand sqlCmd = new MySqlCommand(storedProcedueName, MySqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        
                            sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaTrackingID, edocsITSAddNumberDocsScannedModel.TrackingID));
                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaNumberDocsScanned, edocsITSAddNumberDocsScannedModel.TotalScanned));
                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaNumberDocsUploaded, edocsITSAddNumberDocsScannedModel.TotalRecordsUploaded));
                        return EdocsITSJsonBasicApis.JsonInstance.GetJsonResults(MySqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting EdocsITS customers {ex.Message}");
            }
        }

        //string trackingID, int edocsCustomerID, int numberDocsScanned, int numberDocsUploaded, string spName,string loginName
        public async Task  EdocsAddInventoryTrackingIDByProjectName(string MySqlConnectionStr, string trackingID, int edocsCustomerID, int numberDocsScanned, int numberDocsUploaded, string storedProcedueName, string loginName)
        {
            try
            {
                using (MySqlConnection MySqlConnection = new MySqlConnection(MySqlConnectionStr))
                {
                    using (MySqlCommand sqlCmd = new MySqlCommand(storedProcedueName, MySqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaTrackingID, trackingID));
                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaCustomerID, edocsCustomerID));
                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaLoginName, loginName));
                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaNumberDocsScanned,numberDocsScanned));
                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParmaNumberDocsUploaded,numberDocsUploaded));
                        
                          EdocsITSJsonBasicApis.JsonInstance.ExecSP(MySqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding new trackcing id {trackingID} for customer {edocsCustomerID} {ex.Message}");
            }
        }

    }
}
