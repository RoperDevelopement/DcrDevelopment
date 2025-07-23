using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace EdocsUSA.Utilities
{
	public class EventWaiter
	{
		private bool _State = false;
		public bool State
		{
			get 
			{ 
				if (_State == false) return false;
				else
				{
					_State = false;
					return true;
				}
			}
		}
		
		public void Wait(int interval, int timeout)
		{
			DateTime startTime = DateTime.Now;
			while (_State == false)
			{
				DateTime currentTime = startTime;
				int runTime = (int)(((TimeSpan)(currentTime - startTime)).TotalMilliseconds);
				if ((timeout > 0) && (runTime > timeout))
				{
                    Logging.TraceLogger.TraceLoggerInstance.TraceError($"Timeout wating for event interval:{interval.ToString()} timeout:{timeout.ToString()}");
                    throw new TimeoutException();
                }
				Application.DoEvents();
				Thread.Sleep(interval);
			}
			_State = false;
		}
		
		public void Reset() { _State = false; }
		
		public void Set() { _State = true; }
		
		public static EventWaiter Wait(params EventWaiter[] eventWaiters)
		{ return Wait(eventWaiters.ToList()); }
		
		public static EventWaiter Wait(IEnumerable<EventWaiter> eventWaiters)
		{ return Wait(eventWaiters, 25, 0); }
		
		public static EventWaiter Wait(IEnumerable<EventWaiter> eventWaiters, int interval)
		{ return Wait(eventWaiters, interval, 0); }
		
		public static EventWaiter Wait(IEnumerable<EventWaiter> eventWaiters, int interval, int timeout)
		{
			DateTime startTime = DateTime.Now;
			while (true)
			{
				foreach (EventWaiter eventWaiter in eventWaiters)
				{
					if (eventWaiter.State == true)
					{ return eventWaiter; }
				}
				
				DateTime currentTime = DateTime.Now;
				int runTime = (int)(((TimeSpan)(currentTime - startTime)).TotalMilliseconds);
				if ((timeout > 0) && (runTime > timeout)) 
				{
                    Logging.TraceLogger.TraceLoggerInstance.TraceError($"Timeout wating for event interval:{interval.ToString()} timeout:{timeout.ToString()}");
                    throw new TimeoutException();
                }
				//Trace.TraceInformation("Waiting");
				Application.DoEvents();
				Thread.Sleep(interval);
			}
		}
		
		public static void Reset(IEnumerable<EventWaiter> eventWaiters)
		{
			foreach (EventWaiter eventWaiter in eventWaiters)
			{ eventWaiter.Reset(); }
		}
		
		public static void Reset(params EventWaiter[] eventWaiters)
		{ Reset(eventWaiters.ToList()); }
		
		public EventWaiter()
		{
		}
	}
}
