using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SP = Microsoft.SharePoint.Client;
using System.Security.Cryptography;
using System.Web.Script.Serialization;
using System.IO;
using Edocs.Encrypt.Decrypt;
using System.Security;
using EDL = Edocs.Logger.Logger;
namespace Edocs.Upload.SharePoint
{
    public class UpLoadSharePoint
    {
        const string SharePointAuditLogsDocumentsUrl = "SharePointAuditLogsDocumentsUrl";
        const string SharePointDocumentsLibraryTitle = "SharePointDocumentsLibraryTitle";
        const string SharePointAuditUrl = "SharePointAuditUrl";
        const string AuditLogsFolder = "AuditLogsFolder";
        const string SharePointAuditListName = "SharePointAuditListName";
        const string FileCreateDate = "FileCreateDate";
        const string UploadFileName = "UploadFileName";
        const string CreateBy = "CreateBy";
        const string ProcessName = "ProcessName";
        private readonly string UploadFileExtension = "csv";
        private string spUserName;
        private SecureString spPassWord;

        private string SharePointJsonFile
        { get; set; }

        public UpLoadSharePoint(string userNameSP, string passWordSp, string passwordKey, DataProtectionScope scope)
        {
            EDL.LogInstance.WriteLoggingLogFile("Getting sharepoint user information", false, Logger.LoggingErrorType.Info);
            if ((string.IsNullOrWhiteSpace(userNameSP)) || (string.IsNullOrWhiteSpace(passWordSp)))
            {
                EDL.LogInstance.WriteLoggingLogFile("Either sharepoint username or sharepoint password is empty", false, Logger.LoggingErrorType.Error);
                throw new Exception("Either sharepoint username or sharepoint password is empty");
            }

            if (!(string.IsNullOrWhiteSpace(passwordKey)))
            {
                EDL.LogInstance.WriteLoggingLogFile("Decrypting username password", false, Logger.LoggingErrorType.Info);
                Encrypt_Decrypt.EncryptDecryptKey = passwordKey.Trim();
                spPassWord = GetPassWord(Encrypt_Decrypt.DecryptToString(passWordSp, scope));
                spUserName = Encrypt_Decrypt.DecryptToString(userNameSP, scope);
            }
            else
            {
                spUserName = userNameSP;
                spPassWord = GetPassWord(passWordSp);
            }
        }

