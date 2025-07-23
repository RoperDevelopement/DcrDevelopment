using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace DemoArchivers
{
    class SharepointRestConnector
    {
        private readonly NetworkCredential _Credentials;
        protected NetworkCredential Credentials
        { get { return _Credentials; } }

        private readonly string _AuthorizationHeader;
        public string AuthorizationHeader
        { get { return _AuthorizationHeader; } }

        private readonly string _SiteUrl;
        public string SiteUrl
        { get { return _SiteUrl; } }

        protected Uri SiteUri
        { get { return new Uri(SiteUrl); } }

        protected Uri SiteApiUri
        { get { return new Uri(SiteUri, "_api/"); } }

        private readonly string _LibraryName;
        public string LibraryName
        { get { return _LibraryName; } }

        private readonly string _LibraryServerRelativeUrl;
        public string LibraryServerRelativeUrl
        { get { return _LibraryServerRelativeUrl; } }

        XmlNamespaceManager DefaultXmlNamespaceManager = new XmlNamespaceManager(new NameTable());

        private SharepointRestConnector(string siteUrl)
        {
            //Add pertinent namespaces to the namespace manager.
            DefaultXmlNamespaceManager.AddNamespace("atom", "http://www.w3.org/2005/Atom");
            DefaultXmlNamespaceManager.AddNamespace("d", "http://schemas.microsoft.com/ado/2007/08/dataservices");
            DefaultXmlNamespaceManager.AddNamespace("m", "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata");

            this._SiteUrl = siteUrl;
        }

        public SharepointRestConnector(string siteUrl, string authorizationHeader, string libraryName)
            : this(siteUrl)
        {
            this._AuthorizationHeader = authorizationHeader;
            this._LibraryName = libraryName;

            string digest = GetFormDigest();
            TestConnection(digest);
            this._LibraryServerRelativeUrl = GetLibraryRootFolderServerRelativeUrl(digest);
        }

        public SharepointRestConnector(string siteUrl, NetworkCredential credentials, string libraryName) : this(siteUrl)
        {
            this._Credentials = credentials;
            this._LibraryName = libraryName;
            this._AuthorizationHeader = GetAuthorizationHeader(credentials.Domain, credentials.UserName, credentials.Password);

            string digest = GetFormDigest();
            TestConnection(digest);
            this._LibraryServerRelativeUrl = GetLibraryRootFolderServerRelativeUrl(digest);
        }

        public void TestConnection(string digest)
        {
            EnsureHasCredentials();
            EnsureHasSiteUrl();

            Uri requestUri = new Uri(SiteApiUri, "site");
            HttpWebResponse response = Get(requestUri, CONTENT_TYPE_ATOM, digest);
            Stream stream = response.GetResponseStream();
            if (stream != null)
            { stream.Close(); }
        }

        public void TestConnection()
        { TestConnection(GetFormDigest()); }

        public const string CONTENT_TYPE_JSON = "application/json;odata=verbose";
        public const string CONTENT_TYPE_ATOM = "application/atom+xml";

        protected HttpWebResponse Get(Uri requestUri, string acceptType, string digest)
        {
            Trace.TraceInformation("Getting " + requestUri.ToString());

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(requestUri);
            request.Method = "GET";
            request.Headers["Authorization"] = AuthorizationHeader;
            //request.Credentials = new NetworkCredential("administrator", "e-Docs", "edocsbackup");
            request.Accept = acceptType;
            request.ContentType = "application/atom+xml;type=entry";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                Trace.TraceError("GET failed on " + requestUri.ToString());
                Trace.TraceError(response.StatusCode + " " + response.StatusDescription);
                throw new Exception("Request failed " + response.StatusCode + " " + response.StatusDescription);
            }

            return response;
        }

        protected HttpWebResponse Post(Uri requestUri, string contentType, string acceptType, byte[] content, string digest)
        {
            Trace.TraceInformation("Posting" + requestUri.ToString());

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(requestUri);
            request.Method = "POST";
            request.Accept = acceptType;
            request.ContentType = contentType;
            request.Headers["Authorization"] = this.AuthorizationHeader;
            request.Headers["X-RequestDigest"] = digest;
            request.ContentLength = content.Length;

            using (Stream requestStream = request.GetRequestStream())
            { requestStream.Write(content, 0, content.Length); }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            return response;
        }


        /*string contentText = 
            byte[] content = EncodeToBinary(contentText);*/

        protected string GetLibraryRootFolderServerRelativeUrl(string digest)
        {
            Uri requestUri = new Uri(SiteApiUri, "web/lists/GetByTitle('" + LibraryName + "')/RootFolder?$select=ServerRelativeUrl");

            HttpWebResponse response = Get(requestUri, CONTENT_TYPE_ATOM, digest);

            XmlDocument responseDocument = HttpWebResponseStreamToXml(response);

            return responseDocument.SelectSingleNode("//atom:entry/atom:content/m:properties/d:ServerRelativeUrl", DefaultXmlNamespaceManager).InnerText;
        }


        public string GetFileListItemId(string serverRelativeUrl, string digest = null)
        {
            EnsureHasSiteUrl();
            EnsureHasCredentials();

            if (string.IsNullOrWhiteSpace(digest))
            { digest = GetFormDigest(); }

            Uri requestUri = new Uri(SiteApiUri, "web/GetFileByServerRelativeUrl('" + serverRelativeUrl + "')/ListItemAllFields?$select=Id");
            HttpWebResponse response = Get(requestUri, CONTENT_TYPE_ATOM, digest);

            XmlDocument responseDocument = HttpWebResponseStreamToXml(response);
            return responseDocument.SelectSingleNode("//atom:entry/atom:content/m:properties/d:ID", DefaultXmlNamespaceManager).InnerText;
        }

        public string GetContentType(string digest = null)
        {
            EnsureHasSiteUrl();
            EnsureHasCredentials();

            if (string.IsNullOrWhiteSpace(digest))
            { digest = GetFormDigest(); }

            Uri requestUri = new Uri(SiteApiUri, "Lists/GetByTitle('" + LibraryName + "')/ListItemEntityTypeFullName");
            HttpWebResponse response = Get(requestUri, CONTENT_TYPE_ATOM, digest);

            XmlDocument reponseDocument = HttpWebResponseStreamToXml(response);
            return reponseDocument.SelectSingleNode("//d:ListItemEntityTypeFullName", DefaultXmlNamespaceManager).InnerText;
        }

        public string CreateDirectory(string libraryRelativePath, string digest)
        {
            string[] directories = libraryRelativePath.Split('/', '\\');
            string serverRelativePath = LibraryServerRelativeUrl;
            foreach (string directory in directories)
            {
                digest = GetFormDigest();
                Uri requestUri = new Uri(SiteApiUri, "web/folders/");
                serverRelativePath = UrlCombine(serverRelativePath, directory);
                Trace.TraceInformation("Creating " + serverRelativePath);
                string contentText = "{ '__metadata': { 'type': 'SP.Folder' }, 'ServerRelativeUrl': '" + serverRelativePath + "'}";
                byte[] content = EncodeToBinary(contentText);
                HttpWebResponse response = Post(requestUri, CONTENT_TYPE_JSON, CONTENT_TYPE_ATOM, content, digest);
                Stream stream = response.GetResponseStream();
                if (stream != null)
                { stream.Close(); }
                if (response.StatusCode != HttpStatusCode.Created)
                {
                    Trace.TraceError("Failed to create folder " + requestUri.ToString());
                    Trace.TraceError(response.StatusCode + " " + response.StatusDescription);
                    throw new Exception("Request failed " + response.StatusCode + " " + response.StatusDescription);
                }
                //Discard the response stream
                
            }
            return serverRelativePath;
        }

        public string CreateDirectory(string libraryRelativePath)
        { return CreateDirectory(libraryRelativePath, GetFormDigest()); }
        /*
        public bool TryGetLibraryRootDirectory(out string path)
        {
            HttpClient client = new HttpClient();
            string authData = Convert.ToBase64String(Encoding.Default.GetBytes("administrator:e-Docs"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authData);
            client.DefaultRequestHeaders.Add("Accept", @"application/atom+xml");
            client.BaseAddress = this.SiteApiUri;
            string requestUrl = @"web/lists/getByTitle('Lab Records')/RootFolder/Folders?$select=Name&$filter=Name eq '1999'";
            HttpResponseMessage r = client.GetAsync(requestUrl).Result;

            if (r.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Trace.TraceError("Error executing REST command");
                Trace.TraceError(requestUrl.ToString());
                Trace.TraceError(r.StatusCode + " " + r.ReasonPhrase);
                path = null;
                return false;
            }

            string rs = r.Content.ReadAsStringAsync().Result;
            
            //Console.WriteLine(rs);

            path = rs;
            return true;
        }

        */
        /*
         *                 restService.UseDefaultCredentials = true;
                restService.Credentials = CredentialCache.DefaultCredentials;
                restService.Method = "POST";
                restService.Accept = "application/json;odata=verbose";
                restService.Headers["X-RequestDigest"] = strFormDigest;
                restService.ContentType = "application/atom+xml";
                restService.ContentLength = data.Length;
                restService.GetRequestStream().Write(data, 0, data.Length);
                HttpWebResponse restResponse = (HttpWebResponse)restService.GetResponse();

                if (restResponse.StatusCode == HttpStatusCode.OK)
                { return strdocURL; }
                else
                { throw new Exception(restResponse.StatusCode + " " + restResponse.StatusDescription); }*/

        /*
        public bool TryGetLibraryRootDirectory(string libraryPath, out string absolutPath)
        {
            string digest = GetFormDigest();
            string requestUri = this.SiteUrl + "/_api/web/GetFolderByServerRelativeUrl('/sites/lab/Folder Testing')";
            string requestUri = (HttpWebRequest)WebRequest.Create(requestUri);
        }

        public bool TryCreateDirectory(string libraryPath, string destPath, out string absolutPath)
        {
            string digest = GetFormDigest();
            string requestUri = string.Format(this.SiteUrl + "{0}/_api/web/GetFolderByServerRelativeUrl('{1}')", this.SiteUrl + libraryPath);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUri);
        }*/


        public void CheckInFile(string serverRelativeUrl, string comments, string digest = null)
        {
            EnsureHasSiteUrl();
            EnsureHasCredentials();

            if (string.IsNullOrWhiteSpace(digest))
            { digest = GetFormDigest(); }

            Uri requestUri = new Uri(SiteApiUri, "web/GetFileByServerRelativeUrl('" + serverRelativeUrl + "')/CheckIn(comment='" + comments + "', checkintype=0)");
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(requestUri);
            Trace.TraceError("Requesting " + requestUri);
            request.Method = "POST";
            request.Headers["Authorization"] = this.AuthorizationHeader;
            request.Headers["X-RequestDigest"] = digest;

            request.ContentLength = 0;
            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                string responseText = string.Empty;
                if (responseStream != null)
                {
                    responseText = StreamToString(responseStream);
                    responseStream.Close();
                }

                if (response.StatusCode != HttpStatusCode.OK)
                {

                    Trace.TraceError("Error checking in " + serverRelativeUrl);
                    Trace.TraceError(response.StatusCode + " " + response.StatusDescription);
                    Trace.TraceError(responseText);
                    throw new Exception("Error checking in file " + response.StatusDescription);
                }
            }
            catch (WebException ex)
            {
                Trace.TraceError(ex.Message);
                Stream responseStream = ex.Response.GetResponseStream();
                string responseText = string.Empty;
                if (responseStream != null)
                {
                    responseText = StreamToString(responseStream);
                    responseStream.Close();
                }
                Trace.TraceError(responseText);
                throw ex;
            }
        }



        public void UpdateMetadata(string id, Dictionary<string, object> metadata, string digest = null)
        {
            EnsureHasSiteUrl();
            EnsureHasCredentials();

            if (string.IsNullOrWhiteSpace(digest))
            { digest = GetFormDigest(); }

            Uri requestUri = new Uri(SiteApiUri, "web/lists/GetByTitle('" + LibraryName + "')/items(" + id + ")");
            Trace.TraceError("Requesting " + requestUri);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(requestUri);
            request.Method = "POST";
            request.Headers["Authorization"] = this.AuthorizationHeader;
            request.Headers["X-RequestDigest"] = digest;
            request.Headers["X-HTTP-Method"] = "MERGE";
            request.Headers["IF-MATCH"] = "*";
            request.Accept = CONTENT_TYPE_ATOM;
            request.ContentType = CONTENT_TYPE_JSON;
            Dictionary<string, object> metadataDictionary = new Dictionary<string, object>();

            string metadataContentType = GetContentType(digest);
            //"SP.Data.Folder_x0020_TestingItem"
            metadataDictionary["__metadata"] = new Dictionary<string, object>() { { "type", metadataContentType } };
            foreach (KeyValuePair<string, object> m in metadata)
            { metadataDictionary[m.Key] = m.Value; }
            string contentText = JsonConvert.SerializeObject(metadataDictionary, Newtonsoft.Json.Formatting.Indented);
            File.AppendAllText("json.txt", contentText);
            byte[] content = EncodeToBinary(contentText);
            request.ContentLength = content.Length;
            using (Stream requestStream = request.GetRequestStream())
            { requestStream.Write(content, 0, content.Length); }

            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                Trace.TraceError(ex.Message);
                Stream errorResponseStream = ex.Response.GetResponseStream();
                string errorResponseText = string.Empty;
                if (errorResponseStream != null)
                {
                    errorResponseText = StreamToString(errorResponseStream);
                    errorResponseStream.Close();
                }
                Trace.TraceError(errorResponseText);
                throw ex;
            }

            Stream responseStream = response.GetResponseStream();
            string responseText = string.Empty;
            if (responseStream != null)
            {
                responseText = StreamToString(responseStream);
                responseStream.Close();
            }
            if (response.StatusCode != HttpStatusCode.NoContent)
            {
                Trace.TraceError("Error updating metadata");
                Trace.TraceError(response.StatusCode + " " + response.StatusDescription);
                throw new Exception("Error updating metadata " + response.StatusCode + " " + response.StatusDescription);
            }
        }



        /// <credit>http://sharepointdeck.blogspot.com/2013/09/sharepoint-upload-file-using-rest.html</credit>
        public string UploadFile(byte[] data, string libraryRelativeDirectory, string fileName, Dictionary<string, object> metadata, string digest = null)
        {
            EnsureHasSiteUrl();
            EnsureHasCredentials();

            if (string.IsNullOrWhiteSpace(digest))
            { digest = GetFormDigest(); }

            //Deal with foldering
            if (string.IsNullOrWhiteSpace(libraryRelativeDirectory) == false)
            { CreateDirectory(libraryRelativeDirectory); }

            string serverRelativeDirectory = UrlCombine(LibraryServerRelativeUrl, libraryRelativeDirectory);

            //Uri requestUri = new Uri(this.SiteApiUri, "web/GetFolderByServerRelativeUrl(/getByTitle('" + LibraryName + "')/RootFolder/Files/add(url='" + fileName + "',overwrite='true')?$select=serverrelativeurl");

            Uri requestUri = new Uri(this.SiteApiUri, "web/GetFolderByServerRelativeUrl('" + serverRelativeDirectory + "')/Files/add(url='" + fileName + "',overwrite='true')?$select=serverrelativeurl");
            Trace.TraceError("Requesting " + requestUri);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(requestUri);
            request.Method = "POST";
            request.Headers["Authorization"] = this.AuthorizationHeader;
            request.Headers["X-RequestDigest"] = digest;
            request.ContentLength = data.Length;

            using (Stream stream = request.GetRequestStream())
            { stream.Write(data, 0, data.Length); }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
        
            if (response.StatusCode != HttpStatusCode.OK)
            {
                Trace.TraceError("Error uploading file");
                Trace.TraceError(response.StatusCode + " " + response.StatusDescription);
                
                throw new Exception("Error uploading file " + response.StatusCode + " " + response.StatusDescription);
            }

            XmlDocument responseDocument = HttpWebResponseStreamToXml(response);
            string serverRelativeUrl = UrlCombine(LibraryServerRelativeUrl, libraryRelativeDirectory, fileName);

            string fileId = GetFileListItemId(serverRelativeUrl, digest);
            UpdateMetadata(fileId, metadata, digest);
            //CheckInFile(serverRelativeUrl, "none", digest);

            return serverRelativeUrl;
        }

        public void EnsureHasSiteUrl()
        {
            if (string.IsNullOrWhiteSpace(this.SiteUrl))
            { throw new InvalidOperationException("The requested operation requires SiteUrl to be defined"); }
        }

        public void EnsureHasCredentials()
        {
            if (string.IsNullOrWhiteSpace(this.AuthorizationHeader))
            { throw new InvalidOperationException("The requested operation requires Credentials to be set"); }
        }

        /// <credit>http://www.sharepoint-insight.com/2013/03/17/get-formdigest-from-c-using-rest-in-sharepoint-2013/</credit>
        public string GetFormDigest()
        {
            EnsureHasSiteUrl();
            EnsureHasCredentials();

            //Create REST Request
            Uri uri = new Uri(SiteUrl + "_api/contextinfo");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Headers["Authorization"] = this.AuthorizationHeader;
            request.Method = "POST";
            request.ContentLength = 0;

            //Retrieve Response
            HttpWebResponse restResponse = (HttpWebResponse)request.GetResponse();
            XDocument atomDoc = XDocument.Load(restResponse.GetResponseStream());
            XNamespace d = "http://schemas.microsoft.com/ado/2007/08/dataservices";

            //Extract Form Digest
            return atomDoc.Descendants(d + "FormDigestValue").First().Value;
        }

        public static string GetAuthorizationHeader(string domain, string user, string password)
        {
            //Format is Basic[space][[domain]/[user]:[password] as Base64String]
            string authText = string.Empty;

            if (string.IsNullOrWhiteSpace(domain) == false)
            { authText += domain + @"\" + user; }
            else
            { authText += user; }

            authText += ":" + password;
            return "Basic " + EncodeToBase64String(authText);
        }

        public string UrlCombine(params string[] urls)
        {
            IEnumerator<string> urlsEnumerator = urls.AsEnumerable().GetEnumerator();
            if (urlsEnumerator.MoveNext() == false)
            { throw new ArgumentException("No urls specified"); }

            string combinedUrl = string.Empty;
            string currentUrl = urlsEnumerator.Current.TrimEnd('/');
            if (string.IsNullOrWhiteSpace(currentUrl))
            { throw new ArgumentException("First URL cannot be empty"); }
            combinedUrl += currentUrl;

            while (urlsEnumerator.MoveNext())
            {
                currentUrl = urlsEnumerator.Current.Trim('/');
                if (string.IsNullOrWhiteSpace(currentUrl))
                { continue; }

                //currentUrl = currentUrl.Trim('/');
                combinedUrl += "/" + currentUrl;
            }

            return combinedUrl;
        }

        protected static byte[] EncodeToBinary(string value)
        { return new ASCIIEncoding().GetBytes(value); }

        protected static string EncodeToBase64String(string value)
        { return Convert.ToBase64String(EncodeToBinary(value)); }

        protected string StreamToString(Stream stream)
        {
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            { return reader.ReadToEnd(); }
        }

        protected XmlDocument StreamToXmlDocument(Stream stream)
        {
            string text = StreamToString(stream);
            XmlDocument document = new XmlDocument();
            document.LoadXml(text);
            return document;
        }

        protected string HttpWebResponseStreamToString(HttpWebResponse response)
        {
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            { return reader.ReadToEnd(); }
        }

        protected XmlDocument HttpWebResponseStreamToXml(HttpWebResponse response)
        {
            string responseText = HttpWebResponseStreamToString(response);
            XmlDocument document = new XmlDocument();
            document.LoadXml(responseText);
            return document;
        }

    }
}

