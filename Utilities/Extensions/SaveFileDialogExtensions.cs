using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace EdocsUSA.Utilities.Extensions
{
	public static class SaveFileDialogExtensions
	{
		public static DialogResult ShowDialog(this SaveFileDialog dialog, IWin32Window owner, SynchronizationContext context)
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
				
		public static DialogResult ShowDialog(this SaveFileDialog dialog, SynchronizationContext context)
		{ return dialog.ShowDialog(default(IWin32Window), context); }

		public static DialogResult TryShowDialog(this SaveFileDialog dialog, IWin32Window owner, SynchronizationContext context)
		{
			DialogResult result = dialog.ShowDialog(owner, context);
			if (result != DialogResult.OK)
			{ throw new OperationCanceledException(); }
			return result;
		}
		
		public static DialogResult TryShowDialog(this SaveFileDialog dialog)
		{ return dialog.TryShowDialog(default(IWin32Window), default(SynchronizationContext)); }
			
		public static DialogResult TryShowDialog(this SaveFileDialog dialog, IWin32Window owner)
		{ return dialog.TryShowDialog(owner, default(SynchronizationContext)); }
		
		public static DialogResult TryShowDialog(this SaveFileDialog dialog, SynchronizationContext context)
		{ return dialog.TryShowDialog(default(IWin32Window), context); }
		
		public static string SelectFile(this SaveFileDialog dialog, IWin32Window owner, SynchronizationContext context)
		{
			dialog.TryShowDialog(owner, context);
			return dialog.FileName;
		}
		
		public static string SelectFile(this SaveFileDialog dialog, IWin32Window owner)
		{ return dialog.SelectFile(owner, default(SynchronizationContext)); }
		
		public static string SelectFile(this SaveFileDialog dialog, SynchronizationContext context)
		{ return dialog.SelectFile(default(IWin32Window), context); }
		
		public static string GetCurrentDirectory(this SaveFileDialog dialog)
		{
			string[] fileNames = dialog.FileNames;
			if ((fileNames == null) || (fileNames.Length <= 0))
			{ return null; }
			else 
			{ return Path.GetDirectoryName(fileNames[0]); }
		}
		
	}
}