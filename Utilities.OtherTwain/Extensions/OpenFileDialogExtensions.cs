using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace EdocsUSA.Utilities.Extensions
{
	public static class OpenFileDialogExtensions
	{		
		public static DialogResult ShowDialog(this OpenFileDialog dialog, IWin32Window owner, SynchronizationContext context)
		{
			DialogResult result = DialogResult.None;
			
			//Set up the action to show the dialog and store the result in result.
			Action showDialogAction = new Action(()=> result = dialog.ShowDialog(owner));

			//If context was not provided, just perform showDialogAction on the current thread.
			if (context == null)
			{ showDialogAction(); }
			//Otherwise (context was provided), send showDialogAction to the context's thread.
			else
			{ context.Send(showDialogAction); }
			
			return result;
		}
		
		public static DialogResult ShowDialog(this OpenFileDialog dialog, SynchronizationContext context)
		{ return dialog.ShowDialog(default(IWin32Window), context); }
		
		public static DialogResult TryShowDialog(this OpenFileDialog dialog, IWin32Window owner, SynchronizationContext context)
		{
			DialogResult result = dialog.ShowDialog(owner, context);
			if (result != DialogResult.OK)
			{ throw new OperationCanceledException(); }
			return result;
		}
		
		public static DialogResult TryShowDialog(this OpenFileDialog dialog, IWin32Window owner)
		{ return dialog.TryShowDialog(owner, default(SynchronizationContext)); }
		
		public static DialogResult TryShowDialog(this OpenFileDialog dialog, SynchronizationContext context)
		{ return dialog.TryShowDialog(default(IWin32Window), context); }
		
		public static string[] SelectFiles(this OpenFileDialog dialog, IWin32Window owner, SynchronizationContext context)
		{
			dialog.TryShowDialog(owner, context);
       	return dialog.FileNames;
		}
		
		public static string[] SelectFiles(this OpenFileDialog dialog)
		{ return dialog.SelectFiles(default(IWin32Window), default(SynchronizationContext)); }
		
		public static string[] SelectFiles(this OpenFileDialog dialog, IWin32Window owner)
		{ return dialog.SelectFiles(owner, default(SynchronizationContext)); }
		
		public static string[] SelectFiles(this OpenFileDialog dialog, SynchronizationContext context)
		{ return dialog.SelectFiles(default(IWin32Window), context); }
		
		public static string SelectFile(this OpenFileDialog dialog, IWin32Window owner, SynchronizationContext context)
		{
			string[] filePaths = dialog.SelectFiles(owner, context);
			if ((filePaths == null) || (filePaths.Length <= 0))
			{ return null; }
			else if (filePaths.Length > 1)
			{ throw new InvalidOperationException("The requested operation requires a single file to be selected"); }
			else
			{ return filePaths[0]; }
		}
		
		public static string SelectFile(this OpenFileDialog dialog, IWin32Window owner)
		{ return dialog.SelectFile(owner, default(SynchronizationContext)); }
		
		public static string SelectFile(this OpenFileDialog dialog, SynchronizationContext context)
		{ return dialog.SelectFile(default(IWin32Window), context); }
		
		public static string GetCurrentDirectory(this OpenFileDialog dialog)
		{
			string[] fileNames = dialog.FileNames;
			if ((fileNames == null) || (fileNames.Length <= 0))
			{ return null; }
			else 
			{ return Path.GetDirectoryName(fileNames[0]); }
		}
	}
}
