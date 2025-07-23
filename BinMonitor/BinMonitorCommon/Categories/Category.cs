using Polenter.Serialization;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Xml.Serialization;

namespace BinMonitor.Common
{	
	[Serializable]
	public class Category : INotifyPropertyChanged
    {
        #region Events

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        { 
            if (this.PropertyChanged != null)
            { this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName)); }
        }

        public event EventHandler CheckpointConfigurationChanged;
        protected void NotifyCheckpointConfigurationChanged()
        {
            if (this.CheckpointConfigurationChanged != null)
            { this.CheckpointConfigurationChanged(this, null); }
        }

        public event EventHandler WorkflowStepConfigurationChanged;
        protected void NotifyWorkflowStepConfigurationChanged()
        {
            if (this.WorkflowStepConfigurationChanged != null)
            { this.WorkflowStepConfigurationChanged(this, null); }
        }

        #endregion Events

        private string _Title = null;
        public string Title 
        {
            get { return _Title; }
            set
            {
                _Title = value;
                OnPropertyChanged("Title");
            }
        }

        private SerializableColor _Color = new SerializableColor(SystemColors.Control);
        public SerializableColor Color
        {
            get { return _Color; }
            set 
            { 
                _Color = value;
                OnPropertyChanged("Color");
            }
        }

        private string _MasterCategoryTitle = null;
        public string MasterCategoryTitle
        {
            get { return _MasterCategoryTitle; }
            set 
            { 
                _MasterCategoryTitle = value;
                OnPropertyChanged("MasterCategoryTitle");
            }
        }

        private bool _RequiresRegistration = false;
        public bool RequiresRegistration
        {
            get { return _RequiresRegistration; }
            set { _RequiresRegistration = value; }
        }

        public MasterCategory MasterCategory
        { get { return MasterCategories.Instance.EnsureGetValue(this.MasterCategoryTitle); } }
    

        private CheckPointConfiguration _CheckPoint1Configuration = new CheckPointConfiguration();
        public CheckPointConfiguration CheckPoint1Configuration
        {
            get { return _CheckPoint1Configuration; }
            set
            {
                //If there was a previous value, unregister the event handlers
                if (_CheckPoint1Configuration != null)
                { _CheckPoint1Configuration.PropertyChanged -= OnCheckpointConfiguration_PropertyChanged; }
                //If there is a new value, register the event handlers
                if (value != null)
                { value.PropertyChanged += OnCheckpointConfiguration_PropertyChanged; }
                _CheckPoint1Configuration = value;
                NotifyCheckpointConfigurationChanged();
            }
        }

        private CheckPointConfiguration _CheckPoint2Configuration = new CheckPointConfiguration();
        public CheckPointConfiguration CheckPoint2Configuration
        {
            get { return _CheckPoint2Configuration; }
            set
            {
                //If there was a previous value, unregister the event handlers
                if (_CheckPoint2Configuration != null)
                { _CheckPoint2Configuration.PropertyChanged -= OnCheckpointConfiguration_PropertyChanged; }
                //If there is a new value, register the event handlers
                if (value != null)
                { value.PropertyChanged += OnCheckpointConfiguration_PropertyChanged; }
                _CheckPoint2Configuration = value;
                NotifyCheckpointConfigurationChanged();
            }
        }

        private CheckPointConfiguration _CheckPoint3Configuration = new CheckPointConfiguration();
        public CheckPointConfiguration CheckPoint3Configuration
        {
            get { return _CheckPoint3Configuration; }
            set
            {
                //If there was a previous value, unregister the event handlers
                if (_CheckPoint3Configuration != null)
                { _CheckPoint3Configuration.PropertyChanged -= OnCheckpointConfiguration_PropertyChanged; }
                //If there is a new value, register the event handlers
                if (value != null)
                { value.PropertyChanged += OnCheckpointConfiguration_PropertyChanged; }
                _CheckPoint3Configuration = value;
                NotifyCheckpointConfigurationChanged();
            }
        }

        private CheckPointConfiguration _CheckPoint4Configuration = new CheckPointConfiguration();
        public CheckPointConfiguration CheckPoint4Configuration
        {
            get { return _CheckPoint4Configuration; }
            set
            {
                //If there was a previous value, unregister the event handlers
                if (_CheckPoint4Configuration != null)
                { _CheckPoint4Configuration.PropertyChanged -= OnCheckpointConfiguration_PropertyChanged; }
                //If there is a new value, register the event handlers
                if (value != null)
                { value.PropertyChanged += OnCheckpointConfiguration_PropertyChanged; }
                _CheckPoint4Configuration = value;
                NotifyCheckpointConfigurationChanged();
            }
        }

        private WorkflowStepConfiguration _CreateConfiguration = null;
        public WorkflowStepConfiguration CreateConfiguration 
        {
            get { return _CreateConfiguration; }
            set
            {
                //If there was a previous value, unregister the event handlers
                if (_CreateConfiguration != null)
                { _CreateConfiguration.PropertyChanged -= OnWorkflowStepConfiguration_PropertyChanged; }
                //If there is a new value, register the event handlers
                if (value != null)
                { value.PropertyChanged += OnWorkflowStepConfiguration_PropertyChanged; }

                _CreateConfiguration = value;
                NotifyWorkflowStepConfigurationChanged();
            }
        }

        private WorkflowStepConfiguration _RegisterConfiguration = null;
        public WorkflowStepConfiguration RegisterConfiguration
        {
            get { return _RegisterConfiguration; }
            set
            {
                //If there was a previous value, unregister the event handlers
                if (_RegisterConfiguration != null)
                { _RegisterConfiguration.PropertyChanged -= OnWorkflowStepConfiguration_PropertyChanged; }
                //If there is a new value, register the event handlers
                if (value != null)
                { value.PropertyChanged += OnWorkflowStepConfiguration_PropertyChanged; }

                _RegisterConfiguration = value;
                NotifyWorkflowStepConfigurationChanged();
            }
        }

        private WorkflowStepConfiguration _ProcessConfiguration = null;
        public WorkflowStepConfiguration ProcessConfiguration
        {
            get { return _ProcessConfiguration; }
            set
            {
                //If there was a previous value, unregister the event handlers
                if (_ProcessConfiguration != null)
                { _ProcessConfiguration.PropertyChanged -= OnWorkflowStepConfiguration_PropertyChanged; }
                //If there is a new value, register the event handlers
                if (value != null)
                { value.PropertyChanged += OnWorkflowStepConfiguration_PropertyChanged; }

                _ProcessConfiguration = value;
                NotifyWorkflowStepConfigurationChanged();
            }
        }

        #region Event Handlers
        
        protected void OnPropertyChanged(string propertyName)
        { NotifyPropertyChanged(propertyName); }

        protected void OnWorkflowStepConfigurationChanged()
        { NotifyWorkflowStepConfigurationChanged(); }

        protected void OnWorkflowStepConfiguration_PropertyChanged(object sender, PropertyChangedEventArgs e)
        { OnWorkflowStepConfigurationChanged(); }

        protected void OnCheckpointConfigurationChanged()
        { NotifyCheckpointConfigurationChanged(); }

        protected void OnCheckpointConfiguration_PropertyChanged(object sender, PropertyChangedEventArgs e)
        { OnCheckpointConfigurationChanged(); }

        #endregion EventHandlers
    }   
}
