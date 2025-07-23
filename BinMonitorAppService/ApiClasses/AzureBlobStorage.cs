using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 
using System.Threading.Tasks;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Extensions.Configuration;
using System.IO;
using BinMonitorAppService.Constants;

namespace BinMonitorAppService.ApiClasses
{
    public class AzureBlobStorage
    {
        private static AzureBlobStorage instance = null;
        private readonly IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
        IConfiguration configuration = null;

        public string AzureBlobAccountKey
        { get { return configuration.GetSection(SqlConstants.JsonKeyAzureStorageBlobSettings).GetValue<string>(SqlConstants.JsonKeyAzureStorageBlobAccountKey).ToString(); } }

        public string AzureBlobAccountName
        { get { return configuration.GetSection(SqlConstants.JsonKeyAzureStorageBlobSettings).GetValue<string>(SqlConstants.JsonKeyAzureStorageBlobAccountName).ToString(); } }

        public string AzureBlobStorageConnectionString
        { get { return configuration.GetSection(SqlConstants.JsonKeyAzureStorageBlobSettings).GetValue<string>(SqlConstants.JsonKeyAzureBlobStorageConnectionString).ToString(); } }
        public string AzureBlobShareName
        { get { return configuration.GetSection(SqlConstants.JsonKeyAzureStorageBlobSettings).GetValue<string>(SqlConstants.JsonKeyAzureBlobShareName).ToString(); } }

        public string AzureBlobShareAuditLogsUpLoad
        { get { return configuration.GetSection(SqlConstants.JsonKeyAzureStorageBlobSettings).GetValue<string>(SqlConstants.JsonKeyAzureBlobShareAuditLogsUpLoad).ToString(); } }
        public string AzureBlobShareAuditLogs
        { get { return configuration.GetSection(SqlConstants.JsonKeyAzureStorageBlobSettings).GetValue<string>(SqlConstants.JsonKeyAzureBlobShareAuditLogs).ToString(); } }

        public string AzureBlobShareManual
        { get { return configuration.GetSection(SqlConstants.JsonKeyAzureStorageBlobSettings).GetValue<string>(SqlConstants.JsonKeyAzureBlobShareManual).ToString(); } }


        AzureBlobStorage()
        {
        }


        public static AzureBlobStorage BlobStorageInstance
        {
            get
            {
                if (instance == null)
                    instance = new AzureBlobStorage();
                return instance;
            }
        }

        public void InitIConfiguration()
        {
            if (configuration == null)
            { 
                configurationBuilder.AddJsonFile(SqlConstants.AppSettingsJsonFileName);
            configuration = configurationBuilder.Build();
            }
        }
        public async Task<Uri> UploadAzureBlob(string upLoadFileName, string azureContainer)
        {
            Uri uri = null;
            try
            {
               
                    InitIConfiguration();

                if (CloudStorageAccount.TryParse(AzureBlobStorageConnectionString, out CloudStorageAccount storageAccount))
                {
                    StorageCredentials storageCredentials = new StorageCredentials(AzureBlobAccountName, AzureBlobAccountKey);
                    CloudStorageAccount cloudStorageAccount = new CloudStorageAccount(storageCredentials, useHttps: true);
                    CloudBlobClient blobClient = cloudStorageAccount.CreateCloudBlobClient();
                    CloudBlobContainer container = blobClient.GetContainerReference(azureContainer);
                    CloudBlockBlob cloudBlockBlob = container.GetBlockBlobReference($"{Path.GetFileName(upLoadFileName)}");
                    uri = cloudBlockBlob.Uri;
                    bool fileExists = cloudBlockBlob.ExistsAsync().GetAwaiter().GetResult();
                    if (!(fileExists))
                    {
                        using (var fileStream = System.IO.File.OpenRead(upLoadFileName))
                        {
                            
                            cloudBlockBlob.UploadFromStreamAsync(fileStream).GetAwaiter().GetResult();
                        }
                    }

                }
                else
                    throw new Exception("Could not connect to Azure Blob Storage");
            }
            catch (Exception ex)
            {
                throw new Exception($"Uploading file {upLoadFileName} to azure blob storage using AzureBlobStorageConnectionString {AzureBlobStorageConnectionString} - AzureBlobAccountName {AzureBlobAccountName} - AzureBlobAccountKey {AzureBlobAccountKey} {ex.Message}");
            }
            return uri;
        }

