/*
 * Created by SharpDevelop.
 * User: Sam Brinly
 * Date: 4/11/2011
 * Time: 8:40 AM
 */
using System;

namespace EdocsUSA.Utilities
{
	public class StatusNotifierEventArgs : EventArgs
	{
		public string Message;
		
		public StatusNotifierEventArgs(string message) { this.Message = message; }
	}
	
	public delegate void StatusNotifierEventHandler(object sender, StatusNotifierEventArgs e);
	
	public interface IStatusNotifier
	{	
		event StatusNotifierEventHandler StatusNotify;	
	}
}