using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Newtonsoft.Json;
using System.Data.SqlClient;
using Edocs.OCR.FullText.PDF.Models;
 
namespace Edocs.OCR.FullText.PDF
{
    class LocalSqlServer
    {
        private static LocalSqlServer instance = null;
        public static LocalSqlServer SqlServerInstance
        {
            get
            {
                if (instance == null)
                    instance = new LocalSqlServer();
                return instance;
            }
        }
        private LocalSqlServer()
        {
        }
        public async Task<IDictionary<int, LabReqsModel>> GetLabReqs(string sqlConnectionString,string scanSTDate, string scanEndDate)
        {
            IDictionary<int, LabReqsModel> valuePairs = new Dictionary<int, LabReqsModel>();
            try
            {

             string jrResults = GetLabRecsPDFToOcr(sqlConnectionString, DateTime.Parse(scanSTDate), DateTime.Parse(scanEndDate)).ConfigureAwait(false).GetAwaiter().GetResult();
              
                List<LabReqsModel> labReqsModels = JsonConvert.DeserializeObject<List<LabReqsModel>>(jrResults);
                valuePairs = labReqsModels.ToDictionary(p => p.LabReqID);
                 

            }
            catch (Exception ex)
            {
                throw new Exception($"Getting lab recsd for webAPiUri  controller  scanstartdate {scanSTDate} scanenddate {scanEndDate}");

            }
            return valuePairs;
        }
        public async Task AddNYPPDFFullText(PDFFullTextModel fullTextModel, string sqlConnectionStr,DateTime scanDate)
        {
            try
            {

                //    var k  = JsonConvert.DeserializeObject<(jsonFile);

                //   var k1 = JsonConvert.DeserializeObject(jsonFile);


                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(OCRConstants.SpUploadLabReqsFullText, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 600;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(OCRConstants.SpParmaLabReqID, fullTextModel.ID));
                        sqlCmd.Parameters.Add(new SqlParameter(OCRConstants.SpParmaPDFFullText, fullTextModel.PDFFullText));
                        sqlCmd.Parameters.Add(new SqlParameter(OCRConstants.SpParmaScanDate,scanDate.ToString()));
                        await sqlConnection.OpenAsync();
                        SqlDataReader dr = await sqlCmd.ExecuteReaderAsync();

                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<string> GetLabRecsPDFToOcr(string sqlConnectionString, DateTime scanStartDate, DateTime scanEndDate)

        {
            
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(OCRConstants.SpNypLabReqsGetPDFSToOCR, sqlConnection))
                    {
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new SqlParameter(OCRConstants.SpParmaScanStartDate, scanStartDate));
                        sqlCmd.Parameters.Add(new SqlParameter(OCRConstants.SpParmaScanEndDate, scanEndDate));
                        return await GetJsonResults(sqlConnection, sqlCmd).ConfigureAwait(true);
                    }

                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
        public async Task<string> GetJsonResults(SqlConnection sqlConnection, SqlCommand sqlCmd)
        {
            try
            {

                await sqlConnection.OpenAsync();
                SqlDataReader dr = await sqlCmd.ExecuteReaderAsync();

                var dt = new DataTable();
                dt.BeginLoadData();
                dt.Load(dr);
                dt.EndLoadData();
                dt.AcceptChanges();
              
                return JsonConvert.SerializeObject(dt);  

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