        public void  WriteAzureBlob(string fileName, string azureContainer,string text)
        {
            Uri uri = null;
            try
            {
                
                    InitIConfiguration();

            
                if (CloudStorageAccount.TryParse(AzureBlobStorageConnectionString, out CloudStorageAccount storageAccount))
                {
                    StorageCredentials storageCredentials = new StorageCredentials(AzureBlobAccountName, AzureBlobAccountKey);
                    CloudStorageAccount cloudStorageAccount = new CloudStorageAccount(storageCredentials, useHttps: true);
                    CloudBlobClient blobClient = cloudStorageAccount.CreateCloudBlobClient();
                    CloudBlobContainer container = blobClient.GetContainerReference(azureContainer);
                    CloudBlockBlob cloudBlockBlob = container.GetBlockBlobReference(fileName);
                    uri = cloudBlockBlob.Uri;
                    bool fileExists = cloudBlockBlob.ExistsAsync().GetAwaiter().GetResult();
                    if (!(fileExists))
                    {

                        using (StreamWriter sw = new StreamWriter(cloudBlockBlob.OpenWrite()))
                        {

                            sw.WriteLineAsync(text).GetAwaiter().GetResult();
                        }
                    }

                }
                else
                    throw new Exception("Could not connect to Azure Blob Storage");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error writing to file {fileName} to azure blob storage using AzureBlobStorageConnectionString {AzureBlobStorageConnectionString} - AzureBlobAccountName {AzureBlobAccountName} - AzureBlobAccountKey {AzureBlobAccountKey} {ex.Message}");
            }
           
        }
        public void WriteAzureBlob(string fileName, string azureContainer, string text,string header)
        {
            
            try
            {

                InitIConfiguration();


                if (CloudStorageAccount.TryParse(AzureBlobStorageConnectionString, out CloudStorageAccount storageAccount))
                {
                    StorageCredentials storageCredentials = new StorageCredentials(AzureBlobAccountName, AzureBlobAccountKey);
                    CloudStorageAccount cloudStorageAccount = new CloudStorageAccount(storageCredentials, useHttps: true);
                    CloudBlobClient blobClient = cloudStorageAccount.CreateCloudBlobClient();
                    CloudBlobContainer container = blobClient.GetContainerReference(azureContainer);
                    CloudBlockBlob cloudBlockBlob = container.GetBlockBlobReference(fileName);
                    CloudAppendBlob appBlob = container.GetAppendBlobReference(fileName);
                    //bool fileExists = cloudBlockBlob.ExistsAsync().GetAwaiter().GetResult();
                    // if (!(fileExists))
                    // {

                    //     using (StreamWriter sw = new StreamWriter(cloudBlockBlob.OpenWrite()))
                    //     {

                    //         sw.WriteLineAsync(header).GetAwaiter().GetResult();
                    //     }
                    // }
                    bool fileExists = appBlob.ExistsAsync().GetAwaiter().GetResult();
                    if (!(fileExists))
                    {
                        appBlob.CreateOrReplace(null, null, null);
                        header += "\r\n";
                        appBlob.AppendTextAsync(header).GetAwaiter().GetResult();

                    }
                    text += "\r\n";
                    appBlob.AppendTextAsync(text).GetAwaiter().GetResult();
                    //appBlob.AppendTextAsync()

                }
                else
                    throw new Exception("Could not connect to Azure Blob Storage");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error writing to file {fileName} to azure blob storage using AzureBlobStorageConnectionString {AzureBlobStorageConnectionString} - AzureBlobAccountName {AzureBlobAccountName} - AzureBlobAccountKey {AzureBlobAccountKey} {ex.Message}");
            }

        }

