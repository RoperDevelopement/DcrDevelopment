using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace EdocsUSA.Utilities.Extensions
{
	public static class FormExtensions
	{
		public static DialogResult ShowDialog(this Form form, IWin32Window owner, SynchronizationContext context)
		{
			DialogResult result = DialogResult.None;
			
			//Set up the action to show the dialog and store the result in result.
			Action showDialogAction = new Action(()=> result = form.ShowDialog(owner));

			//If context was not provided, just perform showDialogAction on the current thread.
			if (context == null)
			{ showDialogAction(); }
			//Otherwise (context was provided), send showDialogAction to the context's thread.
			else
			{ context.Send(showDialogAction); }
			
			return result;
		}
		
		public static DialogResult ShowDialog(this Form form, SynchronizationContext context)
		{ return form.ShowDialog(default(IWin32Window), context); }
		
		/// <summary>
		/// Shows the dialog and throws an OperationCancelledException if the result is not in expectedResults.
		/// </summary>
		/// <param name="expectedResults">The expected DialogResults for the form.  Any other result will cause an OperationCanceledException</param>
		/// <returns>The DialogResult returned from the ShowDialog operation</returns>
		/// <exception cref="OperationCanceledException">If the form's ShowDialog returns a result not in expectedResults</exception>
		public static DialogResult TryShowDialog(this Form form, IWin32Window owner, SynchronizationContext context, params DialogResult[] expectedResults)
		{
			DialogResult result = form.ShowDialog(owner, context);
			if (expectedResults.Contains(result))
			{ return result; }
			else
			{ throw new OperationCanceledException(); }
		}
		
		/// <summary>
		/// Shows the dialog and throws an OperationCancelledException if the result is not in expectedResults.
		/// </summary>
		/// <param name="expectedResults">The expected DialogResults for the form.  Any other result will cause an OperationCanceledException</param>
		/// <returns>The DialogResult returned from the ShowDialog operation</returns>
		/// <exception cref="OperationCanceledException">If the form's ShowDialog returns a result not in expectedResults</exception>
		public static DialogResult TryShowDialog(this Form form, IWin32Window owner, params DialogResult[] expectedResults)
		{ return form.TryShowDialog(owner, default(SynchronizationContext), expectedResults); }

		/// <summary>
		/// Shows the dialog and throws an OperationCancelledException if the result is not in expectedResults.
		/// </summary>
		/// <param name="expectedResults">The expected DialogResults for the form.  Any other result will cause an OperationCanceledException</param>
		/// <returns>The DialogResult returned from the ShowDialog operation</returns>		
		/// <exception cref="OperationCanceledException">If the form's ShowDialog returns a result not in expectedResults</exception>
		public static DialogResult TryShowDialog(this Form form, SynchronizationContext context, params DialogResult[] expectedResults)
		{ return form.TryShowDialog(default(IWin32Window), context, expectedResults); }
		
		public static DialogResult TryShowDialog(this Form form, params DialogResult[] expectedResults)
		{ return form.TryShowDialog(default(IWin32Window), default(SynchronizationContext), expectedResults); }
	}
}