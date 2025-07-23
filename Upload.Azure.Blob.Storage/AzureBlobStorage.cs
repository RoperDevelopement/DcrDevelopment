using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.Storage.Auth;
using System.IO;

namespace Edocs.Upload.Azure.Blob.Storage
{
    

    public class AzureBlobStorage
    {
        private static AzureBlobStorage instance = null;

        public string AzureBlobAccountName
        { get; set; }
        public string AzureBlobAccountKey
        { get; set; }

        public string AzureBlobStorageConnectionString
        { get; set; }
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

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<bool> CheckAzureConnection()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            try
            {
                GetAzureStorageCredentials().ConfigureAwait(false).GetAwaiter().GetResult();

            }
            catch (Exception ex)
            { throw new Exception(ex.Message); }

            return true;


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

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        private async Task<CloudBlobContainer> GetCloudBlobContainer(string azureContainer)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            StorageCredentials storageCredentials = GetAzureStorageCredentials().ConfigureAwait(false).GetAwaiter().GetResult();
            CloudStorageAccount cloudStorageAccount = GetCloudStorageAccount(storageCredentials).ConfigureAwait(false).GetAwaiter().GetResult();
            CloudBlobClient blobClient = GetCloudBlobClient(cloudStorageAccount).ConfigureAwait(false).GetAwaiter().GetResult();
            CloudBlobContainer container = GetCloudBlobContainer(blobClient, azureContainer).ConfigureAwait(false).GetAwaiter().GetResult();
            return blobClient.GetContainerReference(azureContainer);
        }
        public async Task<Uri> UploadAzureBlob(string upLoadFileName, string azureContainer)
        {
            Uri uri = null;
            try
            {

                CloudBlobContainer container = GetCloudBlobContainer(azureContainer).ConfigureAwait(false).GetAwaiter().GetResult();
                CloudBlockBlob cloudBlockBlob = GetCloudBlockBlob(container, $"{Path.GetFileName(upLoadFileName)}").ConfigureAwait(false).GetAwaiter().GetResult();

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
            catch (Exception ex)
            {
                throw new Exception($"Uploading file {upLoadFileName} to azure blob storage using AzureBlobStorageConnectionString {AzureBlobStorageConnectionString} - AzureBlobAccountName {AzureBlobAccountName} - AzureBlobAccountKey {AzureBlobAccountKey} {ex.Message}");
            }
            return uri;
        }

        public void WriteAzureBlob(string fileName, string azureContainer, string text)
        {
            Uri uri = null;
            try
            {


                CloudBlobContainer container = GetCloudBlobContainer(azureContainer).ConfigureAwait(false).GetAwaiter().GetResult();
                CloudBlockBlob cloudBlockBlob = GetCloudBlockBlob(container, fileName).ConfigureAwait(false).GetAwaiter().GetResult();
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
            catch (Exception ex)
            {
                throw new Exception($"Error writing to file {fileName} to azure blob storage using AzureBlobStorageConnectionString {AzureBlobStorageConnectionString} - AzureBlobAccountName {AzureBlobAccountName} - AzureBlobAccountKey {AzureBlobAccountKey} {ex.Message}");
            }

        }

        public async Task<byte[]> DownloadFileBytesAzureBlob(string fileName, string azureContainer)
        {

            try
            {

                CloudBlobContainer container = GetCloudBlobContainer(azureContainer).ConfigureAwait(false).GetAwaiter().GetResult();
                CloudBlockBlob cloudBlockBlob = GetCloudBlockBlob(container, fileName).ConfigureAwait(false).GetAwaiter().GetResult();

                bool fileExists = cloudBlockBlob.ExistsAsync().GetAwaiter().GetResult();

                if (fileExists)
                {
                    long fileLengthBytes = cloudBlockBlob.Properties.Length;
                    byte[] retBytes = new byte[fileLengthBytes];
                    int downLoadedBytes = cloudBlockBlob.DownloadRangeToByteArrayAsync(retBytes, 0, 0, fileLengthBytes).GetAwaiter().GetResult();
                    if (fileLengthBytes == downLoadedBytes)
                        return retBytes;
                    throw new Exception($"Downloaded bytes {downLoadedBytes.ToString()} not equal to file bytes {fileLengthBytes.ToString()} for url{cloudBlockBlob.Uri.ToString()}");
                }


            }
            catch (Exception ex)
            {
                throw new Exception($"Uploading file {fileName} azure container {azureContainer} to azure blob storage using AzureBlobStorageConnectionString {AzureBlobStorageConnectionString} - AzureBlobAccountName {AzureBlobAccountName} - AzureBlobAccountKey {AzureBlobAccountKey} {ex.Message}");
            }
            return null;
        }

        public async Task<string> DownloadTextFileAzureAppendBlob(string fileName, string azureContainer)
        {

            try
            {

                StorageCredentials storageCredentials = GetAzureStorageCredentials().ConfigureAwait(false).GetAwaiter().GetResult();
                CloudStorageAccount cloudStorageAccount = GetCloudStorageAccount(storageCredentials).ConfigureAwait(false).GetAwaiter().GetResult();
                CloudBlobClient blobClient = GetCloudBlobClient(cloudStorageAccount).ConfigureAwait(false).GetAwaiter().GetResult();
                CloudBlobContainer container = GetCloudBlobContainer(blobClient, azureContainer).ConfigureAwait(false).GetAwaiter().GetResult();
                CloudAppendBlob cloudAppendBlob = GetCloudAppendBlob(container, fileName).ConfigureAwait(false).GetAwaiter().GetResult(); ;
                bool fileExists = cloudAppendBlob.ExistsAsync().GetAwaiter().GetResult();

                if (fileExists)
                {
                    long fileLengthText = cloudAppendBlob.Properties.Length;

                    string retText = cloudAppendBlob.DownloadTextAsync().GetAwaiter().GetResult();
                    if (fileLengthText == retText.Length)
                        return retText;
                    throw new Exception($"Downloaded text {retText.Length.ToString()} not equal to file bytes {fileLengthText.ToString()} for url{cloudAppendBlob.Uri.ToString()}");
                }



            }
            catch (Exception ex)
            {

                throw new Exception($"Uploading file {fileName} azure container {azureContainer} to azure blob storage using AzureBlobStorageConnectionString {AzureBlobStorageConnectionString} - AzureBlobAccountName {AzureBlobAccountName} - AzureBlobAccountKey {AzureBlobAccountKey} {ex.Message}");
            }
            return string.Empty;
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



        public async Task<string> DownloadTextFileAzureBlob(string fileName, string azureContainer)
        {

            try
            {



                StorageCredentials storageCredentials = GetAzureStorageCredentials().ConfigureAwait(false).GetAwaiter().GetResult();
                CloudStorageAccount cloudStorageAccount = GetCloudStorageAccount(storageCredentials).ConfigureAwait(false).GetAwaiter().GetResult(); ;
                CloudBlobClient blobClient = GetCloudBlobClient(cloudStorageAccount).ConfigureAwait(false).GetAwaiter().GetResult();
                CloudBlobContainer container = GetCloudBlobContainer(blobClient, azureContainer).ConfigureAwait(false).GetAwaiter().GetResult(); ;
                CloudBlockBlob cloudBlockBlob = GetCloudBlockBlob(container, fileName).ConfigureAwait(false).GetAwaiter().GetResult(); ;


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
                return DownloadTextFileAzureAppendBlob(fileName, azureContainer).ConfigureAwait(true).GetAwaiter().GetResult();
                throw new Exception($"Uploading file {fileName} azure container {azureContainer} to azure blob storage using AzureBlobStorageConnectionString {AzureBlobStorageConnectionString} - AzureBlobAccountName {AzureBlobAccountName} - AzureBlobAccountKey {AzureBlobAccountKey} {ex.Message}");
            }
            return string.Empty;
        }


        public async Task<string> DownloadBlobStreamAzureBlob(string fileName, string azureContainer)
        {

            try
            {



                StorageCredentials storageCredentials = GetAzureStorageCredentials().ConfigureAwait(false).GetAwaiter().GetResult();
                CloudStorageAccount cloudStorageAccount = GetCloudStorageAccount(storageCredentials).ConfigureAwait(false).GetAwaiter().GetResult();
                CloudBlobClient blobClient = GetCloudBlobClient(cloudStorageAccount).ConfigureAwait(false).GetAwaiter().GetResult();
                CloudBlobContainer container = GetCloudBlobContainer(blobClient, azureContainer).ConfigureAwait(false).GetAwaiter().GetResult();
                CloudBlockBlob cloudBlockBlob = GetCloudBlockBlob(container, fileName).ConfigureAwait(false).GetAwaiter().GetResult();

                Stream file = File.OpenWrite(@"c:\temp\test.txt");


                cloudBlockBlob.DownloadToStream(file);
                bool fileExists = true;// cloudBlockBlob.ExistsAsync().GetAwaiter().GetResult();

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

        public async Task<Uri> UploadAzureBlob(string upLoadFileName, string azureContainer, byte[] uploadByte)
        {
            Uri uri = null;
            try
            {
                StorageCredentials storageCredentials = GetAzureStorageCredentials().ConfigureAwait(false).GetAwaiter().GetResult();
                CloudStorageAccount cloudStorageAccount = GetCloudStorageAccount(storageCredentials).ConfigureAwait(false).GetAwaiter().GetResult();
                CloudBlobClient blobClient = GetCloudBlobClient(cloudStorageAccount).ConfigureAwait(false).GetAwaiter().GetResult();
                CloudBlobContainer container = GetCloudBlobContainer(blobClient, azureContainer).ConfigureAwait(false).GetAwaiter().GetResult();
                CloudBlockBlob cloudBlockBlob = GetCloudBlockBlob(container, $"{Path.GetFileName(upLoadFileName)}").ConfigureAwait(false).GetAwaiter().GetResult();
                uri = cloudBlockBlob.Uri;
                bool fileExists = cloudBlockBlob.ExistsAsync().GetAwaiter().GetResult();

                if (!(fileExists))
                {
                    cloudBlockBlob.UploadFromByteArrayAsync(uploadByte, 0, uploadByte.Count<byte>(), null, null, null).GetAwaiter().GetResult();

                }


            }
            catch (Exception ex)
            {
                throw new Exception($"Uploading file {upLoadFileName} to azure blob storage using AzureBlobStorageConnectionString {AzureBlobStorageConnectionString} - AzureBlobAccountName {AzureBlobAccountName} - AzureBlobAccountKey {AzureBlobAccountKey} {ex.Message}");
            }
            return uri;
        }

        public async Task UploadAzureBlobTextFile(string upLoadFileName, string azureContainer, string contents)
        {

            try
            {



                StorageCredentials storageCredentials = GetAzureStorageCredentials().ConfigureAwait(false).GetAwaiter().GetResult();
                CloudStorageAccount cloudStorageAccount = GetCloudStorageAccount(storageCredentials).ConfigureAwait(false).GetAwaiter().GetResult();
                CloudBlobClient blobClient = GetCloudBlobClient(cloudStorageAccount).ConfigureAwait(false).GetAwaiter().GetResult();
                CloudBlobContainer container = GetCloudBlobContainer(blobClient, azureContainer).ConfigureAwait(false).GetAwaiter().GetResult();
                CloudBlockBlob cloudBlockBlob = GetCloudBlockBlob(container, $"{Path.GetFileName(upLoadFileName)}").ConfigureAwait(false).GetAwaiter().GetResult();





                cloudBlockBlob.UploadTextAsync(contents).GetAwaiter().GetResult();




            }
            catch (Exception ex)
            {
                throw new Exception($"Uploading file {upLoadFileName} to azure blob storage using AzureBlobStorageConnectionString {AzureBlobStorageConnectionString} - AzureBlobAccountName {AzureBlobAccountName} - AzureBlobAccountKey {AzureBlobAccountKey} {ex.Message}");
            }

        }
        public async Task<Uri> UploadAzureBlobTextFile(string contents, string upLoadFileName, string azureContainer, bool overWriteFile)
        {
            Uri uri = null;
            try
            {

                StorageCredentials storageCredentials = GetAzureStorageCredentials().ConfigureAwait(false).GetAwaiter().GetResult();
                CloudStorageAccount cloudStorageAccount = GetCloudStorageAccount(storageCredentials).ConfigureAwait(false).GetAwaiter().GetResult();
                CloudBlobClient blobClient = GetCloudBlobClient(cloudStorageAccount).ConfigureAwait(false).GetAwaiter().GetResult();
                CloudBlobContainer container = GetCloudBlobContainer(blobClient, azureContainer).ConfigureAwait(false).GetAwaiter().GetResult();
                CloudBlockBlob cloudBlockBlob = GetCloudBlockBlob(container, $"{Path.GetFileName(upLoadFileName)}").ConfigureAwait(false).GetAwaiter().GetResult();
               
                uri = cloudBlockBlob.Uri;
                bool fileExists = cloudBlockBlob.ExistsAsync().GetAwaiter().GetResult();

                if ((fileExists) && (!(overWriteFile)))
                    throw new Exception($"File exits: {upLoadFileName} on {azureContainer} and cannot be over written: {overWriteFile.ToString()}");
                cloudBlockBlob.UploadTextAsync(contents).GetAwaiter().GetResult();



            }
            catch (Exception ex)
            {
                throw new Exception($"Uploading file {upLoadFileName} to azure blob storage using AzureBlobStorageConnectionString {AzureBlobStorageConnectionString} - AzureBlobAccountName {AzureBlobAccountName} - AzureBlobAccountKey {AzureBlobAccountKey} {ex.Message}");
            }
            return uri;

        }
        public async Task<string> DownLoadFileAppendBlod(string azureContainer, string azureFileName, string downLoadFolder)
        {
            if (!(Path.HasExtension(downLoadFolder)))
                downLoadFolder= Path.Combine(downLoadFolder, azureFileName);
            try
            {



                StorageCredentials storageCredentials = GetAzureStorageCredentials().ConfigureAwait(false).GetAwaiter().GetResult();
                CloudStorageAccount cloudStorageAccount = GetCloudStorageAccount(storageCredentials).ConfigureAwait(false).GetAwaiter().GetResult();
                CloudBlobClient blobClient = GetCloudBlobClient(cloudStorageAccount).ConfigureAwait(false).GetAwaiter().GetResult();
                CloudBlobContainer container = GetCloudBlobContainer(blobClient, azureContainer).ConfigureAwait(false).GetAwaiter().GetResult();
                CloudAppendBlob cloudBlockBlob = GetCloudAppendBlob(container, azureFileName).ConfigureAwait(false).GetAwaiter().GetResult();

                if (cloudBlockBlob.ExistsAsync().ConfigureAwait(true).GetAwaiter().GetResult())
                {
                    cloudBlockBlob.DownloadToFileAsync(downLoadFolder, FileMode.Append).ConfigureAwait(false).GetAwaiter().GetResult();
                }
                else
                    return string.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception($"Downloading file {downLoadFolder} from azure container {azureContainer} azure file name {azureFileName} to azure blob storage using AzureBlobStorageConnectionString {AzureBlobStorageConnectionString} - AzureBlobAccountName {AzureBlobAccountName} - AzureBlobAccountKey {AzureBlobAccountKey} {ex.Message}");
            }
            return downLoadFolder;
        }
        public async Task<string> DownLoadFileBlockBlod(string azureContainer, string azureFileName, string downLoadFolder)
        {
            if (!(Path.HasExtension(downLoadFolder)))
                downLoadFolder= Path.Combine(downLoadFolder, azureFileName);
            try
            {



                StorageCredentials storageCredentials = GetAzureStorageCredentials().ConfigureAwait(false).GetAwaiter().GetResult();
                CloudStorageAccount cloudStorageAccount = GetCloudStorageAccount(storageCredentials).ConfigureAwait(false).GetAwaiter().GetResult();
                CloudBlobClient blobClient = GetCloudBlobClient(cloudStorageAccount).ConfigureAwait(false).GetAwaiter().GetResult();
                CloudBlobContainer container = GetCloudBlobContainer(blobClient, azureContainer).ConfigureAwait(false).GetAwaiter().GetResult();
                CloudBlockBlob cloudBlockBlob = GetCloudBlockBlob(container, azureFileName).ConfigureAwait(false).GetAwaiter().GetResult();

                if (cloudBlockBlob.ExistsAsync().ConfigureAwait(true).GetAwaiter().GetResult())
                {
                    cloudBlockBlob.DownloadToFileAsync(downLoadFolder, FileMode.Append).ConfigureAwait(false).GetAwaiter().GetResult();
                }
                else
                    return string.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception($"Downloading file {downLoadFolder} from azure container {azureContainer} azure file name {azureFileName} to azure blob storage using AzureBlobStorageConnectionString {AzureBlobStorageConnectionString} - AzureBlobAccountName {AzureBlobAccountName} - AzureBlobAccountKey {AzureBlobAccountKey} {ex.Message}");
            }
            return downLoadFolder;
        }
        public async Task<bool> DeleteAzureBlobFile(string delFileName, string azureContainer)
        {
            bool fileDel = false;
            try
            {
                StorageCredentials storageCredentials = GetAzureStorageCredentials().ConfigureAwait(false).GetAwaiter().GetResult();
                CloudStorageAccount cloudStorageAccount = GetCloudStorageAccount(storageCredentials).ConfigureAwait(false).GetAwaiter().GetResult();
                CloudBlobClient blobClient = GetCloudBlobClient(cloudStorageAccount).ConfigureAwait(false).GetAwaiter().GetResult();
                CloudBlobContainer container = GetCloudBlobContainer(blobClient, azureContainer).ConfigureAwait(false).GetAwaiter().GetResult();
                CloudBlockBlob cloudBlockBlob = GetCloudBlockBlob(container, $"{Path.GetFileName(delFileName)}").ConfigureAwait(false).GetAwaiter().GetResult();
                if (cloudBlockBlob.ExistsAsync().ConfigureAwait(false).GetAwaiter().GetResult())
                    fileDel = cloudBlockBlob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots, null, null, null).ConfigureAwait(false).GetAwaiter().GetResult();
                else
                    throw new Exception($"File not found {delFileName} on azure container {azureContainer}");


            }


            catch (Exception ex)
            {
                throw new Exception($"Deleting file {delFileName} to azure blob storage using AzureBlobStorageConnectionString {AzureBlobStorageConnectionString} - AzureBlobAccountName {AzureBlobAccountName} - AzureBlobAccountKey {AzureBlobAccountKey} {ex.Message}");
            }
            return fileDel;


        }
        public async Task<bool> DeleteAzureAppendBlobFile(string delFileName, string azureContainer)
        {
            bool fileDel = false;
            try
            {
                StorageCredentials storageCredentials = GetAzureStorageCredentials().ConfigureAwait(false).GetAwaiter().GetResult();
                CloudStorageAccount cloudStorageAccount = GetCloudStorageAccount(storageCredentials).ConfigureAwait(false).GetAwaiter().GetResult();
                CloudBlobClient blobClient = GetCloudBlobClient(cloudStorageAccount).ConfigureAwait(false).GetAwaiter().GetResult();
                CloudBlobContainer container = GetCloudBlobContainer(blobClient, azureContainer).ConfigureAwait(false).GetAwaiter().GetResult();
                CloudAppendBlob cloudBlockBlob = GetCloudAppendBlob(container, $"{Path.GetFileName(delFileName)}").ConfigureAwait(false).GetAwaiter().GetResult();
                if (cloudBlockBlob.ExistsAsync().ConfigureAwait(false).GetAwaiter().GetResult())
                    fileDel = cloudBlockBlob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots,null,null,null).ConfigureAwait(false).GetAwaiter().GetResult();
                else
                    throw new Exception($"File not found {delFileName} on azure container {azureContainer}");

            }


            catch (Exception ex)
            {
                throw new Exception($"Deleting file {delFileName} to azure blob storage using AzureBlobStorageConnectionString {AzureBlobStorageConnectionString} - AzureBlobAccountName {AzureBlobAccountName} - AzureBlobAccountKey {AzureBlobAccountKey} {ex.Message}");
            }
            return fileDel;


        }

        public IEnumerable<IListBlobItem> GetAzureBlobFiles(string fileprefix, string azureContainer, bool overWriteFile)
        {


            StorageCredentials storageCredentials = GetAzureStorageCredentials().ConfigureAwait(false).GetAwaiter().GetResult();
            CloudStorageAccount cloudStorageAccount = GetCloudStorageAccount(storageCredentials).ConfigureAwait(false).GetAwaiter().GetResult();
            CloudBlobClient blobClient = GetCloudBlobClient(cloudStorageAccount).ConfigureAwait(false).GetAwaiter().GetResult();
            CloudBlobContainer container = GetCloudBlobContainer(blobClient, azureContainer).ConfigureAwait(false).GetAwaiter().GetResult();
            foreach (var list in container.ListBlobs(fileprefix, true, blobListingDetails: BlobListingDetails.Metadata, null, null))
            {
                // yield return  list as CloudBlockBlob;
                yield return list;

            }

        }
        public IEnumerable<CloudBlockBlob> GetCloudBlockBlobFiles(string fileprefix, string azureContainer)
        {


            StorageCredentials storageCredentials = GetAzureStorageCredentials().ConfigureAwait(false).GetAwaiter().GetResult();
            CloudStorageAccount cloudStorageAccount = GetCloudStorageAccount(storageCredentials).ConfigureAwait(false).GetAwaiter().GetResult();
            CloudBlobClient blobClient = GetCloudBlobClient(cloudStorageAccount).ConfigureAwait(false).GetAwaiter().GetResult();
            CloudBlobContainer container = GetCloudBlobContainer(blobClient, azureContainer).ConfigureAwait(false).GetAwaiter().GetResult();
            
            foreach (var list in container.ListBlobs(fileprefix, true, blobListingDetails: BlobListingDetails.Metadata, null, null))
            {
                if(list is CloudBlockBlob)
                    yield return list as CloudBlockBlob;

            }

        }

        public IEnumerable<CloudAppendBlob> GetCloudAppendBlobFiles(string fileprefix, string azureContainer)
        {


            StorageCredentials storageCredentials = GetAzureStorageCredentials().ConfigureAwait(false).GetAwaiter().GetResult();
            CloudStorageAccount cloudStorageAccount = GetCloudStorageAccount(storageCredentials).ConfigureAwait(false).GetAwaiter().GetResult();
            CloudBlobClient blobClient = GetCloudBlobClient(cloudStorageAccount).ConfigureAwait(false).GetAwaiter().GetResult();
            CloudBlobContainer container = GetCloudBlobContainer(blobClient, azureContainer).ConfigureAwait(false).GetAwaiter().GetResult();

            foreach (var list in container.ListBlobs(fileprefix, true, blobListingDetails: BlobListingDetails.Metadata, null, null))
            {
                if (list is CloudAppendBlob)
                    yield return list as CloudAppendBlob;

            }

        }


    }
}
