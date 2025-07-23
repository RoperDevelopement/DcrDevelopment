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

using Edocs.Inventory.Tracking.System.RestFul.Api.Models;
using Edocs.ITS.AppService.Interfaces;

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
        public async Task EdocsNewCustomer(EdocsITSCustomersModel customersModel, string sqlConnectionStr)
        {
            try
            {
                EdocsITSJsonBasicApis.JsonInstance.UploadJsonFile(customersModel, sqlConnectionStr, EdocsITSConstants.SpAddUpdateEdocsCustomers).ConfigureAwait(true).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding new customer {customersModel.EdocsCustomerName} {ex.Message}");
            }
        }
        public async Task<JsonResult> MDTGetRecsToOCT(string sqlConnectionStr)
        {
            try
            {
                return EdocsITSJsonBasicApis.JsonInstance.GetJsonResults(sqlConnectionStr, EdocsITSConstants.SPGetMDTRecordsOCR).ConfigureAwait(true).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting MDT Records to OCT {ex.Message}");
            }
        }

        public async Task<JsonResult> EdocsAddInventoryTrackingID(EdocsITSInventoryTransfer edocsITSInventory, string sqlConnectionStr, string storedProcedueName)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaTrackingID, edocsITSInventory.TrackingID));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaEdocsCustomerName, edocsITSInventory.EdocsCustomerName));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaLoginName, edocsITSInventory.UserName));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaDateSent, edocsITSInventory.DateSent));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaNumberDocsSent, edocsITSInventory.NumberDocsSent));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaScanType, edocsITSInventory.ScanType));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaDeliveryMethod, edocsITSInventory.DeliveryMethod));
                        return EdocsITSJsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding new customer {edocsITSInventory.TrackingID} {ex.Message}");
            }
        }

        public async Task<JsonResult> UpLoadInvoiceHtml(HtmlFileModel htmlFile, string sqlConnectionStr, string storedProcedueName)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaInvoiceNumber, htmlFile.InvoiceNum));
                        if (!(string.IsNullOrWhiteSpace(htmlFile.HtmlData)))
                            sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaHtmlData, htmlFile.HtmlData));

                        return EdocsITSJsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding Invoice html data {htmlFile.InvoiceNum} {ex.Message}");
            }
        }
        public async Task<JsonResult> GetTrackingIds(string trackingID, string sqlConnectionStr, string storedProcedueName)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        if (string.Compare(trackingID, "All TrackingIDs", true) == 0)
                            sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpIDTracking, DBNull.Value));
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpIDTracking, trackingID));


                        return EdocsITSJsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error gettign tracking id {trackingID} {ex.Message}");
            }
        }

        public static async Task<JsonResult> GetInvNumDateSent(int custID, string sqlConnectionStr, string storedProcedueName)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaCustomerID, custID));


                        return EdocsITSJsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding Invoice html data {custID} {ex.Message}");
            }
        }
        public async Task<JsonResult> EdocsInventoryTrackingGenerateInvoice(InvoiceModel invoiceModel, string sqlConnectionStr, string storedProcedueName)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaCustomerID, invoiceModel.EdocsCustomerID));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaInvoiceStartDate, invoiceModel.InvoiceStartDate));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaInvoiceEndDate, invoiceModel.InvoiceEndDate));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaInvoiceTotalAmount, invoiceModel.InvoiceTotalAmount));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaFileName, invoiceModel.FileName));

                        return EdocsITSJsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding new customer {invoiceModel.EdocsCustomerID} {ex.Message}");
            }
        }

        public async Task<JsonResult> GetCustomerUnPaidInvoices(string sqlConnectionStr, string storedProcedueName)
        {
            try
            {



                return EdocsITSJsonBasicApis.JsonInstance.GetJsonResults(sqlConnectionStr, storedProcedueName).ConfigureAwait(false).GetAwaiter().GetResult();

            }


            catch (Exception ex)
            {
                throw new Exception($"Error getting customer invoices {ex.Message}");
            }
        }

        public async Task<JsonResult> EdocsInventoryTrackingGetDocuments(int custID, string sqlConnectionStr, string storedProcedueName)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        // sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaID, 0));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaCustomerID, custID));


                        return EdocsITSJsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding new customer {custID} {ex.Message}");
            }
        }

        public async Task MDTUpDateOCRTotals(string sqlConnectionStr, string storedProcedueName, int id, int totDocsOCT)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaID, id));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaNumberDocsOCR, totDocsOCT));

                        EdocsITSJsonBasicApis.JsonInstance.ExecSP(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating OCR Totals for id {id} totals {totDocsOCT} {ex.Message}");
            }
        }
        public async Task<JsonResult> UploadTrackingByProject(UploadTrackingByProjectNameModel upLoadTracking, string sqlConnectionStr, string spName)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(spName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaTrackingID, upLoadTracking.TrackingID));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaBatchID, upLoadTracking.ScanBatchID));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaCustomerID, upLoadTracking.EdocsCustomerID));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaNumberDocsScanned, upLoadTracking.NumberDocsScanned));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaNumberDocsUploaded, upLoadTracking.NumberDocsUploaded));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpNumberTypedPerDoc, upLoadTracking.NumberTypedPerDoc));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpNumberDocOCR, upLoadTracking.NumberDocOCR));

                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaScanOperator, upLoadTracking.ScanOperator));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaFileName, upLoadTracking.FileName));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaScanMachine, upLoadTracking.ScanMachine));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaStandardLargeDoc, upLoadTracking.StandardLargeDocument));

                        return EdocsITSJsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding new customer {upLoadTracking.TrackingID}");
            }
        }
        public async Task<JsonResult> UpDateAcceptRejectDocs(AcceptRejectDocumentsModel acceptReject, string sqlConnectionStr, string spName)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(spName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaID, acceptReject.ID));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaAcceptRject, acceptReject.AcceptRejectDoc));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaComments, acceptReject.Comments));


                        return EdocsITSJsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating accept reject documents {acceptReject.ID} {ex.Message}");
            }
        }



        public async Task<JsonResult> EdocsUpInventoryTrackingID(EdocsITSScanningManModel iTSScanningManModel, string sqlConnectionStr, string storedProcedueName)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        if (iTSScanningManModel.IDTracking != 0)
                            sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpIDTracking, iTSScanningManModel.IDTracking));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaTrackingID, iTSScanningManModel.TrackingID));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaNumberDocsRecived, iTSScanningManModel.NumberDocumentsReceived));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaDateDocumentsReceived, iTSScanningManModel.DateDocumentsReceived));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaDateScanningStarted, iTSScanningManModel.DateScanningStarted));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaLoginName, iTSScanningManModel.UserName));
                        return EdocsITSJsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding new customer {iTSScanningManModel.TrackingID} {ex.Message}");
            }
        }


        public async Task<JsonResult> ReportDocSent(string sqlConnectionStr, string repType, string stDate, string endDate, string storedProcedueName, string custName)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaEdocsCustomerName, custName));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaRepType, repType));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaRepStDate, stDate));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaRepEndDate, endDate));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaTrackingID, DBNull.Value));
                        return EdocsITSJsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<JsonResult> ScanningCost(string sqlConnectionStr, string stDate, string endDate, int custID,string spName)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(spName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaRepStDate, stDate));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaRepEndDate, endDate));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaCustomerID, custID));
                        return EdocsITSJsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<JsonResult> ReportProjectNameNum(string sqlConnectionStr, string stDate, string endDate, string storedProcedueName)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaRepStDate, stDate));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaRepEndDate, endDate));
                        return EdocsITSJsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<JsonResult> ReportTrackingIDDocNameNum(string sqlConnectionStr, string stDate, string endDate, string trackinIDDocname, string repType)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(EdocsITSConstants.SpRunReportByTrackID_DocName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaRepStDate, stDate));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaRepEndDate, endDate));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaTrackingID, trackinIDDocname));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaRepType, repType));
                        return EdocsITSJsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<JsonResult> GetProjectMinMaxDate(string sqlConnectionStr, int cusID)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(EdocsITSConstants.SPGetMaxMinDateByProjectName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaCustomerID, cusID));
                        return EdocsITSJsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<JsonResult> GetInvoices(string sqlConnectionStr, int cusID)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(EdocsITSConstants.SPGetInvoiceNotPaid, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaCustomerID, cusID));
                        return EdocsITSJsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<JsonResult> GetTrackingIDSByCustomerID(string sqlConnectionStr, int cusID)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(EdocsITSConstants.SpGetTrackingIDsByCustomerID, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaCustomerID, cusID));
                        return EdocsITSJsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<JsonResult> EdocsITSGetTrackingId(string trackingID, string custName, string sqlConnectionStr, string storedProcedueName)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaTrackingID, trackingID));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaEdocsCustomerName, custName));
                        return EdocsITSJsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting tracking id {trackingID} for customer {custName} {ex.Message}");
            }
        }

        public async Task<JsonResult> EdocsITSGetFileName(int ID, string sqlConnectionStr, string storedProcedueName)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaCustomerID, ID));

                        return EdocsITSJsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting report for id {ID} for {ex.Message}");
            }
        }
        public async Task<JsonResult> EdocsITSGetCustomer(string sqlConnectionStr, string storedProcedueName, int custID)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        if (custID > 0)
                            sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaCustomerID, custID));
                        return EdocsITSJsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting EdocsITS customers {ex.Message}");
            }
        }
        public async Task<JsonResult> EdocsITSGetCustomerByCustName(string sqlConnectionStr, string storedProcedueName, string custName)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaEdocsCustomerName, custName));
                        return EdocsITSJsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting EdocsITS customers {ex.Message}");
            }
        }

        public async Task<JsonResult> EdocsITSAddDocsScanned(string sqlConnectionStr, string storedProcedueName, EdocsITSAddNumberDocsScannedModel edocsITSAddNumberDocsScannedModel)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaTrackingID, edocsITSAddNumberDocsScannedModel.TrackingID));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaNumberDocsScanned, edocsITSAddNumberDocsScannedModel.TotalScanned));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaNumberDocsUploaded, edocsITSAddNumberDocsScannedModel.TotalRecordsUploaded));
                        return EdocsITSJsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting EdocsITS customers {ex.Message}");
            }
        }

        //string trackingID, int edocsCustomerID, int numberDocsScanned, int numberDocsUploaded, string spName,string loginName
        public async Task EdocsAddInventoryTrackingIDByProjectName(string sqlConnectionStr, string trackingID, int edocsCustomerID, int numberDocsScanned, int numberDocsUploaded, string storedProcedueName, string loginName)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaTrackingID, trackingID));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaCustomerID, edocsCustomerID));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaLoginName, loginName));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaNumberDocsScanned, numberDocsScanned));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaNumberDocsUploaded, numberDocsUploaded));

                        EdocsITSJsonBasicApis.JsonInstance.ExecSP(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding new trackcing id {trackingID} for customer {edocsCustomerID} {ex.Message}");
            }
        }
        #region new apis
        public async Task UpdateInvAmountPaid(int invNum, float tableColoumTotalPaid, float tableColoumAmountPaid, string paid, string sqlConnectionStr, string storedProcedueName)
        {
            using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
            {
                using (SqlCommand sqlCmdUpdate = new SqlCommand(storedProcedueName, sqlConnection))
                {
                    sqlCmdUpdate.CommandTimeout = 180;
                    sqlCmdUpdate.CommandType = CommandType.StoredProcedure;
                    sqlCmdUpdate.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaInvoiceNumber, invNum));
                    sqlCmdUpdate.Parameters.Add(new SqlParameter(EdocsITSConstants.SpAmountPaid, tableColoumTotalPaid));
                    sqlCmdUpdate.Parameters.Add(new SqlParameter(EdocsITSConstants.SpTotalAmountPaid, tableColoumAmountPaid));

                    sqlCmdUpdate.Parameters.Add(new SqlParameter(EdocsITSConstants.SpPaid, paid));



                    EdocsITSJsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmdUpdate).ConfigureAwait(false).GetAwaiter().GetResult();


                }
            }

        }
        public async Task<JsonResult> AddCustomInvoice(IList<CustomInvoiceModel> invoiceModel, string sqlConnectionStr)
        {
            using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
            {
                using (SqlCommand sqlCmd = new SqlCommand(EdocsITSConstants.SPAddCustomInvoice, sqlConnection))
                {
                    sqlCmd.CommandTimeout = 180;
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    foreach (var cInv in invoiceModel)
                    {
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaCustomerID, cInv.EdocsCustomerID));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaItemDescription, cInv.ItemDescription));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaItemCost, cInv.ItemCost));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaDateOFService, cInv.DateofService));
                        EdocsITSJsonBasicApis.JsonInstance.AddData(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();
                        sqlCmd.Parameters.Clear();

                    }
                }
            }
            return null;
        }
        public async Task<JsonResult> UpdateInvAmountPaid(int custID, string amountPaid, int invNum, string sqlConnectionStr, string storedProcedueName)
        {
            try
            {
                float paid = float.Parse(amountPaid);
                float tableColoumTotalPaid = (float)0.00;
                float tableColoumAmountPaid = (float)0.00;
                float tableColoumTotalInvoiceAmount = (float)0.00;
                string tablePaid = string.Empty;
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(EdocsITSConstants.SpGetInvoiceBalance, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaInvoiceNumber, invNum));




                        SqlDataReader dr = EdocsITSJsonBasicApis.JsonInstance.GetDataReaderResults(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();
                        if (dr.HasRows)
                        {
                            dr.Read();
                            tableColoumTotalPaid = float.Parse(dr[EdocsITSConstants.TableColoumTotalPaid].ToString()) + paid;
                            tableColoumAmountPaid = float.Parse(dr[EdocsITSConstants.TableColoumAmountPaid].ToString()) + paid;
                            tableColoumTotalInvoiceAmount = float.Parse(dr[EdocsITSConstants.TableColoumTotalInvoiceAmount].ToString());
                            tablePaid = dr[EdocsITSConstants.TablePaid].ToString();

                            tableColoumAmountPaid = tableColoumTotalInvoiceAmount - tableColoumAmountPaid;
                            if (tableColoumAmountPaid == 0)
                            {
                                tablePaid = "1";
                                //   paid = tableColoumTotalPaid;
                            }

                        }
                        else
                            throw new Exception($"Could not get  invoice Balance for invoice {invNum}");


                    }



                }

                UpdateInvAmountPaid(invNum, tableColoumTotalPaid, tableColoumAmountPaid, tablePaid, sqlConnectionStr, EdocsITSConstants.SPUpdateInvoicePaid).ConfigureAwait(true).GetAwaiter().GetResult();
                return GetInvoices(sqlConnectionStr, custID).ConfigureAwait(false).GetAwaiter().GetResult();


            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding Invoice html data {custID} {ex.Message}");
            }
        }
        public async Task<JsonResult> EdocsITSUpDateByTrackingID(EdocsITSTrackingIDModel edocsITSTrackingID, string sqlConnectionStr, string storedProcedueName)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaTrackingID, edocsITSTrackingID.TrackingID));

                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaNumberDocsScanned, edocsITSTrackingID.NumberDocsScanned));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaNumberDocsUploaded, edocsITSTrackingID.NumberDocsUploaded));
                        return EdocsITSJsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting EdocsITS customers {ex.Message}");
            }
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<JsonResult> GetDocumentNames(string sqlConnectionStr, string custNum, string sDate, string eDate, string spName)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(spName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaCustomerID, custNum));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaRepStDate, sDate));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaRepEndDate, eDate));


                        return EdocsITSJsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error gettign tracking id {custNum} {ex.Message}");
            }
        }
        public async Task<JsonResult> GetTrackingIDDocNamesByCustomer(string sqlConnectionStr, int custID, string trackingID, string repType, string repSDate, string repEDate)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(EdocsITSConstants.SpGetTrackingIDsDocNameByCustomerID, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaCustomerID, custID));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaRepStDate, repSDate));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaRepEndDate, repEDate));
                        if (trackingID.ToLower().StartsWith("all"))
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaTrackingID, EdocsITSConstants.AllTrackIDDocName));

                        }
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaTrackingID, trackingID));

                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaRepType, repType));

                        return EdocsITSJsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<JsonResult> EdocsInventoryTrackingGetDocumentsByTrackID(int custID, string sqlConnectionStr, string storedProcedueName, string trackingID, string repType, string repSDate, string repEDate)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        // sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaID, 0));
                      
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaCustomerID, custID));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaRepStDate, repSDate));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaRepEndDate, repEDate));
                        if (trackingID.ToLower().StartsWith("all"))
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaTrackingID, EdocsITSConstants.AllTrackIDDocName));

                        }
                        else
                            sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaTrackingID, trackingID));

                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaRepType, repType));

                        return EdocsITSJsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding new customer {custID} {ex.Message}");
            }
        }
        public async Task<JsonResult> UploadPSUSDRecords(string sqlConnectionStr, PSUSDUploadRecordsModel pSUSDUploadRecordsModel)
        {
            try
            {
                var js = JsonConvert.SerializeObject(pSUSDUploadRecordsModel);
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(EdocsITSConstants.SpUploadPSUSDRecors, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaJasonFile, js));
                        
                        return EdocsITSJsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<JsonResult> UploadPSUSDFullText(string sqlConnectionStr, PSUSDUpLoadFullText fullText)
        {
            try
            {
                var js = JsonConvert.SerializeObject(fullText);
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(EdocsITSConstants.SpUploadPSUSDFullText, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaJasonFile, js));

                        return EdocsITSJsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<JsonResult> PSUSDRecordSearch(string sqlConnectionStr, string spName,string searchFor,string repType,DateTime stDate,DateTime endDate)
        {
            try
            {
                
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(spName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaSearch, searchFor));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaRepType,repType));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaRepStDate,stDate));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaRepEndDate, endDate));

                        return EdocsITSJsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<JsonResult> GetpsusdRecordsbyKeyWord(string keyWords, string sqlConnectionString)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            try
            {
                keyWords = keyWords.Replace(EdocsITSConstants.DoubleQuotes, "");
                string wereCluse = string.Empty;
                string selStr = "SELECT [Department] as Department ,[OrginationDepartment] as OrginationDepartment,[DescriptionOfRecords] as DescriptionOfRecords,[MethOfFiling] as MethOfFiling,[FirstName] as FirstName,[LastName] as LastName,[DateOfBirth] as DateOfBirth,[DateOfRecords] DateOfRecords,pn.FileID ID  ,PN.BatchID as TrackingID FROM[dbo].[PSUSDFullText] psusdft join[dbo].[PSUSDRecords] Recs on psusdft.[IDTracking] = Recs.TrackindID join[EdocsITSTrackingIDByProjectName] PN on Recs.TrackindID = PN.ID";
                //where[PermitNum] = @PermitNum;"
                //     CONTAINS([PDFLabReqsFullText], @srcStr)) set @srcStr= '"'+@KeyWordSearch+'*"';
                foreach (string keyword in keyWords.Split(','))
                {
                    if (string.IsNullOrWhiteSpace(keyword))
                        continue;
                    if (string.IsNullOrWhiteSpace(wereCluse))
                    {
                        //    wereCluse = $"'{EdocsDemoConstants.DoubleQuotes}{keyword}{EdocsDemoConstants.DoubleQuotes}'";
                        if (string.IsNullOrWhiteSpace(wereCluse))
                            wereCluse = $"CONTAINS(psusdft.[PSUSDFullText],'{EdocsITSConstants.DoubleQuotes}{keyword}*{EdocsITSConstants.DoubleQuotes}')";

                    }
                    else
                        wereCluse = $"{wereCluse} or CONTAINS(psusdft.[PSUSDFullText],'{EdocsITSConstants.DoubleQuotes}{keyword}*{EdocsITSConstants.DoubleQuotes}')";
                }
                // selStr = $"{selStr}  where CONTAINS(ft.[FullTextSearch],{wereCluse})";
                selStr = $"{selStr} where {wereCluse}";

                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(selStr, sqlConnection))
                    {

                        sqlCmd.CommandType = CommandType.Text;
                        sqlConnection.OpenAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                        SqlDataReader dr = sqlCmd.ExecuteReaderAsync().ConfigureAwait(false).GetAwaiter().GetResult();

                        var dt = new DataTable();
                        dt.BeginLoadData();
                        dt.Load(dr);
                        dt.EndLoadData();
                        dt.AcceptChanges();

                        JsonResult jsonResult = new JsonResult(dt);
                        return jsonResult;

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error looking up keywords  {keyWords} {ex.Message}");

            }
        }
        public async Task<JsonResult> PSUSDRecordFLName(string sqlConnectionStr, string fName,string lName, DateTime stDate, DateTime endDate)
        {
            try
            {
       
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(EdocsITSConstants.SPPSUSDRecordsByFirstLastName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        if(string.Compare(fName,"NA",true) != 0)
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaFirstName,fName));
                        }
                        else
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaFirstName, DBNull.Value));
                        }
                        if (string.Compare(lName, "NA", true) != 0)
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaLastName, lName));
                        }
                        else
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaLastName, DBNull.Value));
                        }
                        
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaRepStDate, stDate));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaRepEndDate, endDate));

                        return EdocsITSJsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<JsonResult> PSUSDRecordDept(string sqlConnectionStr, string fName, string lName,string dept,string orgDept, DateTime stDate, DateTime endDate)
        {
            try
            {
                string spName = EdocsITSConstants.SPPSUSDRecordsByDepOrgDepartment;
                if((string.Compare(fName, "NA", true) != 0) || (string.Compare(lName, "NA", true) != 0))
                    spName = EdocsITSConstants.SPPPSUSDRecordsByDepOrgDepartmentFLName;
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(spName, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaDep,dept));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaOrgDep, orgDept));
                        if (string.Compare(spName, EdocsITSConstants.SPPPSUSDRecordsByDepOrgDepartmentFLName, true) == 0)
                        {
                            if (string.Compare(fName, "NA", true) != 0)
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaFirstName, fName));
                        }
                        else
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaFirstName, DBNull.Value));
                        }
                        

                        
                        if (string.Compare(lName, "NA", true) != 0)
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaLastName, lName));
                        }
                        else
                        {
                            sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaLastName, DBNull.Value));
                        }
                        }
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaRepStDate, stDate));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaRepEndDate, endDate));

                        return EdocsITSJsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<JsonResult> AddInvoiceNumber(AddInvoiceNumberModel numberModel, string sqlConnectionStr)
        {
            try
            {
                 
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(EdocsITSConstants.SpAddEocsITSInvoiceNumber, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;

                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaCustomerID, numberModel.EdocsCustomerID));
                        sqlCmd.Parameters.Add(new SqlParameter(EdocsITSConstants.SpParmaInvoiceNumber, numberModel.InvoiceNum));
                        return EdocsITSJsonBasicApis.JsonInstance.GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(false).GetAwaiter().GetResult();

                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

    }
}
