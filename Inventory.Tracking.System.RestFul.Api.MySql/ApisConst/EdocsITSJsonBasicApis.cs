using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;

 
using Microsoft.Extensions.Configuration;

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using MySql.Data.MySqlClient;

namespace Edocs.Inventory.Tracking.System.RestFul.Api.ApisConst
{
    public class EdocsITSJsonBasicApis
    {
        private static EdocsITSJsonBasicApis instance = null;

        private EdocsITSJsonBasicApis() { }

        public static EdocsITSJsonBasicApis JsonInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EdocsITSJsonBasicApis();
                }
                return instance;
            }
        }
        public async Task ExecSP(MySqlConnection sqlConnection, MySqlCommand sqlCmd)
        {
            await sqlConnection.OpenAsync();
            MySqlDataReader dr = sqlCmd.ExecuteReader();

        }
        public async Task<JsonResult> GetJsonResults(string sqlConnectionString, string storedProcedure)
        {
            try
            {
                using (MySqlConnection sqlConnection = new MySqlConnection(sqlConnectionString))
                {
                    using (MySqlCommand sqlCmd = new MySqlCommand(storedProcedure, sqlConnection))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.CommandTimeout = 180;
                        await sqlConnection.OpenAsync();
                       MySqlDataReader dr =   sqlCmd.ExecuteReader();
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
        public async Task<JsonResult> GetJsonResults(MySqlConnection sqlConnection, MySqlCommand sqlCmd)
        {
            try
            {

                await sqlConnection.OpenAsync();
                MySqlDataReader dr =   sqlCmd.ExecuteReader();

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
        public async Task UploadJsonFile(Object jsonFile, string sqlConnectionStr, string storedProcedueName)
        {
            try
            {
                
                var js = JsonConvert.SerializeObject(jsonFile);



                using (MySqlConnection sqlConnection = new MySqlConnection(sqlConnectionStr))
                {
                    
                    using (MySqlCommand sqlCmd = new MySqlCommand(storedProcedueName, sqlConnection))
                    {
                        
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.Add(new MySqlParameter(EdocsITSConstants.SpParamJsonFile, js));
                        await sqlConnection.OpenAsync();
                        MySqlDataReader dr = sqlCmd.ExecuteReader();

                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
