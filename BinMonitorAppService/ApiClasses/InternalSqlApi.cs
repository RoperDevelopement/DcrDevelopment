using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BinMonitorAppService.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using BinMonitorAppService.Constants;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Data;
using System.Data.SqlClient;
using BinMonitorAppService.ApiClasses;
using BinMonitorAppService.Logging;
using System.Text.RegularExpressions;
using BinMonitor.BinInterfaces;
using ITfoxtec.Identity.Saml2;
using ITfoxtec.Identity.Saml2.Schemas;
using ITfoxtec.Identity.Saml2.MvcCore;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using System.Security.Authentication;
using BinMonitorAppService.Pages.Identity;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens.Saml2;
using System.Xml;
using Newtonsoft.Json;

namespace BinMonitorAppService.ApiClasses
{
    public class InternalSqlApi
    {

        private static InternalSqlApi instance = null;
        InternalSqlApi()
        {
        }


        public static InternalSqlApi InstanceSqlApi
        {
            get
            {
                if (instance == null)
                    instance = new InternalSqlApi();
                return instance;
            }
        }

        public async Task<IDictionary<string, ReportVolDurModel>> ReportsByVolDur(string spName, string sqlConnectionString, string cwid, DateTime startDate, DateTime endDate)
        {
            IDictionary<string, ReportVolDurModel> retRep = new Dictionary<string, ReportVolDurModel>();
            try
            {


                using (SqlConnection sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(spName, sqlConnection))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.CommandTimeout = 180;
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaCwid, cwid.Trim()));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaBinStatusStartDate, startDate));
                        sqlCmd.Parameters.Add(new SqlParameter(SqlConstants.SpParmaBinStatusEndDate, endDate));


                        using (SqlDataReader dr = JsonBasicApis.JsonInstance.GetJsonResultsDR(sqlConnection, sqlCmd).ConfigureAwait(true).GetAwaiter().GetResult())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    ReportVolDurModel reportVolDur = new ReportVolDurModel();
                                    reportVolDur.CWID = dr["CWID"].ToString();
                                    reportVolDur.TotalVolume = int.Parse(dr["TotalVolume"].ToString());

                                    int ms = int.Parse(dr["TotalDur"].ToString());
                                    TimeSpan t = TimeSpan.FromMilliseconds(ms);
                                    string answer = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms",
                                                            t.Hours,
                                                            t.Minutes,
                                                            t.Seconds,
                                                            t.Milliseconds);
                                    reportVolDur.TotalDur = answer;
                                    retRep.Add(reportVolDur.CWID, reportVolDur);
                                }
                            }
                        }
                        //  foreach (var item in des.CWID)
                        //  {
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting Volume Total Duration reports for cwid {cwid} Report Start Date {startDate} end date {endDate}");
            }
            return retRep;
        }

    }
}
