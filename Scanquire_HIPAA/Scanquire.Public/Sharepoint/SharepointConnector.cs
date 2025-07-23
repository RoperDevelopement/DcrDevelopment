using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Windows.Forms;
using System.Linq;
using Microsoft.SharePoint.Client;
using EdocsUSA.Utilities;
using EdocsUSA.Controls;
using IO = System.IO;
using SP = Microsoft.SharePoint.Client;


namespace Scanquire.Public.Sharepoint
{	
	/// <summary>
	/// Description of SharepointConnector.
	/// </summary>
	public class SharepointConnector
	{
		private DateTime? LastActionTime;
		
		public long? Timeout { get; set; }
		
		private SharepointCredentialsInputDialog Credentials = new SharepointCredentialsInputDialog();
		
		public IWin32Window APPHwnd { get; set; }
		
		public string Domain
		{
			get { return Credentials.Domain.Value; }
			set { Credentials.Domain.Value = value; }
		}
		
		public string UserName
		{
			get { return Credentials.User.Value; }
			set { Credentials.User.Value = value; }
		}
		
		public string Password
		{
			get { return Credentials.Password.Value; }
			set { Credentials.Password.Value = value; }
		}
		
		public string Host
		{
			get { return Credentials.Host.Value; }
			set { Credentials.Host.Value = value; }
		}
		
		public SP.ClientAuthenticationMode AuthMode
		{
			get { return Credentials.AuthenticationMode; }
			set { Credentials.AuthenticationMode= value; }
		}
		
		public string SiteName { get; set; }
		
		public string LibraryName { get; set; }
		
		public SP.Web Site = null;
		
		public SharepointConnector() { }
		
		public Byte[] RetrieveFile(string camlQuery)
		{
			SP.Web site = GetSite();
			SP.List list = site.Lists.GetByTitle(LibraryName);
            Trace.WriteLine("Executing query");
			SP.CamlQuery query = new SP.CamlQuery();
			query.ViewXml = camlQuery;		
			
			SP.ListItemCollection docs = list.GetItems(query);
			
			site.Context.Load(docs);
			site.Context.ExecuteQuery();
			
			if (docs.Count == 0) throw new Exception("File not found");
			
			Trace.WriteLine("Downloading file");
			SP.ListItem doc = docs[0];
			//doc.File.CheckOut();
			site.Context.ExecuteQuery();
			SP.FileInformation fi = SP.File.OpenBinaryDirect((SP.ClientContext)site.Context, doc["FileRef"].ToString());
			using (IO.MemoryStream stream = new IO.MemoryStream())
			{			
				byte[] buffer = new Byte[8 * 1024];
				int len;
				while ( (len = fi.Stream.Read(buffer, 0, buffer.Length)) > 0)
				{ stream.Write(buffer, 0, len); } 
			
				LastActionTime = DateTime.Now;
			
				return stream.ToArray();
			}
		}
		
		public void SendFile(byte[] fileContents, string fileName, Dictionary<string, object> fieldValues)
		{
			if (Site == null) Site = GetSite();
			SP.List list = Site.Lists.GetByTitle(LibraryName);
			Site.Context.Load(list, mrl => mrl.RootFolder);
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
			{ SP.File.SaveBinaryDirect((SP.ClientContext)Site.Context, uploadPath, inputFileStream, true); }
			
			//Get the file object from the server
			SP.File file = Site.GetFileByServerRelativeUrl(uploadPath);
			SP.ListItem listItem = file.ListItemAllFields;
			
			Site.Context.Load(listItem);
			Site.Context.ExecuteQuery();
			
			//Populate the fields
			foreach (KeyValuePair<string, object> fieldValue in fieldValues)
			{ listItem[UnspaceName(fieldValue.Key)] = fieldValue.Value; }
			listItem.Update();
			Site.Context.ExecuteQuery();
			
			//Check the file in
			try 
			{
				if (file.CheckedOutByUser != null)
				{
					file.CheckIn(null, CheckinType.MajorCheckIn); 
					Site.Context.ExecuteQuery();
				}
			}
			catch {} 
			
			
			Site.Context.Dispose();
			Site = null;
			
			LastActionTime = DateTime.Now;
		}
		
		public object GetUserObject(string loginName)
		{
			if (loginName.IsEmpty()) throw new ArgumentNullException("loginName");
			
			SP.Web site = GetSite();
			
			try
			{
				SP.User user = site.EnsureUser(loginName);
				if (user == null) throw new Exception(loginName + " is not a valid user account");
				
				site.Context.Load(user);
				site.Context.ExecuteQuery();
				
				return new SP.FieldLookupValue() { LookupId = user.Id } ;
			}
			finally 
			{	site.Context.Dispose();
				site = null;
			}
		}
		
		private SP.Web GetSite()
		{
			if (LibraryName.IsEmpty()) throw new Exception("Library Name has not been set");
			
			if (
					(Timeout != null) &&
					(
						(LastActionTime == null) ||
						(((TimeSpan)(DateTime.Now - LastActionTime)).TotalMilliseconds > Timeout)
					)
				)
			{ Password = null; }
			
			if (Password.IsEmpty())
			{
				if (Credentials.ShowDialog(APPHwnd) != DialogResult.OK)
				{ throw new OperationCanceledException(); }
			}
			
			SP.ClientContext context = new SP.ClientContext(Host);
			
			if (AuthMode == SP.ClientAuthenticationMode.FormsAuthentication)
			{				
				context.AuthenticationMode = SP.ClientAuthenticationMode.FormsAuthentication;
				context.FormsAuthenticationLoginInfo = new SP.FormsAuthenticationLoginInfo(UserName, Password);
			}
			else if (AuthMode == SP.ClientAuthenticationMode.Default)
			{
				if (Domain.IsEmpty())
				{ context.Credentials = new NetworkCredential(UserName, Password); }
				else 
				{ context.Credentials = new NetworkCredential(UserName, Password, Domain); }
			}
			else throw new Exception("Invalid authentication mode");
			
			SP.Web rootSite = context.Web;
			if (SiteName == null) return rootSite;
			else
			{
				context.Load(rootSite, site => site.Webs, site => site.Title, site => site.Lists, site => site.RootFolder);
				context.ExecuteQuery();
				foreach (SP.Web subSite in rootSite.Webs)
				{ if (subSite.Title == SiteName) return subSite; }
				
				throw new Exception("Site " + SiteName + " could not be found");
			}
		}
		
		public static string UnspaceName(string visibleName)
		{ return visibleName.Replace(" ", "_x0020_"); }
	}
}