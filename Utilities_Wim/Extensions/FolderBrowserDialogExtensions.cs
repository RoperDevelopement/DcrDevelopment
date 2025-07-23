using System;
using System.Threading;
using System.Windows.Forms;

namespace EdocsUSA.Utilities.Extensions
{
	public static class FolderBrowserDialogExtensions
	{
		public static DialogResult ShowDialog(this FolderBrowserDialog dialog, IWin32Window owner, SynchronizationContext context)
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
		
		public static DialogResult ShowDialog(this FolderBrowserDialog dialog, SynchronizationContext context)
		{ return dialog.ShowDialog(default(IWin32Window), context); }
		
		public static DialogResult TryShowDialog(this FolderBrowserDialog dialog, IWin32Window owner, SynchronizationContext context)
		{
			DialogResult result = dialog.ShowDialog(owner, context);
			if (result != DialogResult.OK)
			{ throw new OperationCanceledException(); }
			return result;
		}
		
		public static DialogResult TryShowDialog(this FolderBrowserDialog dialog, IWin32Window owner)
		{ return dialog.TryShowDialog(owner, default(SynchronizationContext)); }
		
		public static DialogResult TryShowDialog(this FolderBrowserDialog dialog, SynchronizationContext context)
		{ return dialog.TryShowDialog(default(IWin32Window), context); }
		
		public static string SelectFolder(this FolderBrowserDialog dialog, IWin32Window owner, SynchronizationContext context)
		{
			dialog.TryShowDialog(owner, context);
			return dialog.SelectedPath;
		}
		
		public static string SelectFolder(this FolderBrowserDialog dialog)
		{ return dialog.SelectFolder(default(IWin32Window), default(SynchronizationContext)); }
		
		public static string SelectFolder(this FolderBrowserDialog dialog, IWin32Window owner)
		{ return dialog.SelectFolder(owner, default(SynchronizationContext)); }
			
		public static string SelectFolder(this FolderBrowserDialog dialog, SynchronizationContext context)
		{ return dialog.SelectFolder(null, context); }
		
	}
}