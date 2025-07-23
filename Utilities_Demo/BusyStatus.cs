using System;

namespace EdocsUSA.Utilities
{
	public class BusyStatus
	{
		
		private string _BusyMessage = "The requested operation could not be performed while Busy is true";
		public string BusyMessage
		{
			get { return _BusyMessage; }
			set { _BusyMessage = value; }
		}
	
		private bool _Busy = false;
		public bool Busy
		{
			get { return _Busy; }
			private set 
			{
				_Busy = value;
				OnStatusChanged();
			}
		}
		
		public event EventHandler StatusChanged;
		
		protected virtual void OnStatusChanged()
		{
			EventHandler handler = StatusChanged;
			if (handler != null)
			{ handler(this, null); }
		}
		
		public BusyStatus()
		{ }
		
		public BusyStatus(bool initialState) : this()
		{ Busy = initialState; }
		
		public void EnsureNotBusy()
		{
			if (Busy == true)
			{ throw new InvalidOperationException(BusyMessage); }
		}
		
		public void Set()
		{
			EnsureNotBusy();
			Busy = true;
		}
		
		public void Clear()
		{ Busy = false; }
		
		public void PerformBusyingAction(Action action)
		{
			EnsureNotBusy();
			try
			{
				Busy = true;
				action();
			}
			finally
			{ Busy = false; }
		}
	}
}
