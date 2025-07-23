using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;

using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace Edocs.Demos.Restful.Api
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
        public async Task<string> GetJsonResultsString(SqlConnection sqlConnection, SqlCommand sqlCmd)
        {
            try
            {

                await sqlConnection.OpenAsync();
                SqlDataReader dr = await sqlCmd.ExecuteReaderAsync();

                if (dr.HasRows)
                {
                    dr.Read();
                    string s = dr[0].ToString();
                    if(s.StartsWith("Error:"))
                    {
                        throw new Exception(s);
                    }
                    return dr[0].ToString();

                }


                return "No Results";

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public async Task<string> GetJsonResultsString(string sqlConnectionString, string storedProcedure)
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
                        if(dr.HasRows)
                        {
                            dr.Read();
                            return dr[0].ToString();
                            
                        }


                        return "No Results";
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

        public async Task ExecSP(SqlConnection sqlConnection, SqlCommand sqlCmd)
        {
            await sqlConnection.OpenAsync();
            SqlDataReader dr = await sqlCmd.ExecuteReaderAsync();

        }
    }
}
