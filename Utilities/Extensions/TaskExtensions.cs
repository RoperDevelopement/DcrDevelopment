/*<Credit>http://blog.stephencleary.com/2010/06/reporting-progress-from-tasks.html</Credit> */
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EdocsUSA.Utilities.Extensions
{
	public static class TaskExtensions
	{
		/// <summary> 
		/// Registers a UI thread handler for when the specified task finishes execution,
		/// whether it finishes with success, failiure, or cancellation. 
		/// </summary> 
		/// <param name="task">The task to monitor for completion.</param> 
		/// <param name="action">The action to take when the task has completed, in the context of the UI thread.</param> 
		/// <returns>The continuation created to handle completion. This is normally ignored.</returns> 
		public static Task RegisterContinuationHandler(this Task task, Action action)
		{ return task.ContinueWith(_ => action(), CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.Current); }
		
		public static Task RegisterContinuationHandler(this Task task, Action action, CancellationToken cancellationToken, TaskScheduler scheduler)
		{ return task.ContinueWith(_ => action(), cancellationToken, TaskContinuationOptions.None, scheduler); }
		
		public static Task RegisterContinuationHandler(this Task task, Action action, TaskScheduler scheduler)
		{ return task.ContinueWith(_ => action(), CancellationToken.None, TaskContinuationOptions.None, scheduler); }
		
		/// <summary> 
		/// Registers a UI thread handler for when the specified task finishes execution,
		/// whether it finishes with success, failiure, or cancellation. 
		/// </summary> 
		/// <typeparam name="TResult">The type of the task result.</typeparam> 
		/// <param name="task">The task to monitor for completion.</param> 
		/// <param name="action">The action to take when the task has completed, in the context of the UI thread.</param> 
		/// <returns>The continuation created to handle completion. This is normally ignored.</returns> 
		public static Task RegisterContinuation<TResult>(this Task<TResult> task, Action action)
		{ return task.ContinueWith(_ => action(), CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.Current); }
		
		public static Task RegisterContinuation<TResult>(this Task<TResult> task, Action action, CancellationToken cancellationToken, TaskScheduler scheduler)
		{ return task.ContinueWith(_ => action(), cancellationToken, TaskContinuationOptions.None, scheduler); }
		
		/// <summary> 
		/// Registers a UI thread handler for when the specified task is completed. 
		/// </summary> 
		/// <param name="task">The task to monitor for completed.</param> 
		/// <param name="action">The action to take when the task is completed.</param> 
		/// <returns>The continuation created to handle completed. This is normally ignored.</returns> 
		public static Task ContinueWithOnRunToCompletion(this Task task, Action action)
		{ return task.ContinueWith(_ => action(), TaskContinuationOptions.OnlyOnRanToCompletion); }
		
		public static Task ContinueWithOnRunToCompletion(this Task task, Action action, CancellationToken cancellationToken, TaskScheduler scheduler)
		{ return task.ContinueWith(_ => action(), cancellationToken, TaskContinuationOptions.OnlyOnRanToCompletion, scheduler); }
		
		public static Task ContinueWithOnRunToCompletion(this Task task, Action action, TaskScheduler scheduler)
		{ return task.ContinueWith(_ => action(), CancellationToken.None, TaskContinuationOptions.OnlyOnRanToCompletion, scheduler); }
		
		/// <summary> 
		/// Registers a UI thread handler for when the specified task is completed. 
		/// </summary> 
		/// <typeparam name="TResult">The type of the task result.</typeparam> 
		/// <param name="task">The task to monitor for completed.</param> 
		/// <param name="action">The action to take when the task is completed, in the context of the UI thread.</param> 
		/// <returns>The continuation created to handle completed. This is normally ignored.</returns>
		public static Task FaultedContinueWith<TResult>(this Task<TResult> task, Action action)
		{ return task.ContinueWith(_ => action(), TaskContinuationOptions.OnlyOnRanToCompletion); }
		
		public static Task FaultedContinueWith<TResult>(this Task<TResult> task, Action action, CancellationToken cancellationToken, TaskScheduler scheduler)
		{ return task.ContinueWith(_ => action(), cancellationToken, TaskContinuationOptions.OnlyOnRanToCompletion, scheduler); }
		
		public static Task FaultedContinueWith<TResult>(this Task<TResult> task, Action action, TaskScheduler scheduler)
		{ return task.ContinueWith(_ => action(), CancellationToken.None, TaskContinuationOptions.OnlyOnRanToCompletion, scheduler); }
		
		/// <summary> 
		/// Registers a UI thread handler for when the specified task is fails. 
		/// </summary> 
		/// <param name="task">The task to monitor for completed.</param> 
		/// <param name="action">The action to take when the task is completed.</param> 
		/// <returns>The continuation created to handle completed. This is normally ignored.</returns> 
		public static Task FaultedContinueWith(this Task task, Action action)
		{ return task.ContinueWith(_ => action(), TaskContinuationOptions.OnlyOnFaulted); }
		
		public static Task FaultedContinueWith(this Task task, Action action, CancellationToken cancellationToken, TaskScheduler scheduler)
		{ return task.ContinueWith(_ => action(), cancellationToken, TaskContinuationOptions.OnlyOnFaulted, scheduler); }
		
		public static Task FaultedContinueWith(this Task task, Action action, TaskScheduler scheduler)
		{ return task.ContinueWith(_ => action(), CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, scheduler); }
		
		/// <summary> 
		/// Registers a UI thread handler for when the specified task fails. 
		/// </summary> 
		/// <typeparam name="TResult">The type of the task result.</typeparam> 
		/// <param name="task">The task to monitor for completed.</param> 
		/// <param name="action">The action to take when the task is completed, in the context of the UI thread.</param> 
		/// <returns>The continuation created to handle completed. This is normally ignored.</returns>
		public static Task RegisterFaultedHandler<TResult>(this Task<TResult> task, Action action)
		{ return task.ContinueWith(_ => action(), TaskContinuationOptions.OnlyOnFaulted); }
		
		public static Task RegisterFaultedHandler<TResult>(this Task<TResult> task, Action action, CancellationToken cancellationToken, TaskScheduler scheduler)
		{ return task.ContinueWith(_ => action(), cancellationToken, TaskContinuationOptions.OnlyOnFaulted, scheduler); }
		
		public static Task RegisterFaultedHandler<TResult>(this Task<TResult> task, Action action, TaskScheduler scheduler)
		{ return task.ContinueWith(_ => action(), CancellationToken.None, TaskContinuationOptions.OnlyOnFaulted, scheduler); }
		
		/// <summary> 
		/// Registers a UI thread handler for when the specified task is cancelled. 
		/// </summary> 
		/// <param name="task">The task to monitor for cancellation.</param> 
		/// <param name="action">The action to take when the task is cancelled.</param> 
		/// <returns>The continuation created to handle cancellation. This is normally ignored.</returns> 
		public static Task RegisterCancelledHandler(this Task task, Action action)
		{ return task.ContinueWith(_ => action(), TaskContinuationOptions.OnlyOnCanceled); }
		
		public static Task RegisterCancelledHandler(this Task task, Action action, CancellationToken cancellationToken, TaskScheduler scheduler)
		{ return task.ContinueWith(_ => action(), cancellationToken, TaskContinuationOptions.OnlyOnCanceled, scheduler); }
		
		public static Task RegisterCancelledHandler(this Task task, Action action, TaskScheduler scheduler)
		{ return task.ContinueWith(_ => action(), CancellationToken.None, TaskContinuationOptions.OnlyOnCanceled, scheduler); }
		
		/// <summary> 
		/// Registers a UI thread handler for when the specified task is cancelled. 
		/// </summary> 
		/// <typeparam name="TResult">The type of the task result.</typeparam> 
		/// <param name="task">The task to monitor for cancellation.</param> 
		/// <param name="action">The action to take when the task is cancelled, in the context of the UI thread.</param> 
		/// <returns>The continuation created to handle cancellation. This is normally ignored.</returns>
		public static Task RegisterCancelledHandler<TResult>(this Task<TResult> task, Action action)
		{ return task.ContinueWith(_ => action(), TaskContinuationOptions.OnlyOnCanceled); }
		
		public static Task RegisterCancelledHandler<TResult>(this Task<TResult> task, Action action, CancellationToken cancellationToken, TaskScheduler scheduler)
		{ return task.ContinueWith(_ => action(), cancellationToken, TaskContinuationOptions.OnlyOnCanceled, scheduler); }
		
		public static Task RegisterCancelledHandler<TResult>(this Task<TResult> task, Action action, TaskScheduler scheduler)
		{ return task.ContinueWith(_ => action(), CancellationToken.None, TaskContinuationOptions.OnlyOnCanceled, scheduler); }
		
	}
}
