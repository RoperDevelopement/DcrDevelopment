using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using EdocsUSA.Utilities.Extensions;

namespace EdocsUSA.Utilities.Controls
{

	public class ProgressMonitor : UserControl
	{
		protected ProgressBarEx ProgressBar;
		protected Button CancelTaskButton;
		
		private void InitializeComponent()
		{
			this.MinimumSize = new Size(125, 25);
			
			ProgressBar = new ProgressBarEx()
			{
				Dock = DockStyle.Fill
			};
			Controls.Add(ProgressBar);
			
			CancelTaskButton = new Button() 
			{
				Text="X",
				Enabled = false,
				Dock = DockStyle.Right,
				AutoSize = true,
				AutoSizeMode = AutoSizeMode.GrowAndShrink
			};
			Controls.Add(CancelTaskButton);
		}
		
		public ProgressMonitor()
		{ InitializeComponent(); }
		
		protected bool _Busy = false;
		/// <summary>True if currently monitoring a Progress, false if not.</summary>
		public bool Busy 
		{
			get { return _Busy; }
			protected set { _Busy = value; }
		}
		
		/// <summary>Throws an InvalidOperationException <see cref="Busy">is true</see></summary>
		public void EnsureNotBusy()
		{
			if (Busy)
			{ throw new InvalidOperationException("Busy, cannot monitor multiple tasks"); }
		}
		
		protected Progress Progress;
		protected CancellationTokenSource CancellationTokenSource;
		protected Task Task;		
		
		public void StartMonitoring(Task task, Progress progress, CancellationTokenSource cancellationTokenSource)
		{
			EnsureNotBusy();
			Busy = true;
			
			Progress = progress;
			CancellationTokenSource = cancellationTokenSource;
			Task = task;
			
			Task.ContinueWith(_=>
         {
         	try
         	{
	         	switch (Task.Status)
	         	{
	         		case TaskStatus.Faulted:
	         			Exception ex = Task.Exception.GetBaseException();
	         			if (ex is OperationCanceledException)
	         			{ goto case TaskStatus.Canceled; }
	         			ProgressBar.BarColor = Color.Red;
							ProgressBar.Caption = "Failed";         			
							ProgressBar.ToolTipText = ex.Message;
	         			break;
	         		case TaskStatus.Canceled:
	         			ProgressBar.BarColor = Color.Orange;
							ProgressBar.Caption = "Canceled";         			
	         			break;
	         		case TaskStatus.RanToCompletion:
	         			ProgressBar.BarColor = Color.LightGreen;
							ProgressBar.Caption = "Completed";         			
	         			break;
	         		default:
	         			Trace.TraceWarning("Unexpected task status " + Task.Status);
	         			break;
	         	}
         	}
         	finally
         	{ 
         		Busy = false;
         		this.CancelTaskButton.Enabled = false;
         	}
         	
         }, CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
			
			this.CancelTaskButton.Enabled = true;
			ProgressBar.BarColor = SystemColors.ControlDark;
			ProgressBar.ToolTipText = null;
			
			CancelTaskButton.Click += CancelTaskButton_Click;
			Progress.ProgressChanged += Progress_ProgressChanged;
			
		}
		
		public void CancelTaskButton_Click(object sender, EventArgs e)
		{ CancellationTokenSource.Cancel(); }
		
		protected void Progress_ProgressChanged(object sender, ProgressEventArgs e)
		{
			ProgressBar.Value = e.PercentComplete;
			ProgressBar.Caption = e.ToString();
		}	
	}
}
