using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using BinMonitorAppService.Models;
using Newtonsoft;
using Newtonsoft.Json;
using BinMonitorAppService.Constants;
using Microsoft.AspNetCore.Http;
using BinMonitorAppService.Logging;

namespace BinMonitorAppService.ApiClasses
{
    public class TransferApi
    {
        private static TransferApi instance = null;
        public static TransferApi GetTransFerApisIntance
        {
            get
            {
                if (instance == null)
                    instance = new TransferApi();
                return instance;
            }
        }
        private TransferApi()
        {
        }
        public async Task TransFer(TransFerModel transFer, string webApiUri, string webApiControllerName,ILog aLog)
        {
            aLog.LogInformation($"TaskAddUpdateSpeciumBatch url {webApiUri} for web  controller name:{webApiControllerName}");

            try
            {
                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(webApiUri);
                    var jsonString = JsonConvert.SerializeObject(transFer);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    var responseTask = client.PostAsync($"{client.BaseAddress.AbsoluteUri}{webApiControllerName}", content);
                    responseTask.Wait();

                    //  var result = await client.PostAsync("Method Address", content);
                    // string resultContent = await result.Content.ReadAsStringAsync();
                    var results = responseTask.Result;
                //    results.EnsureSuccessStatusCode();
                    var httpResponseBody = await results.Content.ReadAsStringAsync();
                    
                    if (results.IsSuccessStatusCode)
                    {
                        aLog.LogInformation("Retung status code:{results.StatusCode.ToString()} for TaskAddUpdateSpeciumBatch url {webApiUri} for web  controller name:{webApiControllerName}");
                    }
                    else
                    {
                        aLog.LogError($"Return status code:{results.StatusCode.ToString()} TaskAddUpdateSpeciumBatch url {webApiUri} for web  controller name:{webApiControllerName}");

                        throw new Exception($"Return status code:{results.StatusCode.ToString()} TaskAddUpdateSpeciumBatch url {webApiUri} for web  controller name:{webApiControllerName}");
                    }
                }
            }
            catch (AggregateException ae)
            {
                aLog.LogError($"TaskAddUpdateSpeciumBatch url {webApiUri} for web  controller name:{webApiControllerName} message{ae.Message}");
                throw new Exception($"TaskAddUpdateSpeciumBatch url {webApiUri} for web  controller name:{webApiControllerName} message{ae.Message}");
            }
            catch (Exception ex)
            {
                aLog.LogError($"TaskAddUpdateSpeciumBatch url {webApiUri} for web  controller name:{webApiControllerName} message{ex.Message}");
                throw new Exception($"TaskAddUpdateSpeciumBatch url {webApiUri} for web  controller name:{webApiControllerName} message{ex.Message}");
            }
        }
        public async Task TransFerBin(string webApiURl, string oldBinId, string newBinId, string comments, string cwid, ILog log)
        {

            string sp = $"{SqlConstants.WebApiBinMonitor}{SqlConstants.SpGetActiveBinsModelByBinId}/{oldBinId}";
            try
            {
                log.LogInformation($"Method TransFerBin old binid:{oldBinId} new bindId: {newBinId} cwid: {cwid} weburl controller: {sp}");
                BinRegProcessModel ManageBatchModel = await GetApis.GetApisInctance.ApiActiveBinsModel(webApiURl, sp, log);
                string oldBatchId = ManageBatchModel.BatchID.ToString();
                ManageBatchModel.BatchID = Guid.NewGuid();
                ManageBatchModel.BinID = newBinId;
                if (!(string.IsNullOrEmpty(ManageBatchModel.BinComments)))
                {
                    ManageBatchModel.BinComments = $"{ManageBatchModel.BinComments} {comments.Replace("{UserName}", cwid)}";
                }
                else
                    ManageBatchModel.BinComments = comments.Replace("{UserName}", cwid); ;
                TransFerModel transFer = new TransFerModel();
                transFer.BinID = newBinId;
                transFer.OldBinId = oldBinId;
                transFer.Comments = ManageBatchModel.BinComments;
                //  await ApiCreateBatch(ManageBatchModel, log);
                await BinsInformation.BinsApisInctance.TransFer(transFer, webApiURl, SqlConstants.ApiTransFer, log);
                //   await PostApis.PostApisIntance.ApiUpdateTransFromEmailInfo(WebApiUrl, oldBatchId, ManageBatchModel.BatchID.ToString(), log);
            }
            catch (Exception ex)
            {
                log.LogError($"Method TransFerBin old binid:{oldBinId} new bindId: {newBinId} cwid: {cwid} weburl controller: {sp} {ex.Message}");
                throw new Exception($"Method TransFerBin old binid:{oldBinId} new bindId: {newBinId} cwid: {cwid} weburl controller: {sp} {ex.Message}");
            }

        }
    }
}