        public async Task<byte[]> DownloadFileBytesAzureBlob(string fileName, string azureContainer)
        {
            Uri uri = null;
            try
            {

               
                    InitIConfiguration();

                if (CloudStorageAccount.TryParse(AzureBlobStorageConnectionString, out CloudStorageAccount storageAccount))
                {
                    StorageCredentials storageCredentials = new StorageCredentials(AzureBlobAccountName, AzureBlobAccountKey);
                    CloudStorageAccount cloudStorageAccount = new CloudStorageAccount(storageCredentials, useHttps: true);
                    CloudBlobClient blobClient = cloudStorageAccount.CreateCloudBlobClient();
                    CloudBlobContainer container = blobClient.GetContainerReference(azureContainer);
                    CloudBlockBlob cloudBlockBlob = container.GetBlockBlobReference(fileName);
                    
                    bool fileExists = cloudBlockBlob.ExistsAsync().GetAwaiter().GetResult();

                    if (fileExists)
                    {
                        long fileLengthBytes = cloudBlockBlob.Properties.Length;
                        byte[] retBytes = new byte[fileLengthBytes];
                        int downLoadedBytes = cloudBlockBlob.DownloadRangeToByteArrayAsync(retBytes, 0, 0, fileLengthBytes).GetAwaiter().GetResult();
                        if(fileLengthBytes == downLoadedBytes)
                            return retBytes;
                        throw new Exception($"Downloaded bytes {downLoadedBytes.ToString()} not equal to file bytes {fileLengthBytes.ToString()} for url{cloudBlockBlob.Uri.ToString()}");
                    }

                }
                else
                    throw new Exception("Could not connect to Azure Blob Storage");
            }
            catch (Exception ex)
            {
                throw new Exception($"Uploading file {fileName} azure container {azureContainer} to azure blob storage using AzureBlobStorageConnectionString {AzureBlobStorageConnectionString} - AzureBlobAccountName {AzureBlobAccountName} - AzureBlobAccountKey {AzureBlobAccountKey} {ex.Message}");
            }
            return null;
        }

        public async Task<Uri> UploadAzureBlob(string upLoadFileName, string azureContainer,byte[] uploadByte)
        {
            Uri uri = null;
            try
            {
                
                    InitIConfiguration();

                if (CloudStorageAccount.TryParse(AzureBlobStorageConnectionString, out CloudStorageAccount storageAccount))
                {
                    StorageCredentials storageCredentials = new StorageCredentials(AzureBlobAccountName, AzureBlobAccountKey);
                    CloudStorageAccount cloudStorageAccount = new CloudStorageAccount(storageCredentials, useHttps: true);
                    CloudBlobClient blobClient = cloudStorageAccount.CreateCloudBlobClient();
                    CloudBlobContainer container = blobClient.GetContainerReference(azureContainer);
                    CloudBlockBlob cloudBlockBlob = container.GetBlockBlobReference($"{Path.GetFileName(upLoadFileName)}");
                    uri = cloudBlockBlob.Uri;
                      bool fileExists = cloudBlockBlob.ExistsAsync().GetAwaiter().GetResult();
                    
                    if (!(fileExists))
                    {
                        cloudBlockBlob.UploadFromByteArrayAsync(uploadByte, 0, uploadByte.Length, null, null, null).GetAwaiter().GetResult();
                        //using (var stream = new MemoryStream(uploadByte,writable:false)
                        //{

                        //    cloudBlockBlob.UploadFromStreamAsync(stream).GetAwaiter().GetResult();
                        //}
                    }

                }
                else
                    throw new Exception("Could not connect to Azure Blob Storage");
            }
            catch (Exception ex)
            {
                throw new Exception($"Uploading file {upLoadFileName} to azure blob storage using AzureBlobStorageConnectionString {AzureBlobStorageConnectionString} - AzureBlobAccountName {AzureBlobAccountName} - AzureBlobAccountKey {AzureBlobAccountKey} {ex.Message}");
            }
            return uri;
        }
    }
}
