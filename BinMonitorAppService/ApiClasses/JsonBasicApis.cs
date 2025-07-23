using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;

using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using BinMonitorAppService.Models;
using BinMonitorAppService.Constants;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BinMonitorAppService.ApiClasses
{
    public class JsonBasicApis
    {

        private static JsonBasicApis instance = null;

        private JsonBasicApis() { }

        public static JsonBasicApis JsonInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new JsonBasicApis();
                }
                return instance;
            }
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
        public async Task<SqlDataReader> GetJsonResultsDR(SqlConnection sqlConnection, SqlCommand sqlCmd)
        {
            try
            {

                await sqlConnection.OpenAsync();
                SqlDataReader dr = await sqlCmd.ExecuteReaderAsync();

                
                return dr;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
