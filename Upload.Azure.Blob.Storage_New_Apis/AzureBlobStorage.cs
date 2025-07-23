using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.IO;
using static System.Net.WebRequestMethods;
using System.Runtime.Remoting.Contexts;

//The latest libraries to interact with the Azure KeyVault service are:

//*https://www.nuget.org/packages/Azure.Security.KeyVault.Keys
//*https://www.nuget.org/packages/Azure.Security.KeyVault.Secrets
//*https://www.nuget.org/packages/Azure.Security.KeyVault.Certificates

//The latest libraries to interact with the Azure Storage service are:

//      *https://www.nuget.org/packages/Azure.Storage.Blobs
//      *https://www.nuget.org/packages/Azure.Storage.Queues/
//      *https://www.nuget.org/packages/Azure.Storage.Files.Shares/

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
        public async Task<BlobServiceClient> ConnectToBlobStorage(string connectionString)
        {
            return new BlobServiceClient(connectionString);
        }
        public async Task<BlobClient> ConnectToBlobStorage(string connectionString, string container, string path)
        {
            return new BlobClient(AzureBlobStorageConnectionString, "auditlogs-prod", "2020-06-22/06_22_2020.csv");
        }
        public async Task<BlobContainerClient> ConnectToBlobContainer(string connectionString, string containerName)
        {
            return new BlobContainerClient(connectionString, containerName);
        }
        public async Task<BlobClient> GetBlobClient(BlobContainerClient blobContainer, string containerName)
        {
            return blobContainer.GetBlobClient(containerName);
        }
        public async Task<BlobContainerClient> GetBlobContainerClient(BlobServiceClient blobService, string containerName)
        {
            return blobService.GetBlobContainerClient(containerName);
        }
        //string IEnumerable<CloudBlockBlob> GetCloudBlockBlobFiles(string fileprefix, string azureContainer)
        //{


        //    StorageCredentials storageCredentials = GetAzureStorageCredentials().ConfigureAwait(false).GetAwaiter().GetResult();
        //    CloudStorageAccount cloudStorageAccount = GetCloudStorageAccount(storageCredentials).ConfigureAwait(false).GetAwaiter().GetResult();
        //    CloudBlobClient blobClient = GetCloudBlobClient(cloudStorageAccount).ConfigureAwait(false).GetAwaiter().GetResult();
        //    CloudBlobContainer container = GetCloudBlobContainer(blobClient, azureContainer).ConfigureAwait(false).GetAwaiter().GetResult();

        //    foreach (var list in container.ListBlobs(fileprefix, true, blobListingDetails: BlobListingDetails.Metadata, null, null))
        //    {
        //        if (list is CloudBlockBlob)
        //            yield return list as CloudBlockBlob;

        //    }

        //}

        //static IEnumerable<IListBlobItem> GetAzureBlobFiles(string fileprefix, string azureContainer, bool overWriteFile)
        //{


        //    StorageCredentials storageCredentials = GetAzureStorageCredentials().ConfigureAwait(false).GetAwaiter().GetResult();
        //    CloudStorageAccount cloudStorageAccount = GetCloudStorageAccount(storageCredentials).ConfigureAwait(false).GetAwaiter().GetResult();
        //    CloudBlobClient blobClient = GetCloudBlobClient(cloudStorageAccount).ConfigureAwait(false).GetAwaiter().GetResult();
        //    CloudBlobContainer container = GetCloudBlobContainer(blobClient, azureContainer).ConfigureAwait(false).GetAwaiter().GetResult();
        //    foreach (var list in container.ListBlobs(fileprefix, true, blobListingDetails: BlobListingDetails.Metadata, null, null))
        //    {
        //        // yield return  list as CloudBlockBlob;
        //        yield return list;

        //    }

        //}
        public IEnumerable<BlockBlobModel> GetCloudAppendBlobFiles(string fileprefix, string azureContainer, bool overWriteFile)
        // static void GetAzureBlobFiles(string fileprefix, string azureContainer)
        {

            BlobServiceClient bsc = ConnectToBlobStorage(AzureBlobStorageConnectionString).ConfigureAwait(false).GetAwaiter().GetResult();
            BlobContainerClient blobContainer = GetBlobContainerClient(bsc, azureContainer).ConfigureAwait(false).GetAwaiter().GetResult();

            foreach (BlobItem blobItem in blobContainer.GetBlobs(BlobTraits.All, BlobStates.All, fileprefix, System.Threading.CancellationToken.None))

            {
                BlockBlobModel blobModel = new BlockBlobModel();
                blobModel.Name = blobItem.Name;
                blobModel.StorageUri = new Uri($"{System.IO.Path.Combine(blobContainer.Uri.ToString(), blobItem.Name)}");

                //   blobModel.Create = blobItem.Properties.CreatedOn.Value.DateTime;
                blobModel.Create = blobItem.Properties.CreatedOn;
                yield return blobModel;


            }

        }

        public IEnumerable<BlockBlobModel> GetCloudBlockBlobFiles(string fileprefix, string azureContainer)
        {
            BlobServiceClient bsc = ConnectToBlobStorage(AzureBlobStorageConnectionString).ConfigureAwait(false).GetAwaiter().GetResult();
            BlobContainerClient blobContainer = GetBlobContainerClient(bsc, azureContainer).ConfigureAwait(false).GetAwaiter().GetResult();

            foreach (BlobItem blobItem in blobContainer.GetBlobs(BlobTraits.All, BlobStates.All, fileprefix, System.Threading.CancellationToken.None))

            {
                BlockBlobModel blobModel = new BlockBlobModel();
                blobModel.Name = blobItem.Name;
                blobModel.StorageUri = new Uri($"{System.IO.Path.Combine(blobContainer.Uri.ToString(), blobItem.Name)}");

                //   blobModel.Create = blobItem.Properties.CreatedOn.Value.DateTime;
                blobModel.Create = blobItem.Properties.CreatedOn;
                yield return blobModel;


            }
        }
        //public IEnumerable<string> (string fileprefix, string azureContainer, string connectionString)
        //// static void GetCloudAppendBlobFiles(string fileprefix, string azureContainer, string connectionString)
        //{
        //    BlobServiceClient bsc = ConnectToBlobStorage(connectionString).ConfigureAwait(false).GetAwaiter().GetResult();
        //    BlobContainerClient blobContainer = GetBlobContainerClient(bsc, azureContainer).ConfigureAwait(false).GetAwaiter().GetResult();

        //    foreach (BlobItem blobItem in blobContainer.GetBlobs(BlobTraits.All, BlobStates.All, fileprefix, System.Threading.CancellationToken.None))
        //    //   foreach (BlobItem blobItem in blobContainer.GetBlobs(BlobTraits.All,BlobStates.All,fileprefix,System.Threading.CancellationToken.None))
        //    {

        //        yield return blobItem.Name;
        //        //Console.WriteLine("\t" + blobItem.Name);
        //    }
        //    //    StorageCredentials storageCredentials = GetAzureStorageCredentials().ConfigureAwait(false).GetAwaiter().GetResult();
        //    //    CloudStorageAccount cloudStorageAccount = GetCloudStorageAccount(storageCredentials).ConfigureAwait(false).GetAwaiter().GetResult();
        //    //    CloudBlobClient blobClient = GetCloudBlobClient(cloudStorageAccount).ConfigureAwait(false).GetAwaiter().GetResult();
        //    //    CloudBlobContainer container = GetCloudBlobContainer(blobClient, azureContainer).ConfigureAwait(false).GetAwaiter().GetResult();

        //    //    foreach (var list in container.ListBlobs(fileprefix, true, blobListingDetails: BlobListingDetails.Metadata, null, null))
        //    //    {
        //    //        if (list is CloudAppendBlob)
        //    //            yield return list as CloudAppendBlob;

        //    //    }

        //}
        public async Task<string> DownloadTextFileAzureBlob(string fileName, string azureContainer)
        {


            try
            {

                BlobContainerClient blobContainer = ConnectToBlobContainer(AzureBlobStorageConnectionString, azureContainer).ConfigureAwait(false).GetAwaiter().GetResult();
                BlobClient blobClient = GetBlobClient(blobContainer, fileName).ConfigureAwait(false).GetAwaiter().GetResult();
                bool fileExists = blobClient.ExistsAsync().GetAwaiter().GetResult();

                if (fileExists)
                {
                    //  long fileLengthText = blobClient.GetProperties(.Properties.Length;

                    //string retText = cloudAppendBlob.DownloadTextAsync().GetAwaiter().GetResult();
                    BlobDownloadResult downloadResult = blobClient.DownloadContentAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                    string retText = downloadResult.Content.ToString();
                    if (retText.Length > 0)
                        return retText;
                    throw new Exception($"Downloaded text {retText.Length.ToString()}");
                }



            }
            catch (Exception ex)
            {

                throw new Exception($"Uploading file {fileName} azure container {azureContainer} to azure blob storage using AzureBlobStorageConnectionString {AzureBlobStorageConnectionString} - AzureBlobAccountName {AzureBlobAccountName} - AzureBlobAccountKey {AzureBlobAccountKey} {ex.Message}");
            }
            return string.Empty;
        }
        public async Task<bool> DeleteAzureBlobFile(string delFileName, string azureContainer)
        {
            bool fileDel = false;
            try
            {
                BlobContainerClient blobContainer = ConnectToBlobContainer(AzureBlobStorageConnectionString, azureContainer).ConfigureAwait(false).GetAwaiter().GetResult();
                BlobClient blobClient = GetBlobClient(blobContainer, delFileName).ConfigureAwait(false).GetAwaiter().GetResult();
                bool fileExists = blobClient.ExistsAsync().GetAwaiter().GetResult();

                if (fileExists)
                    fileDel = blobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots, null, System.Threading.CancellationToken.None).ConfigureAwait(false).GetAwaiter().GetResult();
                else
                    throw new Exception($"File not found {delFileName} on azure container {azureContainer}");


            }


            catch (Exception ex)
            {
                throw new Exception($"Deleting file {delFileName} to azure blob storage using AzureBlobStorageConnectionString {AzureBlobStorageConnectionString} - AzureBlobAccountName {AzureBlobAccountName} - AzureBlobAccountKey {AzureBlobAccountKey} {ex.Message}");
            }
            return fileDel;


        }
        public IEnumerable<BlockBlobModel> GetAzureBlobFiles(string fileprefix, string azureContainer, bool overWriteFile)
        // static void GetAzureBlobFiles(string fileprefix, string azureContainer)
        {

            BlobServiceClient bsc = ConnectToBlobStorage(AzureBlobStorageConnectionString).ConfigureAwait(false).GetAwaiter().GetResult();
            BlobContainerClient blobContainer = GetBlobContainerClient(bsc, azureContainer).ConfigureAwait(false).GetAwaiter().GetResult();

            foreach (BlobItem blobItem in blobContainer.GetBlobs(BlobTraits.All, BlobStates.All, fileprefix, System.Threading.CancellationToken.None))

            {
                BlockBlobModel blobModel = new BlockBlobModel();
                blobModel.Name = blobItem.Name;
                blobModel.StorageUri = new Uri($"{System.IO.Path.Combine(blobContainer.Uri.ToString(), blobItem.Name)}");

                //   blobModel.Create = blobItem.Properties.CreatedOn.Value.DateTime;
                blobModel.Create = blobItem.Properties.CreatedOn;
                yield return blobModel;


            }

        }
        public IEnumerable<BlockBlobModel> GetCloudAppendBlobFiles(string fileprefix, string azureContainer)
        {


            BlobServiceClient bsc = ConnectToBlobStorage(AzureBlobStorageConnectionString).ConfigureAwait(false).GetAwaiter().GetResult();
            BlobContainerClient blobContainer = GetBlobContainerClient(bsc, azureContainer).ConfigureAwait(false).GetAwaiter().GetResult();

            foreach (BlobItem blobItem in blobContainer.GetBlobs(BlobTraits.All, BlobStates.All, fileprefix, System.Threading.CancellationToken.None))

            {
                BlockBlobModel blobModel = new BlockBlobModel();
                blobModel.Name = blobItem.Name;
                blobModel.StorageUri = new Uri($"{System.IO.Path.Combine(blobContainer.Uri.ToString(), blobItem.Name)}");

                blobModel.Create = blobItem.Properties.CreatedOn.Value.DateTime;
                yield return blobModel;


            }

        }
        public async Task<bool> DeleteAzureAppendBlobFile(string delFileName, string azureContainer)
        {
            bool fileDel = false;
            try
            {
                BlobContainerClient blobContainer = ConnectToBlobContainer(AzureBlobStorageConnectionString, azureContainer).ConfigureAwait(false).GetAwaiter().GetResult();
                BlobClient blobClient = GetBlobClient(blobContainer, delFileName).ConfigureAwait(false).GetAwaiter().GetResult();
                bool fileExists = blobClient.ExistsAsync().GetAwaiter().GetResult();

                if (fileExists)
                    fileDel = blobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots, null, System.Threading.CancellationToken.None).ConfigureAwait(false).GetAwaiter().GetResult();
                else
                    throw new Exception($"File not found {delFileName} on azure container {azureContainer}");

            }


            catch (Exception ex)
            {
                throw new Exception($"Deleting file {delFileName} to azure blob storage using AzureBlobStorageConnectionString {AzureBlobStorageConnectionString} - AzureBlobAccountName {AzureBlobAccountName} - AzureBlobAccountKey {AzureBlobAccountKey} {ex.Message}");
            }
            return fileDel;


        }
        public void WriteAzureBlob(string fileName, string azureContainer, string text)
        {
            Uri uri = null;
            try
            {


                BlobContainerClient blobContainer = ConnectToBlobContainer(AzureBlobStorageConnectionString, azureContainer).ConfigureAwait(false).GetAwaiter().GetResult();
                BlobClient blobClient = GetBlobClient(blobContainer, fileName).ConfigureAwait(false).GetAwaiter().GetResult();
                uri = blobClient.Uri;
                bool fileExists = blobClient.ExistsAsync().GetAwaiter().GetResult();
                if (!(fileExists))
                {


                    MemoryStream ms = ConvertStringToStrem(text).ConfigureAwait(false).GetAwaiter().GetResult();
                    blobClient.UploadAsync(ms, false, System.Threading.CancellationToken.None).ConfigureAwait(false).GetAwaiter().GetResult();
                    ms.Dispose();
                }


            }
            catch (Exception ex)
            {
                throw new Exception($"Error writing to file {fileName} to azure blob storage using AzureBlobStorageConnectionString {AzureBlobStorageConnectionString} - AzureBlobAccountName {AzureBlobAccountName} - AzureBlobAccountKey {AzureBlobAccountKey} {ex.Message}");
            }

        }

        public async Task<Uri> UploadAzureBlob(string upLoadFileName, string azureContainer, byte[] uploadByte)
        {
            Uri uri = null;
            try
            {

                BlobContainerClient blobContainer = ConnectToBlobContainer(AzureBlobStorageConnectionString, azureContainer).ConfigureAwait(false).GetAwaiter().GetResult();
                BlobClient blobClient = GetBlobClient(blobContainer, upLoadFileName).ConfigureAwait(false).GetAwaiter().GetResult();
                bool fileExists = blobClient.ExistsAsync().GetAwaiter().GetResult();
                uri = blobClient.Uri;

                if (!(fileExists))
                {
                    using (MemoryStream ms = new MemoryStream(uploadByte.Length))
                    {

                        ms.Write(uploadByte, 0, uploadByte.Length);
                        blobClient.UploadAsync(ms).GetAwaiter().GetResult();
                    }

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



                BlobContainerClient blobContainer = ConnectToBlobContainer(AzureBlobStorageConnectionString, azureContainer).ConfigureAwait(false).GetAwaiter().GetResult();
                BlobClient blobClient = GetBlobClient(blobContainer, upLoadFileName).ConfigureAwait(false).GetAwaiter().GetResult();
                bool fileExists = blobClient.ExistsAsync().GetAwaiter().GetResult();

                if (fileExists)
                {

                }



                MemoryStream ms = ConvertStringToStrem(contents).ConfigureAwait(false).GetAwaiter().GetResult();
                blobClient.UploadAsync(ms, true, System.Threading.CancellationToken.None).ConfigureAwait(false).GetAwaiter().GetResult();
                ms.Dispose();





            }
            catch (Exception ex)
            {
                throw new Exception($"Uploading file {upLoadFileName} to azure blob storage using AzureBlobStorageConnectionString {AzureBlobStorageConnectionString} - AzureBlobAccountName {AzureBlobAccountName} - AzureBlobAccountKey {AzureBlobAccountKey} {ex.Message}");
            }

        }
        internal async Task<MemoryStream> ConvertStringToStrem(string instr)
        {
            MemoryStream memStream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(memStream);
            streamWriter.Write(instr);
            streamWriter.Flush();
            memStream.Position = 0;
            return memStream;
        }
        public async Task<Uri> UploadAzureBlobTextFile(string contents, string upLoadFileName, string azureContainer, bool overWriteFile)
        {
            Uri uri = null;
            try
            {

                BlobContainerClient blobContainer = ConnectToBlobContainer(AzureBlobStorageConnectionString, azureContainer).ConfigureAwait(false).GetAwaiter().GetResult();
                BlobClient blobClient = GetBlobClient(blobContainer, upLoadFileName).ConfigureAwait(false).GetAwaiter().GetResult();
                bool fileExists = blobClient.ExistsAsync().GetAwaiter().GetResult();

                uri = blobClient.Uri;


                if ((fileExists) && (!(overWriteFile)))
                    throw new Exception($"File exits: {upLoadFileName} on {azureContainer} and cannot be over written: {overWriteFile.ToString()}");
                MemoryStream ms = ConvertStringToStrem(contents).ConfigureAwait(false).GetAwaiter().GetResult();
                blobClient.UploadAsync(ms, overWriteFile, System.Threading.CancellationToken.None).ConfigureAwait(false).GetAwaiter().GetResult();
                ms.Dispose();


            }
            catch (Exception ex)
            {
                throw new Exception($"Uploading file {upLoadFileName} to azure blob storage using AzureBlobStorageConnectionString {AzureBlobStorageConnectionString} - AzureBlobAccountName {AzureBlobAccountName} - AzureBlobAccountKey {AzureBlobAccountKey} {ex.Message}");
            }
            return uri;

        }

        public async Task<string> DownloadTextFileAzureAppendBlob(string fileName, string azureContainer)
        {

            try
            {

                BlobContainerClient blobContainer = ConnectToBlobContainer(AzureBlobStorageConnectionString, azureContainer).ConfigureAwait(false).GetAwaiter().GetResult();
                BlobClient blobClient = GetBlobClient(blobContainer, fileName).ConfigureAwait(false).GetAwaiter().GetResult();
                bool fileExists = blobClient.ExistsAsync().GetAwaiter().GetResult();

                if (fileExists)
                {
                    //  long fileLengthText = blobClient.GetProperties(.Properties.Length;

                    //string retText = cloudAppendBlob.DownloadTextAsync().GetAwaiter().GetResult();
                    BlobDownloadResult downloadResult = blobClient.DownloadContentAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                    string retText = downloadResult.Content.ToString();
                    if (retText.Length > 0)
                        return retText;
                    throw new Exception($"Downloaded text {retText.Length.ToString()}");
                }



            }
            catch (Exception ex)
            {

                throw new Exception($"Uploading file {fileName} azure container {azureContainer} to azure blob storage using AzureBlobStorageConnectionString {AzureBlobStorageConnectionString} - AzureBlobAccountName {AzureBlobAccountName} - AzureBlobAccountKey {AzureBlobAccountKey} {ex.Message}");
            }
            return string.Empty;
        }

        //static async Task<string> DownloadBlobStreamAzureBlob(string fileName, string azureContainer)
        //{

        //    try
        //    {



        //        BlobContainerClient blobContainer = ConnectToBlobContainer(AzureBlobStorageConnectionString, azureContainer).ConfigureAwait(false).GetAwaiter().GetResult();
        //        BlobClient blobClient = GetBlobClient(blobContainer, fileName).ConfigureAwait(false).GetAwaiter().GetResult();
        //        bool fileExists = blobClient.ExistsAsync().GetAwaiter().GetResult();

        //        Stream file = File.OpenWrite(@"c:\temp\test.txt");


        //        cloudBlockBlob.DownloadToStream(file);
        //        bool fileExists = true;// cloudBlockBlob.ExistsAsync().GetAwaiter().GetResult();

        //        if (fileExists)
        //        {
        //            long fileLengthText = cloudBlockBlob.Properties.Length;

        //            string retText = cloudBlockBlob.DownloadTextAsync().GetAwaiter().GetResult();
        //            if (fileLengthText == retText.Length)
        //                return retText;
        //            throw new Exception($"Downloaded text {retText.Length.ToString()} not equal to file bytes {fileLengthText.ToString()} for url{cloudBlockBlob.Uri.ToString()}");
        //        }



        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"Uploading file {fileName} azure container {azureContainer} to azure blob storage using AzureBlobStorageConnectionString {AzureBlobStorageConnectionString} - AzureBlobAccountName {AzureBlobAccountName} - AzureBlobAccountKey {AzureBlobAccountKey} {ex.Message}");
        //    }
        //    return string.Empty;
        //}

        //public async Task<StorageCredentials> GetAzureStorageCredentials()
        //{
        //    if (CloudStorageAccount.TryParse(AzureBlobStorageConnectionString, out CloudStorageAccount storageAccount))
        //    {
        //        return new StorageCredentials(AzureBlobAccountName, AzureBlobAccountKey);
        //    }
        //    else
        //        throw new Exception($"Could not connect to Azure Blob Storage using connection string {AzureBlobStorageConnectionString}");


        //}

        public async Task<string> DownLoadFileAppendBlod(string azureContainer, string azureFileName, string downLoadFolder)
        {
            if (!(Path.HasExtension(downLoadFolder)))
                downLoadFolder = Path.Combine(downLoadFolder, azureFileName);
            try
            {



                BlobContainerClient blobContainer = ConnectToBlobContainer(AzureBlobStorageConnectionString, azureContainer).ConfigureAwait(false).GetAwaiter().GetResult();
                BlobClient blobClient = GetBlobClient(blobContainer, azureFileName).ConfigureAwait(false).GetAwaiter().GetResult();
                bool fileExists = blobClient.ExistsAsync().GetAwaiter().GetResult();

                if (blobClient.ExistsAsync().ConfigureAwait(true).GetAwaiter().GetResult())
                {
                    using (var stream = blobClient.OpenReadAsync().ConfigureAwait(false).GetAwaiter().GetResult())
                    {
                        FileStream fileStream = System.IO.File.OpenWrite(downLoadFolder);
                        await stream.CopyToAsync(fileStream);
                        stream.FlushAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                        stream.Close();
                        stream.Dispose();
                        fileStream.FlushAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                        fileStream.Close();
                        fileStream.Dispose();
                    }
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
                downLoadFolder = Path.Combine(downLoadFolder, azureFileName);
            try
            {



                BlobContainerClient blobContainer = ConnectToBlobContainer(AzureBlobStorageConnectionString, azureContainer).ConfigureAwait(false).GetAwaiter().GetResult();
                BlobClient blobClient = GetBlobClient(blobContainer, azureFileName).ConfigureAwait(false).GetAwaiter().GetResult();
                bool fileExists = blobClient.ExistsAsync().GetAwaiter().GetResult();
                if (fileExists)
                {
                    BlobDownloadResult downloadResult = blobClient.DownloadContentAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                    string retText = downloadResult.Content.ToString();
                    if (retText.Length > 0)
                        System.IO.File.WriteAllText(downLoadFolder, retText);

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
        public async Task<byte[]> DownloadFileBytesAzureBlob(string fileName, string azureContainer)
        {

            try
            {
                BlobContainerClient blobContainer = ConnectToBlobContainer(AzureBlobStorageConnectionString, azureContainer).ConfigureAwait(false).GetAwaiter().GetResult();
                BlobClient blobClient = GetBlobClient(blobContainer, fileName).ConfigureAwait(false).GetAwaiter().GetResult();



                bool fileExists = blobClient.ExistsAsync().GetAwaiter().GetResult();

                if (fileExists)
                {
                    MemoryStream memoryStream = new MemoryStream();
                    await blobClient.DownloadToAsync(memoryStream);
                    return memoryStream.ToArray();
                    //   await blobClient.DownloadToAsync(fileStream);

                    //long fileLengthBytes = blobContainer.getProperties.Length;
                    //byte[] retBytes = new byte[fileLengthBytes];
                    //int downLoadedBytes = cloudBlockBlob.DownloadRangeToByteArrayAsync(retBytes, 0, 0, fileLengthBytes).GetAwaiter().GetResult();
                    //if (fileLengthBytes == downLoadedBytes)
                    //    return retBytes;
                    // throw new Exception($"Downloaded bytes {downLoadedBytes.ToString()} not equal to file bytes {fileLengthBytes.ToString()} for url{cloudBlockBlob.Uri.ToString()}");
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
