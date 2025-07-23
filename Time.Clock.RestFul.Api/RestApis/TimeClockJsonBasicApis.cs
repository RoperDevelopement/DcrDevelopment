using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;

using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Edocs.Employee.Time.Clock.RestFul.Api.RestApis
{
    public class TimeClockJsonBasicApis
    {
        private static TimeClockJsonBasicApis instance = null;

        private TimeClockJsonBasicApis() { }

        public static TimeClockJsonBasicApis JsonInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TimeClockJsonBasicApis();
                }
                return instance;
            }
        }
        public async Task ExecSP(SqlConnection sqlConnection, SqlCommand sqlCmd)
        {
            await sqlConnection.OpenAsync();
            SqlDataReader dr = await sqlCmd.ExecuteReaderAsync();

        }
        public async Task<JsonResult> GetJsonResults(string sqlConnectionString, string storedProcedure)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(storedProcedure, sqlConnection))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.CommandTimeout = 180;
                        await sqlConnection.OpenAsync();
                        SqlDataReader dr = await sqlCmd.ExecuteReaderAsync();
                        //if(dr.HasRows)
                        //{
                        //    dr.Read();
                        //    string s = dr[0].ToString();
                        //}
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

                throw new Exception(ex.Message);
            }

        }
        public async Task<JsonResult> GetJsonResults(SqlConnection sqlConnection, SqlCommand sqlCmd)
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

                JsonResult jsonResult = new JsonResult(dt);
                return jsonResult;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        //public async Task UploadJsonFile(Object jsonFile, string sqlConnectionStr, string storedProcedueName)
        //{
        //    try
        //    {
                
        //        var js = JsonConvert.SerializeObject(jsonFile);



        //        using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionStr))
        //        {
        //            using (SqlCommand sqlCmd = new SqlCommand(storedProcedueName, sqlConnection))
        //            {
        //                sqlCmd.CommandTimeout = 180;
        //                sqlCmd.CommandType = CommandType.StoredProcedure;
        //                sqlCmd.Parameters.Add(new SqlParameter(.SpParamJsonFile, js));
        //                await sqlConnection.OpenAsync();
        //                SqlDataReader dr = await sqlCmd.ExecuteReaderAsync();

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}
    }
}
