using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BinMonitor.Common
{

    [Serializable]
    public class WorkflowStepConfiguration : INotifyPropertyChanged
    {
        #region Events

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            { this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName)); }
        }
        
        #endregion

        private bool _EmailOnStart = false;
        public bool EmailOnStart
        {
            get { return _EmailOnStart; }
            set 
            { 
                _EmailOnStart = value;
                NotifyPropertyChanged("EmailOnStart");
            }
        }

        private bool _EmailOnCompletion = false;
        public bool EmailOnCompletion
        {
            get { return _EmailOnCompletion; }
            set 
            { 
                _EmailOnCompletion = value;
                NotifyPropertyChanged("EmailOnCompletion");
            }
        }

        private bool _IncludeContentsInEmail = false;
        public bool IncludeContentsInEmail
        {
            get { return _IncludeContentsInEmail; }
            set 
            { 
                _IncludeContentsInEmail = value;
                NotifyPropertyChanged("IncludeContentsInEmail");
            }
        }

        private string _EmailRecipients = null;
        public string EmailRecipients 
        {
            get { return _EmailRecipients; }
            set
            {
                _EmailRecipients = value;
                NotifyPropertyChanged("EmailRecipients");
            }
        }

        #region Event Handlers

        protected void OnPropertyChanged(string propertyName)
        { NotifyPropertyChanged(propertyName); }

        #endregion Event Handlers
    }

    [Serializable]
    public class WorkflowStep : INotifyPropertyChanged
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

        public event EventHandler Started;
        protected void NotifyStarted()
        {
            if (this.Started != null)
            { this.Started(this, null); }
        }

        public event EventHandler Completed;
        protected void NotifyCompleted()
        {
            if (this.Completed != null)
            { this.Completed(this, null); }
        }

        #endregion Events

        private WorkflowStepConfiguration _Configuration = null;
        public WorkflowStepConfiguration Configuration
        {
            get { return _Configuration; }
            set 
            { 
                //If there was a previous value, unregister the events
                if (_Configuration != null)
                {
                    _Configuration.PropertyChanged -= OnConfiguration_PropertyChanged;
                }
                _Configuration = value; 
            }
        }

        private DateTime _StartedAt = DateTime.MinValue;
        public DateTime StartedAt
        {
            get { return _StartedAt; }
            set 
            { 
                _StartedAt = value;
                OnPropertyChanged("StartedAt");
            }
        }

        public bool HasStarted
        { get { return this.StartedAt > DateTime.MinValue; } }

        private string _AssignedBy = null;
        public string AssignedBy
        {
            get { return _AssignedBy; }
            set
            {
                _AssignedBy = value;
                OnPropertyChanged("AssignedBy");
            }
        }

        private string _AssignedTo = null;
        public string AssignedTo
        {
            get { return _AssignedTo; }
            set
            {
                _AssignedTo = value;
                OnPropertyChanged("AssignedTo");
            }
        }

        private DateTime _CompletedAt = DateTime.MinValue;
        public DateTime CompletedAt 
        {
            get { return _CompletedAt; }
            set 
            {
                _CompletedAt = value;
                OnPropertyChanged("CompletedAt");
            }
        }

        public bool HasCompleted
        { get { return (this.CompletedAt > DateTime.MinValue); } }

        private string _CompletedBy = null;
        public string CompletedBy
        {
            get { return _CompletedBy; }
            set { _CompletedBy = value; }
        }
 

        /// <summary>
        /// The time since the step began, or if the step is completed the total time the step took.
        /// </summary>
        public TimeSpan GetDuration(DateTime current)
        {
            if (HasCompleted == true)
            { return ((TimeSpan)(CompletedAt - StartedAt)); }
            else
            { return ((TimeSpan)(current - StartedAt)); }
        }

        /// <summary>
        /// The time since the step began, or if the step is completed the total time the step took.
        /// </summary>
        public TimeSpan GetDuration()
        { return GetDuration(DateTime.Now); }

        public WorkflowStep()
        { }

        public WorkflowStep(WorkflowStepConfiguration configuration) 
            : this()
        { this.Configuration = configuration; }

        public void Start(string assignedBy, string assignedTo, DateTime startedAt) 
        {   
            this.AssignedBy = assignedBy;
            this.AssignedTo = assignedTo;
            this.StartedAt = startedAt;
            OnStarted();
        }

        public void Start(string assignedBy, string assignedTo)
        { this.Start(assignedBy, assignedTo, DateTime.Now); }

        public void Complete(string completedBy, DateTime completedAt)
        {
            this.CompletedBy = completedBy;
            this.CompletedAt = completedAt;
            OnCompleted();
        }

        public void Complete(string completedBy)
        { this.Complete(completedBy, DateTime.Now); }

        #region Event Handlers

        protected void OnPropertyChanged(string propertyName)
        { NotifyPropertyChanged(propertyName); }

        protected void OnConfigurationChanged()
        { NotifyConfigurationChanged(); }

        protected void OnConfiguration_PropertyChanged(object sender, PropertyChangedEventArgs e)
        { OnConfigurationChanged(); }

        protected void OnStarted()
        { NotifyStarted(); }

        protected void OnCompleted()
        { NotifyCompleted(); }

        #endregion Event Handlers
    }
}
