using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace EdocsUSA.Utilities.Extensions
{
	public static class SynchronizationContextExtensions
	{
		public static readonly SynchronizationContext DefaultContext = new SynchronizationContext();
		
		private static SendOrPostCallback ActionToSendOrPostCallback(Action action)
		{
			return delegate(object state)
			{
				((Action)(state))();
			};
		}
		
		public static void Post(this SynchronizationContext context, Action action)
		{
			SendOrPostCallback callback = ActionToSendOrPostCallback(action);
			
			context.Post(callback, action);
		}
		
		public static void Send(this SynchronizationContext context, Action action)
		{
			SendOrPostCallback callback = ActionToSendOrPostCallback(action);
			
			context.Send(callback, action);
		}
	}
}
