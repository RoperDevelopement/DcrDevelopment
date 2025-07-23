using Polenter.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BinMonitor.Common
{
    [Serializable]
    public class CheckPointConfiguration : INotifyPropertyChanged
    {
        #region Events 

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            { this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName)); }
        }

        #endregion Events

        #region Properties

        private TimeSpan _Duration = TimeSpan.MaxValue;
        public TimeSpan Duration
        {
            get { return _Duration; }
            set 
            { 
                _Duration = value;
                OnPropertyChanged("Duration");
            }
        }

        private bool _Flash = false;
        public bool Flash
        {
            get { return _Flash; }
            set 
            { 
                _Flash = value;
                OnPropertyChanged("Flash");
            }
        }

        private SerializableColor _Color = new SerializableColor(SystemColors.Control);
        public SerializableColor Color
        {
            get { return _Color; }
            set { _Color = value; }
        }

        private bool _EmailImmediately = false;
        public bool EmailImmediately
        {
            get { return _EmailImmediately; }
            set 
            { 
                _EmailImmediately = value;
                OnPropertyChanged("_EmailImmediately");
            }
        }

        private bool _EmailUntilCompelte = false;
        public bool EmailUntilComplete
        {
            get { return _EmailUntilCompelte; }
            set 
            { 
                _EmailUntilCompelte = value;
                OnPropertyChanged("EmailUntilComplete");
            }
        }

        private TimeSpan _EmailFrequency = TimeSpan.MaxValue;
        public TimeSpan EmailFrequency
        {
            get { return _EmailFrequency; }
            set 
            { 
                _EmailFrequency = value;
                OnPropertyChanged("EmailFrequency");
            }
        }

        private string _EmailRecipients = null;
        public string EmailRecipients
        { 
            get {return _EmailRecipients;} 
            set 
            {
                _EmailRecipients = value;
                OnPropertyChanged("EmailRecipients");
            }
        }

        #endregion Properties

        #region Event Handlers

        protected void OnPropertyChanged(string propertyName)
        { NotifyPropertyChanged(propertyName); }

        #endregion Event Handlers
    }

    [Serializable]
    public class CheckPoint : INotifyPropertyChanged
    {
        #region Events

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            { this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName)); }
        }

        public event EventHandler ConfigurationChanged;
        protected void NotifyConfigurationChanged()
        {
            if (this.ConfigurationChanged != null)
            { this.ConfigurationChanged(this, null); }
        }

        #endregion Events

        private DateTime _Origin = DateTime.Now;
        public DateTime Origin
        {
            get { return _Origin; }
            set 
            {
                _Origin = value;
                OnPropertyChanged("Origin");
            }
        }

        private CheckPointConfiguration _Configuration = null;
        public CheckPointConfiguration Configuration
        {
            get { return _Configuration; }
            set 
            { 
                //If there was a previous value, unregister the events
                if (_Configuration != null)
                { _Configuration.PropertyChanged -= OnConfiguration_PropertyChanged; }

                //If there is a new value, register the events
                if (value != null)
                { value.PropertyChanged += OnConfiguration_PropertyChanged; }
                
                _Configuration = value;
                OnConfigurationChanged();
            }
        }

        private DateTime _EmailLastSentAt = DateTime.MinValue;
        public DateTime EmailLastSentAt
        {
            get { return _EmailLastSentAt; }
            set 
            { 
                _EmailLastSentAt = value;
                OnPropertyChanged("EmailLastSentAt");
            }
        }

        private bool EmailSent
        { get { return (_EmailLastSentAt > DateTime.MinValue); } }

        private bool EmailDue(DateTime origin, DateTime current)
        {
            if (this.Elapsed() == false)
            { return false; }

            if (Configuration.EmailImmediately && EmailSent == false)
            { return true; }

            TimeSpan timeSinceLastEmail;
            if (EmailSent == true)
            { timeSinceLastEmail = (TimeSpan)(current - this.EmailLastSentAt); }
            else
            { timeSinceLastEmail = (TimeSpan)(current - origin); }

            if (timeSinceLastEmail > Configuration.EmailFrequency)
            { return true; }

            return false;            
        }

        public bool EmailDue()
        { return EmailDue(this.Origin, DateTime.Now); }

        public bool Elapsed(DateTime origin, DateTime current)
        { return ((TimeSpan)(current - origin)) > this.Configuration.Duration; }

        public bool Elapsed(DateTime origin)
        { return this.Elapsed(origin, DateTime.Now); }

        public bool Elapsed()
        { return this.Elapsed(this.Origin); }

        public DateTime ElapsesAt(DateTime origin)
        { return origin.Add(Configuration.Duration); }

        public DateTime ElapsesAt()
        { return ElapsesAt(this.Origin); }

        public TimeSpan RemainingTime(DateTime origin)
        { return (TimeSpan)(this.ElapsesAt() - origin); }

        public TimeSpan RemainingTime()
        { return RemainingTime(DateTime.Now); }

        public CheckPoint() { }

        public CheckPoint(CheckPointConfiguration configuration)
            : this()
        { this.Configuration = configuration; }

        public CheckPoint(CheckPointConfiguration configuration, DateTime origin)
            : this(configuration)
        { this.Origin = origin; }

        #region Event Handlers

        protected void OnPropertyChanged(string propertyName)
        { NotifyPropertyChanged(propertyName); }

        protected void OnConfigurationChanged()
        { NotifyConfigurationChanged(); }

        protected void OnConfiguration_PropertyChanged(object sender, PropertyChangedEventArgs e)
        { OnConfigurationChanged(); }

        #endregion Event Handlers
    }
}