        private SecureString GetPassWord(string passWord)
        {
            var securePW = new SecureString();
            foreach (char c in passWord)
            {
                securePW.AppendChar(c);
            }
            return securePW;
        }
        public void UploadAuditFile(string jsonFileName)
        {
            EDL.LogInstance.WriteLoggingLogFile($"Uploading information to sharepoint using json file:{jsonFileName}", false, Logger.LoggingErrorType.Info);
            if (!(File.Exists(jsonFileName)))
            {
                EDL.LogInstance.WriteLoggingLogFile($"Cannout upload to sharepoint jason file:{jsonFileName} not found", false, Logger.LoggingErrorType.Error);
                throw new Exception($"Cannout upload to sharepoint jason file:{jsonFileName} not found");
            }


            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            List<Dictionary<string, object>> records = new List<Dictionary<string, object>>();
            //   Dictionary<string, object> batchSettings = (Dictionary<string, object>)(jsonSerializer.DeserializeObject(File.ReadAllText(jsonFileName)));
            foreach (string line in File.ReadAllLines(jsonFileName))
            {
                if (!(string.IsNullOrEmpty(line)))
                {
                    records.Add((Dictionary<string, object>)(jsonSerializer.DeserializeObject(line)));
                }
            }

            foreach (Dictionary<string, object> record in records)
            {
                UploadAuditFiles(record);
            }
        }
        private void UploadAuditFiles(Dictionary<string, object> auditInfo)
        {
            if (!(DateTime.TryParse(auditInfo[FileCreateDate].ToString(), out DateTime result)))
            {
                throw new Exception($"Invalid file create time {auditInfo[FileCreateDate]}");
            }
            string docFolderName = result.ToString("yyyy");
            EDL.LogInstance.WriteLoggingLogFile($"Uploading information to sharepoint using sharepoint url:{auditInfo[SharePointAuditLogsDocumentsUrl].ToString()}", false, Logger.LoggingErrorType.Info);
            using (SP.ClientContext filesContext = new SP.ClientContext(auditInfo[SharePointAuditLogsDocumentsUrl].ToString()))
            {
                try
                {
                    filesContext.RequestTimeout = 300000;
                    filesContext.Credentials = new SP.SharePointOnlineCredentials(spUserName, spPassWord);
                    SP.Web filesWeb = filesContext.Web;
                    SP.List filesList = GetListByTitle(filesContext, auditInfo[SharePointDocumentsLibraryTitle].ToString());
                    filesContext.ExecuteQuery();
                    SP.Folder filesFolder = CreateSubfolders(filesContext, filesList, result.ToString("yyyy") + "/" + result.ToString("MM"));
                    filesContext.Load(filesFolder, f => f.ServerRelativeUrl);
                    filesContext.ExecuteQuery();
                    string spCSVFile = Path.ChangeExtension(Path.GetFileName(auditInfo[UploadFileName].ToString()), UploadFileExtension);
                    EDL.LogInstance.WriteLoggingLogFile($"Uploading  file:{spCSVFile} to sharepoint using sharepoint url:{auditInfo[SharePointAuditLogsDocumentsUrl].ToString()}", false, Logger.LoggingErrorType.Info);
                    byte[] csvData = File.ReadAllBytes(auditInfo[UploadFileName].ToString());
                    SP.File newCvsFile;
                    using (MemoryStream csvStream = new MemoryStream(csvData))
                    {
                        newCvsFile = filesFolder.Files.Add(new SP.FileCreationInformation()
                        {

                            ContentStream = csvStream,
                            Url = spCSVFile,
                            Overwrite = true
                        });
                        
                        filesContext.Load(newCvsFile, f => f.ServerRelativeUrl);
                        filesContext.ExecuteQuery();
                    }
                    EDL.LogInstance.WriteLoggingLogFile($"Adding:{newCvsFile.Name} to sharepoint url:{newCvsFile.ServerRelativeUrl} using sharepoint url:{auditInfo[SharePointAuditLogsDocumentsUrl].ToString()}", false, Logger.LoggingErrorType.Info);
                    AddToSharePointList(auditInfo, newCvsFile.ServerRelativeUrl);

                }
                catch (Exception ex)
                {
                    EDL.LogInstance.WriteLoggingLogFile($"Creating folder for sharepoint url:{auditInfo[SharePointAuditLogsDocumentsUrl].ToString()} {ex.Message}", false, Logger.LoggingErrorType.Error);
                    throw new Exception($"Creating folder for sharepoint url:{auditInfo[SharePointAuditLogsDocumentsUrl].ToString()} {ex.Message}");
                }

            }
        }






