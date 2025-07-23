/*
 * User: Sam Brinly
 * Date: 1/29/2014
 */
using System;
using System.ComponentModel;

namespace EdocsUSA.Utilities
{
	/// <credit>MaLio http://stackoverflow.com/questions/1351138/bindinglist-listchanged-event</credit>
	public class SynchronizedBindingList<T> : BindingList<T>
	{
		private ISynchronizeInvoke _Host = null;

		public SynchronizedBindingList(ISynchronizeInvoke host)
		{
			_Host = host;
		}

		public SynchronizedBindingList()
			: this(null)
		{ }

		protected override void OnListChanged(ListChangedEventArgs e)
		{
			Action action = new Action(()=> base.OnListChanged(e));
			if ((_Host == null) || (_Host.InvokeRequired == false))
			{ action(); }
			else
			{ _Host.Invoke(action, new object[0]); }
		}
	}
}
