/*
 * User: Sam Brinly
 * Date: 11/19/2014
 */
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.IO;
using System.Diagnostics;

namespace BinMonitor.Common
{
    [Serializable]
	public class SpecimenBatch : INotifyPropertyChanged
    {
        public static string GenerateNewId()
        { return Guid.NewGuid().ToString(); }

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            { this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName)); }
        }

        public event EventHandler WorkflowStepChanged;
        protected void NotifyWorkflowStepChanged()
        {
            if (this.WorkflowStepChanged != null)
            { this.WorkflowStepChanged(this, null); }
        }

        public event EventHandler CheckpointChanged;
        protected void NotifyCheckpointChanged()
        {
            if (this.CheckpointChanged != null)
            { this.CheckpointChanged(this, null); }
        }

        public event EventHandler Closed;
        public void NotifyClosed()
        {
            if (this.Closed != null)
            { this.Closed(this, null); }
        }

        #endregion Events

        private string _Id = null;
		public string Id 
		{
			get { return _Id; }
			set 
            { 
                _Id = value;
                OnPropertyChanged("Id");
            }
		}

        private string _BinId = null;
        public string BinId
        {
            get { return _BinId; }
            set 
            { 
                _BinId = value;
                OnPropertyChanged("BinId");
            }
        }

        private string _TransferredFrom = null;
        public string TransferredFrom
        {
            get { return _TransferredFrom; }
            set 
            { 
                _TransferredFrom = value;
                OnPropertyChanged("TransferredFrom");
            }
        }

        private string _CategoryId = null;
        public string CategoryId
        {
            get { return _CategoryId; }
            set
            {
                _CategoryId = value;
                OnPropertyChanged("CategoryId");
                OnPropertyChanged("Category");
            }
        }

        public Category Category
        {
            get 
            {
                Category category;
                if (SpecimenCategories.Instance.TryGetValue(this.CategoryId, out category) == false)
                { return null; }
                return category;
            }
        }

        public Category EnsureGetCategory()
        {
            Category category = this.Category;
            if (category == null)
            { throw new InvalidOperationException("The requested operation requires a category to be assigned"); }
            return category;
        }

        private CheckPoint _CheckPoint1 = null;
        public CheckPoint CheckPoint1
        {
            get { return _CheckPoint1; }
            set
            {
                //If there was a previous value, unregister the event handlers
                if (_CheckPoint1 != null)
                {
                    _CheckPoint1.ConfigurationChanged -= OnCheckpoint_ConfigurationChanged;
                    _CheckPoint1.PropertyChanged -= OnCheckpoint_PropertyChanged;
                }
                //If there is a new value, register the event handlers
                if (value != null)
                {
                    value.ConfigurationChanged += OnCheckpoint_ConfigurationChanged;
                    value.PropertyChanged += OnCheckpoint_PropertyChanged;
                }
                _CheckPoint1 = value;
                NotifyCheckpointChanged();
            }
        }

        private CheckPoint _CheckPoint2 = null;
        public CheckPoint CheckPoint2
        {
            get { return _CheckPoint2; }
            set
            {
                //If there was a previous value, unregister the event handlers
                if (_CheckPoint2 != null)
                {
                    _CheckPoint2.ConfigurationChanged -= OnCheckpoint_ConfigurationChanged;
                    _CheckPoint2.PropertyChanged -= OnCheckpoint_PropertyChanged;
                }
                //If there is a new value, register the event handlers
                if (value != null)
                {
                    value.ConfigurationChanged += OnCheckpoint_ConfigurationChanged;
                    value.PropertyChanged += OnCheckpoint_PropertyChanged;
                }
                _CheckPoint2 = value;
                NotifyCheckpointChanged();
            }
        }

        private CheckPoint _CheckPoint3 = null;
        public CheckPoint CheckPoint3
        {
            get { return _CheckPoint3; }
            set
            {
                //If there was a previous value, unregister the event handlers
                if (_CheckPoint3 != null)
                {
                    _CheckPoint3.ConfigurationChanged -= OnCheckpoint_ConfigurationChanged;
                    _CheckPoint3.PropertyChanged -= OnCheckpoint_PropertyChanged;
                }
                //If there is a new value, register the event handlers
                if (value != null)
                {
                    value.ConfigurationChanged += OnCheckpoint_ConfigurationChanged;
                    value.PropertyChanged += OnCheckpoint_PropertyChanged;
                }
                _CheckPoint3 = value;
                NotifyCheckpointChanged();
            }
        }
		
		private CheckPoint _CheckPoint4 = null;
		public CheckPoint CheckPoint4
		{ 
            get { return _CheckPoint4; } 
            set 
            { 
                //If there was a previous value, unregister the event handlers
                if (_CheckPoint4 != null)
                {
                    _CheckPoint4.ConfigurationChanged -= OnCheckpoint_ConfigurationChanged;
                    _CheckPoint4.PropertyChanged -= OnCheckpoint_PropertyChanged;
                }
                //If there is a new value, register the event handlers
                if (value != null)
                {
                    value.ConfigurationChanged += OnCheckpoint_ConfigurationChanged;
                    value.PropertyChanged += OnCheckpoint_PropertyChanged;
                }
                _CheckPoint4 = value;
                NotifyCheckpointChanged();
            }
        }

        private DateTime _CheckpointOrigin = DateTime.MinValue;
        public DateTime CheckpointOrigin
        {
            get { return _CheckpointOrigin; }
            set
            {
                _CheckpointOrigin = value;
                OnPropertyChanged("CheckpointOrigin");
            }
        }

        private string _CreatedBy = null;
        public string CreatedBy
        {
            get { return _CreatedBy; }
            set
            {
                _CreatedBy = value;
                OnPropertyChanged("CreatedBy");
            }
        }

        private DateTime _CreatedAt = DateTime.MinValue;
        public DateTime CreatedAt
        {
            get { return _CreatedAt; }
            set
            {
                _CreatedAt = value;
                OnPropertyChanged("CreatedAt");
            }
        }
            /* 
        private WorkflowStep _Creation = null;
		public WorkflowStep Creation 
		{ 
            get { return _Creation; } 
            set
            {
                //If there was a previous value, unregister the event handlers
                if (_Creation != null)
                {
                    _Creation.Completed -= this.OnWorkflowStep_Completed;
                    _Creation.ConfigurationChanged -= this.OnWorkflowStep_ConfigurationChanged;
                    _Creation.PropertyChanged -= this.OnWorkflowStep_PropertyChanged;
                    _Creation.Started -= this.OnWorkflowStep_Started;
                }
                //If there is a new value, register the event handlers
                if (value != null)
                {   
                    value.Completed += this.OnWorkflowStep_Completed;
                    value.ConfigurationChanged += this.OnWorkflowStep_ConfigurationChanged;
                    value.PropertyChanged += this.OnWorkflowStep_PropertyChanged;
                    value.Started += this.OnWorkflowStep_Started;
                }

                _Creation = value;
                OnWorkflowStepChanged();
            }
        }
        
    */
        private WorkflowStep _Registration = new WorkflowStep();
		public WorkflowStep Registration
		{ 
            get { return _Registration; } 
            set
            {
                //If there was a previous value, unregister the event handlers
                if (_Registration != null)
                {
                    _Registration.Completed -= this.OnWorkflowStep_Completed;
                    _Registration.ConfigurationChanged -= this.OnWorkflowStep_ConfigurationChanged;
                    _Registration.PropertyChanged -= this.OnWorkflowStep_PropertyChanged;
                    _Registration.Started -= this.OnWorkflowStep_Started;
                }
                //If there is a new value, register the event handlers
                if (value != null)
                {
                    value.Completed += this.OnWorkflowStep_Completed;
                    value.ConfigurationChanged += this.OnWorkflowStep_ConfigurationChanged;
                    value.PropertyChanged += this.OnWorkflowStep_PropertyChanged;
                    value.Started += this.OnWorkflowStep_Started;
                }

                _Registration = value;
                OnWorkflowStepChanged();
            }
        }

        private WorkflowStep _Processing = new WorkflowStep();
		public WorkflowStep Processing
		{ 
            get { return _Processing; } 
            set
            {
                //If there was a previous value, unregister the event handlers
                if (_Processing != null)
                {
                    _Processing.Completed -= this.OnWorkflowStep_Completed;
                    _Processing.ConfigurationChanged -= this.OnWorkflowStep_ConfigurationChanged;
                    _Processing.PropertyChanged -= this.OnWorkflowStep_PropertyChanged;
                    _Processing.Started -= this.OnWorkflowStep_Started;
                }
                //If there is a new value, register the event handlers
                if (value != null)
                {
                    value.Completed += this.OnWorkflowStep_Completed;
                    value.ConfigurationChanged += this.OnWorkflowStep_ConfigurationChanged;
                    value.PropertyChanged += this.OnWorkflowStep_PropertyChanged;
                    value.Started += this.OnWorkflowStep_Started;
                }

                _Processing = value;
                OnWorkflowStepChanged();
            }
        }

        private string _ClosedBy = null;
        public string ClosedBy
        {
            get { return _ClosedBy; }
            set 
            { 
                _ClosedBy = value;
                OnPropertyChanged("ClosedBy");
                
            }
        }

        private DateTime _ClosedAt = DateTime.MinValue;
        public DateTime ClosedAt
        {
            get { return _ClosedAt; }
            set
            {
                _ClosedAt = value;
                OnPropertyChanged("ClosedAt");
            }
        }
        private bool _IsClosed;
        public bool IsClosed
        {
            get { return _IsClosed; }
            set { _IsClosed = value; }
        }

        public bool IsCheckedOutForRegistration
        { get { return (Registration.HasStarted && (Registration.HasCompleted == false)); } }

        public bool IsCheckedOutForProcessing
        { get { return (Processing.HasStarted && (Processing.HasCompleted == false)); } }

        public bool IsCheckedOut
        { get { return (IsCheckedOutForRegistration || IsCheckedOutForProcessing); } }        
		
		private List<string> _Specimens = new List<string>();
		public List<string> Specimens
		{ 
            get { return _Specimens; }
            set 
            {
                _Specimens = value;
                OnPropertyChanged("Specimens");
            }
        }

        private string _Comments = null;
        public string Comments 
        {
            get { return _Comments; }
            set
            {
                _Comments = value;
                OnPropertyChanged("Comments");
            }
        }

        /// <summary>Constructor for serialization only</summary>
		public SpecimenBatch()
		{ }

        public SpecimenBatch(string createdBy, DateTime createdAt)
            : this()
        {
            this.CreatedBy = createdBy;
            this.CreatedAt = createdAt;
        }

        public SpecimenBatch(string createdBy)
            : this()
        {
            this.CreatedBy = createdBy;
            this.CreatedAt = DateTime.Now;
        }

        public void AddComment(string comment, string userName, DateTime timeStamp)
        { 
            string newComment = FormatComment(comment, userName, timeStamp);
            if (string.IsNullOrWhiteSpace(this.Comments))
            { this.Comments = newComment; }
            else
            { this.Comments = string.Join(Environment.NewLine, newComment, this.Comments); }
        }

        public MasterCategoryPermissions EnsureGetPermissionsByUser(User user)
        {
            return user.EnsureGetUserProfile().CategoryPermissions[this.EnsureGetCategory().MasterCategoryTitle];
        }

        public static string FormatComment(string comment, string userName, DateTime timeStamp)
        { return string.Format("{0} - (at {1} by {2})", comment, timeStamp.ToString("yyyy-MM-dd HH:mm:ss"), userName); }

        #region Event Handlers

        protected void OnPropertyChanged(string propertyName)
        { NotifyPropertyChanged(propertyName); }

        protected void OnWorkflowStepChanged()
        { NotifyWorkflowStepChanged(); }

        protected void OnWorkflowStep_PropertyChanged(object sender, PropertyChangedEventArgs e)
        { OnWorkflowStepChanged(); }

        protected void OnWorkflowStep_ConfigurationChanged(object sender, EventArgs e)
        { OnWorkflowStepChanged(); }

        protected void OnWorkflowStep_Started(object sender, EventArgs e)
        { OnWorkflowStepChanged(); }

        protected void OnWorkflowStep_Completed(object sender, EventArgs e)
        { OnWorkflowStepChanged(); }

        protected void OnCheckpointChanged()
        { NotifyCheckpointChanged(); }

        protected void OnCheckpoint_PropertyChanged(object sender, PropertyChangedEventArgs e)
        { OnCheckpointChanged(); }

        protected void OnCheckpoint_ConfigurationChanged(object sender, EventArgs e)
        { OnCheckpointChanged(); }

        protected void OnClosed()
        { NotifyClosed(); }

        protected void OnClosed(object sender, EventArgs e)
        { OnClosed(); }

        #endregion Event Handlers
    }



}
