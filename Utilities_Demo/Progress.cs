using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using EdocsUSA.Utilities.Extensions;

namespace EdocsUSA.Utilities
{
	public class ProgressEventArgs : EventArgs
	{
		protected readonly decimal _Current;
		public decimal Current { get { return _Current; } }
		
		protected readonly decimal _Total;
		public decimal Total { get { return _Total; } }
		
		protected readonly decimal _PercentCompletePrecise;
		public decimal PercentCompletePrecise { get { return _PercentCompletePrecise; } }
		
		protected readonly int _PercentComplete;
		public int PercentComplete { get { return _PercentComplete; } }
		
		protected readonly string _Caption;
		public string Caption { get { return _Caption; } }
		
		public ProgressEventArgs(decimal current, decimal total, string caption)
		{
			_Current = current;
			_Total = total;
			_PercentCompletePrecise = EdocsUSA.Utilities.Percent.Calculate(Current, Total);
			_PercentCompletePrecise = Math.Round(_PercentCompletePrecise, 2, MidpointRounding.AwayFromZero);
			_PercentComplete = (int)Math.Round(_PercentCompletePrecise, MidpointRounding.AwayFromZero);
			_Caption = caption;
		}
		
		public ProgressEventArgs(decimal current, decimal total) : this(current, total, null)
		{ }
				
		/*
		public ProgressEventArgs(decimal percentComplete)
		{
			_Current = -1;
			_Total = -1;
			_PercentCompletePrecise = percentComplete;
			_PercentCompletePrecise = Math.Round(_PercentCompletePrecise, 2, MidpointRounding.AwayFromZero);
			_PercentComplete = (int)Math.Round(_PercentCompletePrecise, MidpointRounding.AwayFromZero);
		}
		*/
		
		public override string ToString()
		{
			string value = ((Current >= 0) ? Current.ToString() : "?")
				+ " of " + ((Total >= 0) ? Total.ToString() : "?")
				+ " (" + ((PercentComplete >= 0) ? PercentComplete.ToString() : "?") + "%)"
			   + " " + Caption;
			return value;
		}
	}
	
	public class ProgressEventArgs<T> : ProgressEventArgs
	{
		private readonly T _Value;
		public T Value
		{ get { return _Value; } }
		
		public ProgressEventArgs(decimal current, decimal total, T value, string caption) : base(current, total, caption)
		{ 
			_Value = value;
			
			Trace.TraceWarning(caption + " : " + Caption);
		}
		
		public ProgressEventArgs(decimal current, decimal total, T value) : this(current, total, value, null)
		{ }
	}
	/*
	public class Progress
	{		
		private readonly SynchronizationContext _Context;
		protected SynchronizationContext Context
		{ get { return _Context; } }
		
		private readonly CancellationToken _CancellationToken;
		public CancellationToken CancellationToken
		{ get { return _CancellationToken; } }
		
		public event EventHandler<ProgressEventArgs> ProgressChanged;

		public Progress()
		{ 
			this._Context = (SynchronizationContext.Current ?? SynchronizationContextExtensions.DefaultContext);
			this._CancellationToken = new CancellationToken();
		}
		
		public Progress(SynchronizationContext context, CancellationToken cancellationToken)
		{
			this._Context = context;
			this._CancellationToken = cancellationToken;
		}
		
		public Progress(SynchronizationContext context)
		{ 
			this._Context = context;
			this._CancellationToken = new CancellationToken();
		}
		
		public Progress(CancellationToken cancellationToken)
		{
			this._Context = (SynchronizationContext.Current ?? SynchronizationContextExtensions.DefaultContext);
			this._CancellationToken = cancellationToken;
		}
				
		public void NotifyProgressChanged(ProgressEventArgs value)
		{
			EventHandler<ProgressEventArgs> progressChanged = this.ProgressChanged;
			if (progressChanged != null)
			{
				Action action = new Action(()=>ProgressChanged(this, value));
				this.Context.Post(action);
			}	
		}
		
		public void NotifyProgressChanged(int current, int total)
		{
			ProgressEventArgs e = new ProgressEventArgs(current, total);
			NotifyProgressChanged(e);
		}
	}
	
	public class Progress<T> : Progress
	{
		private readonly BlockingCollection<T> _Items;
		public BlockingCollection<T> Items
		{ get { return _Items; } }
		
		public Progress() : base()
		{ _Items = new BlockingCollection<T>(); }
		
		public Progress(SynchronizationContext context, CancellationToken cancellationToken) : base(context, cancellationToken)
		{ _Items = new BlockingCollection<T>(); }
		
		public Progress(SynchronizationContext context) : base(context)
		{ _Items = new BlockingCollection<T>(); }
		
		public Progress(CancellationToken cancellationToken) : base(cancellationToken)
		{ _Items = new BlockingCollection<T>(); }
		
		public Progress(SynchronizationContext context, CancellationToken cancellationToken, BlockingCollection<T> items) : base(context, cancellationToken)
		{ _Items = items; }
		
		public Progress(SynchronizationContext context, BlockingCollection<T> items) : base(context)
		{ _Items = items; }
		
		public Progress(CancellationToken cancellationToken, BlockingCollection<T> items) : base(cancellationToken)
		{ _Items = items; }
		
		public Progress(BlockingCollection<T> items) : base()
		{ _Items = items; }
	}
	*/
	
