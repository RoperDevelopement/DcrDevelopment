using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SP = Microsoft.SharePoint.Client;
using IO = System.IO;

namespace BinMonitor.Common.Sharepoint
{
    /// <summary>
    /// Description of SharepointConnector.
    /// </summary>
    public static class SharepointConnector
    {
        public static async Task<byte[]> RetrieveFile(SP.List list, string camlQuery)
        {
            return await Task.Factory.StartNew<byte[]>(() =>
            {
                Trace.WriteLine("Executing query");
                SP.CamlQuery query = new SP.CamlQuery();
                query.ViewXml = camlQuery;

                SP.ListItemCollection docs = list.GetItems(query);

                list.ParentWeb.Context.Load(docs);
                list.ParentWeb.Context.ExecuteQuery();

                if (docs.Count == 0) throw new Exception("File not found");

                Trace.WriteLine("Downloading file");
                SP.ListItem doc = docs[0];
                //doc.File.CheckOut();
                list.ParentWeb.Context.ExecuteQuery();
                SP.FileInformation fi = SP.File.OpenBinaryDirect((SP.ClientContext)list.ParentWeb.Context, doc["FileRef"].ToString());
                using (IO.MemoryStream stream = new IO.MemoryStream())
                {
                    byte[] buffer = new Byte[8 * 1024];
                    int len;
                    while ((len = fi.Stream.Read(buffer, 0, buffer.Length)) > 0)
                    { stream.Write(buffer, 0, len); }

                    return stream.ToArray();
                }
            });
        }

        public static async Task SendFile(SP.List list, byte[] fileContents, string fileName, Dictionary<string, object> fieldValues)
        {  
            await Task.Factory.StartNew(() =>
            {
                list.ParentWeb.Context.Load(list, mrl => mrl.RootFolder);
                SP.Folder rootFolder = list.RootFolder;
                list.Context.Load(rootFolder, folder => folder.ServerRelativeUrl);
                list.Context.ExecuteQuery();

                //Get the upload path
                //TODO: Check to see if it already exists
                string uploadPath = rootFolder.ServerRelativeUrl;
                if (uploadPath.EndsWith("/") == false) uploadPath += "/";
                uploadPath += fileName;

                //Upload the file
                using (IO.MemoryStream inputFileStream = new IO.MemoryStream(fileContents))
                { SP.File.SaveBinaryDirect((SP.ClientContext)(list.ParentWeb.Context), uploadPath, inputFileStream, true); }

                //Get the file object from the server
                SP.File file = list.ParentWeb.GetFileByServerRelativeUrl(uploadPath);
                SP.ListItem listItem = file.ListItemAllFields;

                list.ParentWeb.Context.Load(listItem);
                list.ParentWeb.Context.ExecuteQuery();

                //Populate the fields
                foreach (KeyValuePair<string, object> fieldValue in fieldValues)
                {
                    Debug.WriteLine(string.Format("Setting field ({0}) to ({1})", fieldValue.Key, fieldValue.Value.ToString()));
                    listItem[SPHelper.UnspaceValue(fieldValue.Key)] = fieldValue.Value; 
                }
                listItem.Update();
                list.ParentWeb.Context.ExecuteQuery();
                


                //Check the file in
                if (listItem["CheckoutUser"] != null)
                { 
                    file.CheckIn(null, SP.CheckinType.MajorCheckIn);
                    list.ParentWeb.Context.ExecuteQuery();
                }
            });
        }        

        public static async Task<object> GetUserObject(SP.Web site, string loginName)
        {
            if (string.IsNullOrWhiteSpace(loginName))
            { throw new ArgumentNullException("loginName"); }

            return await Task.Factory.StartNew<object>(() =>
            {
                SP.User user = site.EnsureUser(loginName);
                if (user == null) throw new Exception(loginName + " is not a valid user account");

                site.Context.Load(user);
                site.Context.ExecuteQuery();

                return new SP.FieldLookupValue() { LookupId = user.Id };
            });
        }




        public static async Task TestAuthentication(string host, SP.ClientAuthenticationMode authMode, NetworkCredential credentials)
        {
            //EnsureHasCredentials();

            using (SP.ClientContext context = await Authenticate(host, authMode, credentials))
            { }

        }

        public static async Task<SP.ClientContext> Authenticate(string host, SP.ClientAuthenticationMode authMode, NetworkCredential credentials)
        {
            if (string.IsNullOrWhiteSpace(host))
            { throw new ArgumentNullException("Value required", "host"); }
           
            if (authMode != SP.ClientAuthenticationMode.Anonymous)
            {
                if (credentials == null)
                { throw new ArgumentNullException("Value required", "credentials"); }
                if (string.IsNullOrWhiteSpace(credentials.UserName))
                { throw new ArgumentNullException("Value required", "credentials.UserName"); }
                if (string.IsNullOrWhiteSpace(credentials.Password))
                { throw new ArgumentNullException("Value required", "credentials.Password"); }
            }

            return await Task.Factory.StartNew<SP.ClientContext>(() =>
            {
                SP.ClientContext context = new SP.ClientContext(host);

                if (authMode == SP.ClientAuthenticationMode.FormsAuthentication)
                {
                    context.AuthenticationMode = SP.ClientAuthenticationMode.FormsAuthentication;
                    context.FormsAuthenticationLoginInfo = new SP.FormsAuthenticationLoginInfo(credentials.UserName, credentials.Password); 
                }
                else if (authMode == SP.ClientAuthenticationMode.Default)
                {
                    context.AuthenticationMode = SP.ClientAuthenticationMode.Default;
                    context.Credentials = credentials;
                }
                else if (authMode == SP.ClientAuthenticationMode.Anonymous)
                { context.AuthenticationMode = SP.ClientAuthenticationMode.Anonymous; }

                context.ExecuteQuery();

                return context;
            });
        }

        public static async Task<SP.List> GetList(SP.Web site, string listTitle)
        {
            return await Task.Factory.StartNew<SP.List>(()=>
            {
                site.Context.Load(site, s => s.Lists);
                site.Context.ExecuteQuery();
                return site.Lists.GetByTitle(listTitle);
            });
        }

        public static async Task<SP.Web> GetSite(SP.ClientContext context, string siteTitle)
        {
            return await Task.Factory.StartNew<SP.Web>(() =>
            {
                SP.Web rootSite = context.Web;
                if (string.IsNullOrWhiteSpace(siteTitle))
                { return rootSite; }

                context.Load(rootSite, site => site.Webs, site => site.Title);
                context.ExecuteQuery();
                foreach (SP.Web subSite in rootSite.Webs)
                {
                    Debug.WriteLine(string.Format("Checking site {0}", subSite.Title));
                    if (subSite.Title == siteTitle) return subSite; 
                }

                throw new KeyNotFoundException("Site " + siteTitle + " could not be found");
            });
        }
    }
}
