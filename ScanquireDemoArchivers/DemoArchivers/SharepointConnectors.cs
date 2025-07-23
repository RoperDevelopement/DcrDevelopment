using System;
using SP = Microsoft.SharePoint.Client;
using Scanquire.Public.Sharepoint;
namespace DemoArchivers
{
	public static class SharepointConnectors
	{
		public static SharepointConnector GetDemoConnector()
		{
			SharepointConnector sp = new SharepointConnector();
			sp.Host = "http://sp.edocsusa.com/";
			sp.SiteName = "Demo";
			sp.UserName = "automated@sp.edocsusa.com";
			sp.Password = "P@perle$$2012";
			sp.AuthMode = SP.ClientAuthenticationMode.Default;
			sp.Timeout = null;
			return sp;
		}
	}
}