	/*
	
	public class Progress
	{
		/// <summary> 
		/// The underlying scheduler for the UI's synchronization context. 
		/// </summary> 
		private readonly TaskScheduler _TaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
		/// <summary> 
		/// Gets the task scheduler which executes tasks on the UI thread. 
		/// </summary> 
		public TaskScheduler TaskScheduler
		{ get { return this._TaskScheduler;; } }

		private readonly CancellationToken _CancellationToken = CancellationToken.None;	

		public CancellationToken CancellationToken
		{ get { return this._CancellationToken; } }
		
		/// <summary> 
		/// Initializes a new instance of the <see cref="Progress"/> class.
		/// This should be run on a UI thread. 
		/// </summary>
		public Progress()
		{ }
		
		public Progress(TaskScheduler taskScheduler)
		{ this._TaskScheduler = taskScheduler; }
		
		public Progress(CancellationToken cancellationToken)
		{ this._CancellationToken = CancellationToken.None; }
		
		public Progress(TaskScheduler taskScheduler, CancellationToken cancellationToken)
		{
			this._TaskScheduler = taskScheduler;
			this._CancellationToken = cancellationToken;
		}
		
		public event EventHandler<ProgressEventArgs> ProgressChanged;
		
		/// <summary>
		/// Raise the ProgressChanged event with the provided ProgressEventArgs
		/// </summary>
		/// <remarks>Blocks until the message has been received</remarks>
		public void NotifyProgressChanged(ProgressEventArgs e)
		{				
			EventHandler<ProgressEventArgs> handler = ProgressChanged;
			if (handler != null)
			{
				//Post the event back to the caller's thread.
				//We must block until the event is recieved to enforce event order.
				//TODO: Rework to use async reporting (synchonizationContext?)
				Task.Factory.StartNew(()=>handler(this, e), CancellationToken.None, TaskCreationOptions.None, TaskScheduler).Wait();
			}
		}
		
		
	}
	
	public class ProducerProgress<T> : Progress
	{		
		public BlockingCollection<T> Items = new BlockingCollection<T>();
		
		public ProducerProgress() : base()
		{ }
		
		public ProducerProgress(TaskScheduler taskScheduler) : base(taskScheduler)
		{ }
		
		public ProducerProgress(TaskScheduler scheduler, CancellationToken cancellationToken) : base(scheduler, cancellationToken)
		{ }
		
		public ProducerProgress(CancellationToken cancellationToken) : base(cancellationToken)
		{ }
	}
	
	*/
}
