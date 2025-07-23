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
using System.Text.RegularExpressions;
using BinMonitorAppService.Logging;
namespace BinMonitorAppService.ApiClasses
{
    public class CloseBatches
    {
        private static CloseBatches instance = null;
        public static CloseBatches CloseBatchesInctance
        {
            get
            {
                if (instance == null)
                    instance = new CloseBatches();
                return instance;
            }
        }
        private CloseBatches()
        {
        }

        public async Task CompletBatch(string cwid, string binId, string webApiUrl, ILog log)
        {

            BinLabRecModel recModel = new BinLabRecModel();
            //BinRegProcessModel completeRegModel = new BinRegProcessModel();

            recModel.BinID = binId;
            recModel.BinAssignedTo = cwid;
            recModel.BinProcessStarted = true;
            log.LogInformation($"Method CompletBatch cwid: {cwid} binid:{binId}");
            await BinsInformation.BinsApisInctance.UpDateRegStatusByBinId(webApiUrl, SqlConstants.ApiUpdateBinsByBInID, recModel, log);
        }

        public async Task UpDateBeginProcess(string cwid, string binId, string webApiUrl, ILog log)

        {

            BinLabRecModel recModel = new BinLabRecModel();

            recModel.BinID = binId;
            recModel.BinAssignedTo = cwid;
            recModel.BinProcessStarted = false;
            log.LogInformation($"Method UpDateBeginProcess BinID: {recModel.BinID} BinAssignedTo: {recModel.BinAssignedTo} BinProcessStarted: {recModel.BinProcessStarted.ToString()}");
            await BinsInformation.BinsApisInctance.UpDateRegStatusByBinId(webApiUrl, SqlConstants.ApiUpdateBinsByBInID, recModel, log);
        }
        public async Task UpDateCompleteProcess(string cwid, string binId, string webApiUrl, ILog log)

        {
            BinLabRecModel recModel = new BinLabRecModel();
            recModel.BinID = binId;
            recModel.BinAssignedTo = cwid;
            recModel.BinProcessStarted = true;
            log.LogInformation($"Method UpDateCompleteProcess BinID: {recModel.BinID} BinAssignedTo: {recModel.BinAssignedTo} BinProcessStarted: {recModel.BinProcessStarted.ToString()}");
            await BinsInformation.BinsApisInctance.UpDateRegStatusByBinId(webApiUrl, SqlConstants.ApiUpdateBinsByBInID, recModel, log);
        }



    }
}