        private void AddToSharePointList(Dictionary<string, object> auditInfo, string fileUrl)
        {
            if (!(DateTime.TryParse(auditInfo[FileCreateDate].ToString(), out DateTime result)))
            {
                EDL.LogInstance.WriteLoggingLogFile($"Invalid  file create date time  to sharepoint list:{auditInfo[FileCreateDate].ToString()}", false, Logger.LoggingErrorType.Error);
                throw new Exception($"Invalid file create time {auditInfo[FileCreateDate]}");
            }
            string spCSVFile = Path.ChangeExtension(Path.GetFileName(auditInfo[UploadFileName].ToString()), UploadFileExtension);
            EDL.LogInstance.WriteLoggingLogFile($"Uploading  to sharepoint list:{auditInfo[SharePointAuditUrl].ToString()}", false, Logger.LoggingErrorType.Info);
            using (SP.ClientContext mDataListContext = new SP.ClientContext(auditInfo[SharePointAuditUrl].ToString()))
            {
                mDataListContext.RequestTimeout = 300000;
                mDataListContext.Credentials = new SP.SharePointOnlineCredentials(spUserName, spPassWord);
                mDataListContext.ExecuteQuery();



                SP.List mDataList = GetListByTitle(mDataListContext, auditInfo[SharePointAuditListName].ToString());
                //mDataContext.Load(mDataList);
                mDataListContext.ExecuteQuery();
                EDL.LogInstance.WriteLoggingLogFile($"Sharepoint list root folder:{mDataList.RootFolder}", false, Logger.LoggingErrorType.Info);



                //SP.FieldCollection fieldColl = mDataList.Fields;
                //mDataContext.Load(fieldColl);
                //mDataContext.ExecuteQuery();
                //StreamWriter sw = new StreamWriter(@"C:\Archives\spfields.txt", false);
                //foreach (SP.Field f in fieldColl)
                //{
                //    Console.WriteLine(f.StaticName.ToString() + " - " + f.TypeDisplayName.ToString() + "\n");
                //    sw.WriteLine((f.StaticName.ToString() + " - " + f.TypeDisplayName.ToString() + "\n"));
                //}
                //sw.Flush();
                //sw.Close();
                SP.Folder mDataFolder = CreateSubfolders(mDataListContext, mDataList, result.ToString("/yyyy/MM"));
                mDataListContext.Load(mDataFolder, f => f.ServerRelativeUrl);
                mDataListContext.ExecuteQuery();

                SP.CamlQuery query = new SP.CamlQuery();
                query.ViewXml =
                    "<View>"
                    + "<Query>"
                    + "</Query>"
                    + "<ViewFields>"
                    // + "<FieldRef Name='File_x0020_Url'/>"
                    + "<FieldRef Name='FileUrl'/>"
                    + "</ViewFields>"
                    + "</View>";

                query.FolderServerRelativeUrl = mDataFolder.ServerRelativeUrl;
                EDL.LogInstance.WriteLoggingLogFile($"Running sharepoint query:{query.ViewXml} for server relative url:{query.FolderServerRelativeUrl} ", false, Logger.LoggingErrorType.Info);
                SP.ListItemCollection mDataFolderItems = mDataList.GetItems(query);

                mDataListContext.ExecuteQuery();
                mDataListContext.LoadQuery(mDataFolderItems);
                mDataListContext.Load(mDataFolderItems, items => items);
                mDataListContext.ExecuteQuery();

                List<string> ExistingUrls = new List<string>();
                foreach (SP.ListItem item in mDataFolderItems)
                {
                    // string url = ((SP.FieldUrlValue)(item["File_x0020_Url"])).Url;
                    string url = ((SP.FieldUrlValue)(item["FileUrl"])).Url;
                    ExistingUrls.Add(url.Substring(url.LastIndexOf('/') + 1));

                }
                if (!(ExistingUrls.Contains(spCSVFile)))
                {
                    // foreach (SP.Field f in mDataList.Fields)
                    // {
                    //  Console.WriteLine(f.StaticName.ToString() + " - " + f.TypeDisplayName.ToString() + "\n");
                    // }
                    SP.ListItem mItem = mDataList.AddItem(new SP.ListItemCreationInformation()
                    {
                        FolderUrl = mDataFolder.ServerRelativeUrl
                        ,
                        UnderlyingObjectType = SP.FileSystemObjectType.File
                    });

                    //  SP.ListItemCreationInformation oListItem = new SP.ListItemCreationInformation();
                    // SP.ListItem mItem = mDataList.AddItem(oListItem);

                    //  mItem["Audit_x0020_Log_x0020_File"] = new SP.FieldUrlValue() { Description = $"{spCSVFile}", Url = newCvsFile.ServerRelativeUrl };
                    mItem["FileUrl"] = new SP.FieldUrlValue() { Description = "File", Url = fileUrl };
                    mItem["AuditLogFile"] = spCSVFile;
                    mItem["Audit_x0020_Log_x0020_Date"] = result.Date.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
                    mItem["Process_x0020_Name"] = auditInfo[ProcessName];
                    mItem["User_x0020_Uploaded_x0020_Audit_"] = auditInfo[CreateBy];
                    try
                    {
                        mItem.Update();
                        mDataListContext.ExecuteQuery();
                    }
                    catch (Exception ex)
                    {
                        EDL.LogInstance.WriteLoggingLogFile($"Adding  to sharepoint list {ex.Message}", false, Logger.LoggingErrorType.Error);
                        throw new Exception($"Adding  to sharepoint list {ex.Message}");
                    }
                }
                else
                    EDL.LogInstance.WriteLoggingLogFile($"File all ready exists:{spCSVFile} in list", false, Logger.LoggingErrorType.Warning);

            }
        }
        private SP.List GetListByTitle(SP.ClientContext clientContext, string title)
        {

            EDL.LogInstance.WriteLoggingLogFile($"Getting  to sharepoint list name using titl{title} for sharepint url:{clientContext.Url}", false, Logger.LoggingErrorType.Info);

            SP.Web web = clientContext.Web;
            SP.ListCollection lists = web.Lists;

            IEnumerable<SP.List> existingLists = clientContext.LoadQuery(
                     lists.Where(l => l.Title == title));
            clientContext.ExecuteQuery();
            return existingLists.FirstOrDefault();
        }

