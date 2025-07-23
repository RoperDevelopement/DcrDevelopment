using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edocs.DOH.DL.Doc.Utilities
{
  public class Edocs_Constants
    {
      public  const string AzureBlobAccountKey = "XNvT1ZL9JwsPLtI/AXGu8pJ7aGtkcaU5GoF6ibbzJ0RTuU5reUqdquOqJEyohAeNFZDjoAxM/tEVQNyR1g5kCg==";
        public const string AzureBlobStorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=edocsnypstorageprod;AccountKey=XNvT1ZL9JwsPLtI/AXGu8pJ7aGtkcaU5GoF6ibbzJ0RTuU5reUqdquOqJEyohAeNFZDjoAxM/tEVQNyR1g5kCg==;EndpointSuffix=core.windows.net";
        public const string AzureContainer = "doh";
        public const string AzureCloudDBIPAdress = "AzureCloudDB";
        public const string AzureDBUserName = "edocsusa";
        public const string AzureDBPassWord = "6746edocs@";
        // public const string AzureLocalSqlConnectionString = "Server=172.191.7.0;Database=DOH;User=edocsusa;PassWord=6746edocs@;MultipleActiveResultSets=true;Integrated Security=false;Connection Timeout=30;";
           public const string AzureLocalSqlConnectionString = "Server=DCR;Database=DOH;User=edocsusa;PassWord=6746edocs@;MultipleActiveResultSets=true;Integrated Security=false;";
        //  public const string LocalSqlConnectionString = "Server=edocshomeoffice;Database=DOH;User=edocsusa;PassWord=6746edocs@;MultipleActiveResultSets=true;Integrated Security=false;";
         //    public const string AzureLocalSqlConnectionString = "Server=DCR;Database=DOH;User=edocsusa;PassWord=6746edocs@;MultipleActiveResultSets=true;Integrated Security=false;Connection Timeout=30;";
       // public const string AzureLocalSqlConnectionString = "Server=Queens;Database=DOH;User=edocsusa;PassWord=6746edocs@;MultipleActiveResultSets=true;Integrated Security=false;Connection Timeout=30;";
        public const string AzureBlobAccountName = "edocsnypstorageprod";
        public const string EmailServer = "smtp.gmail.com";
        public const string EmailFrom = "AlertSender@edocsusa.com";
        public const string EmailTo = "dan.roper@edocsusa.com";
        public const string EmailCC = "tressa.orizotti@edocsusa.com";
        public const string EmailPassWord = "6746edocs";
        public const int EmailPort = 587;
       public const string EdocsProgFolder = "e-Docs_DOH_Files";
        public const string SpDownLoadDOHDocuments = "[dbo].[sp_DownLoadDOHDocuments]";
        public const string SpUpDateDownLoadDocument = "[dbo].[sp_UpDateDownLoadDocument]";
        public const string Spsp_RejectDOHDocument = "[dbo].[sp_RejectDOHDocument]";
        public const string SpReDownLoadDOHDocuments = "[dbo].[sp_ReDownLoadDOHDocuments]";
        public const string SpChangeDownLoadValue = "[dbo].[sp_ChangeDownLoadValue]";
        
        public const string ParmID = "@ID";
        public const string ParmRejectReason = "@RejectReason";
        public const string ParmUserName = "@UserName";
        
        public const string MSEdge = @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe";
        public const string NotePad = @"NotePad";
        public const string Quoat = "\"";
        public const string Crome = @"C:\Program Files\Google\Chrome\Application\chrome.exe";

    }
}
