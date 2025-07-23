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
using EDocs.Nyp.LabReqs.AppServices.LabReqsConstants;

namespace Edocs.Upload.Azure.Blob.Storage
{
    public class AzureBlobStorage
    {
        private static AzureBlobStorage instance = null;
        private readonly IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
        IConfiguration configuration = null;

        public string AzureBlobAccountKey
        { get { return configuration.GetSection(ConstNypLabReqs.JsonKeyAzureStorageBlobSettings).GetValue<string>(ConstNypLabReqs.JsonKeyAzureStorageBlobAccountKey).ToString(); } }

        public string AzureBlobAccountName
        { get { return configuration.GetSection(ConstNypLabReqs.JsonKeyAzureStorageBlobSettings).GetValue<string>(ConstNypLabReqs.JsonKeyAzureStorageBlobAccountName).ToString(); } }

        public string AzureBlobStorageConnectionString
        { get { return configuration.GetSection(ConstNypLabReqs.JsonKeyAzureStorageBlobSettings).GetValue<string>(ConstNypLabReqs.JsonKeyAzureBlobStorageConnectionString).ToString(); } }
        public string AzureBlobShareName
        { get { return configuration.GetSection(ConstNypLabReqs.JsonKeyAzureStorageBlobSettings).GetValue<string>(ConstNypLabReqs.JsonKeyAzureBlobShareName).ToString(); } }

        public string AzureBlobShareAuditLogsUpLoad
        { get { return configuration.GetSection(ConstNypLabReqs.JsonKeyAzureStorageBlobSettings).GetValue<string>(ConstNypLabReqs.JsonKeyAzureBlobShareAuditLogsUpLoad).ToString(); } }
        public string AzureBlobShareAuditLogs
        { get { return configuration.GetSection(ConstNypLabReqs.JsonKeyAzureStorageBlobSettings).GetValue<string>(ConstNypLabReqs.JsonKeyAzureBlobShareAuditLogs).ToString(); } }

        public string AzureBlobShareManual
        { get { return configuration.GetSection(ConstNypLabReqs.JsonKeyAzureStorageBlobSettings).GetValue<string>(ConstNypLabReqs.JsonKeyAzureBlobShareManual).ToString(); } }


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
                configurationBuilder.AddJsonFile(ConstNypLabReqs.AppSettingsJsonFileName);
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
        public async Task<StorageCredentials> GetAzureStorageCredentials()
        {
            if (CloudStorageAccount.TryParse(AzureBlobStorageConnectionString, out CloudStorageAccount storageAccount))
            {
                return new StorageCredentials(AzureBlobAccountName, AzureBlobAccountKey);
            }
            else
                throw new Exception($"Could not connect to Azure Blob Storage using connection string {AzureBlobStorageConnectionString}");


        }
        private async Task<CloudStorageAccount> GetCloudStorageAccount(StorageCredentials storageCredentials)
        {
            return new CloudStorageAccount(storageCredentials, useHttps: true);

        }
        private async Task<CloudBlobClient> GetCloudBlobClient(CloudStorageAccount cloudStorageAccount)
        {
            return cloudStorageAccount.CreateCloudBlobClient();
        }

        private async Task<CloudAppendBlob> GetCloudAppendBlob(CloudBlobContainer container, string azureFileName)
        {
            return container.GetAppendBlobReference(azureFileName);
        }


        private async Task<CloudBlockBlob> GetCloudBlockBlob(CloudBlobContainer container, string azureFileName)
        {
            return container.GetBlockBlobReference(azureFileName);
        }
        private async Task<CloudBlobContainer> GetCloudBlobContainer(CloudBlobClient blobClient, string azureContainer)
        {

            return blobClient.GetContainerReference(azureContainer);
        }
        public async Task<string> DownloadTextAzureAppendBlob(string fileName, string azureContainer)
        {

            try
            {

                StorageCredentials storageCredentials = GetAzureStorageCredentials().ConfigureAwait(false).GetAwaiter().GetResult();
                CloudStorageAccount cloudStorageAccount = GetCloudStorageAccount(storageCredentials).ConfigureAwait(false).GetAwaiter().GetResult();
                CloudBlobClient blobClient = GetCloudBlobClient(cloudStorageAccount).ConfigureAwait(false).GetAwaiter().GetResult();
                CloudBlobContainer container = GetCloudBlobContainer(blobClient, azureContainer).ConfigureAwait(false).GetAwaiter().GetResult();
                CloudBlockBlob cloudBlockBlob = GetCloudBlockBlob(container, fileName).ConfigureAwait(false).GetAwaiter().GetResult();
                bool fileExists = cloudBlockBlob.ExistsAsync().GetAwaiter().GetResult();
                
                if (fileExists)
                {
                    long fileLengthText = cloudBlockBlob.Properties.Length;

                    string retText = cloudBlockBlob.DownloadTextAsync().GetAwaiter().GetResult();
                    if (fileLengthText == retText.Length)
                        return retText;
                    throw new Exception($"Downloaded text {retText.Length.ToString()} not equal to file bytes {fileLengthText.ToString()} for url{cloudBlockBlob.Uri.ToString()}");
                }



            }
            catch (Exception ex)
            {

                throw new Exception($"Uploading file {fileName} azure container {azureContainer} to azure blob storage using AzureBlobStorageConnectionString {AzureBlobStorageConnectionString} - AzureBlobAccountName {AzureBlobAccountName} - AzureBlobAccountKey {AzureBlobAccountKey} {ex.Message}");
            }
            return string.Empty;
        }

        public async Task<MemoryStream> DownloadTextMemoryStreamAzureAppendBlob(string fileName, string azureContainer)
        {

            try
            {

                StorageCredentials storageCredentials = GetAzureStorageCredentials().ConfigureAwait(false).GetAwaiter().GetResult();
                CloudStorageAccount cloudStorageAccount = GetCloudStorageAccount(storageCredentials).ConfigureAwait(false).GetAwaiter().GetResult();
                CloudBlobClient blobClient = GetCloudBlobClient(cloudStorageAccount).ConfigureAwait(false).GetAwaiter().GetResult();
                CloudBlobContainer container = GetCloudBlobContainer(blobClient, azureContainer).ConfigureAwait(false).GetAwaiter().GetResult();
                CloudBlockBlob cloudBlockBlob = GetCloudBlockBlob(container, fileName).ConfigureAwait(false).GetAwaiter().GetResult();
                bool fileExists = cloudBlockBlob.ExistsAsync().GetAwaiter().GetResult();

                if (fileExists)
                {
                    long fileLengthText = cloudBlockBlob.Properties.Length;
                    MemoryStream memstream = new MemoryStream();
                    cloudBlockBlob.DownloadToStreamAsync(memstream).GetAwaiter().GetResult();
                    if (fileLengthText == memstream.Length)
                        return memstream;
                    throw new Exception($"Downloaded text {memstream.Length.ToString()} not equal to file bytes {fileLengthText.ToString()} for url{cloudBlockBlob.Uri.ToString()}");
                }



            }
            catch (Exception ex)
            {

                throw new Exception($"Uploading file {fileName} azure container {azureContainer} to azure blob storage using AzureBlobStorageConnectionString {AzureBlobStorageConnectionString} - AzureBlobAccountName {AzureBlobAccountName} - AzureBlobAccountKey {AzureBlobAccountKey} {ex.Message}");
            }
            return null;
        }
    }
}