        private SP.Folder CreateSubfolders(SP.ClientContext context, SP.List list, string listRelativeUrl)
        {
            EDL.LogInstance.WriteLoggingLogFile($"Creating folder for sharepoint list:{list.RootFolder} for relative url{listRelativeUrl} for sharepint url:{context.Url}", false, Logger.LoggingErrorType.Info);
            SP.Folder currentFolder = list.RootFolder;


            foreach (string folderName in listRelativeUrl.Split('/'))
            {

                if (string.IsNullOrWhiteSpace(folderName))
                {

                    continue;
                }

                context.Load(currentFolder, f => f.ServerRelativeUrl);
                context.ExecuteQuery();

                SP.Folder subFolder = GetSubFolderByName(context, currentFolder, folderName);
                if (subFolder == null)
                {
                    EDL.LogInstance.WriteLoggingLogFile($"Creating folder:{folderName} for  url:{context.Url}", false, Logger.LoggingErrorType.Info);

                    SP.ListItem subFolderItem = list.AddItem(new SP.ListItemCreationInformation()
                    {
                        LeafName = folderName,
                        FolderUrl = currentFolder.ServerRelativeUrl,
                        UnderlyingObjectType = SP.FileSystemObjectType.Folder
                    });
                    subFolderItem["Title"] = folderName;
                    subFolderItem.Update();
                    context.Load(subFolderItem.Folder);
                    context.ExecuteQuery();
                    currentFolder = subFolderItem.Folder;
                }
                else
                {

                    currentFolder = subFolder;
                }

            }
            EDL.LogInstance.WriteLoggingLogFile($"Creating folder:{currentFolder} for  url:{context.Url}", false, Logger.LoggingErrorType.Info);
            return currentFolder;
        }

        private SP.Folder GetSubFolderByName(SP.ClientContext context, SP.Folder folder, string name)
        {

            EDL.LogInstance.WriteLoggingLogFile($"Get folder name:{name} for url:{context.Url}", false, Logger.LoggingErrorType.Info);
            SP.FolderCollection folders = folder.Folders;

            IEnumerable<SP.Folder> existingFolders = context.LoadQuery<SP.Folder>(
                folders.Where(fldr => fldr.Name == name));
            context.ExecuteQuery();
            return existingFolders.FirstOrDefault();
        }

    }
}
