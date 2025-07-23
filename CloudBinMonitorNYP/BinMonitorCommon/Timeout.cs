using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BinMonitor.Common
{
    [Serializable]
    public class Timeout
    {
        public bool Enabled { get; set; }
        
        public TimeSpan Duration { get; set; }

        public DateTime _LastAction = DateTime.MinValue;
        private DateTime LastAction
        {
            get { return _LastAction; }
            set { _LastAction = value; }
        }

        public bool Ellapsed
        {
            get
            {
                if (Enabled == false)
                { return false; }

                TimeSpan elapsed = ((TimeSpan)(DateTime.Now - LastAction));
                return (elapsed > Duration);
            }
        }

        public Timeout()
        { Reset(); }

        public Timeout(TimeSpan duration)
            : this()
        { Duration = duration; }

        public void Reset()
        { LastAction = DateTime.Now; }
    }
}
