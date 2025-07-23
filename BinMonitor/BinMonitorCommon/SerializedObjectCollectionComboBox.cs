using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BinMonitor.Common
{
    public class SerializedObjectDictionaryComboBox<T> : UserControl where T : class
    {
        protected override Size DefaultSize
        {
            get
            {
                return new Size(100, 21);
            }
        }

        public event EventHandler SelectedKeyChanged;
        protected void NotifySelectedKeyChanged()
        {
            EventHandler handler = this.SelectedKeyChanged;
            if (handler != null)
            { handler(this, null); }
        }

        private SerializedObjectDictionary<T> _Source = null;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),Browsable(false)]
        public SerializedObjectDictionary<T> Source
        {
            get { return _Source; }
            set 
            { 
                if (_Source != null)
                { _Source.CollectionChanged -= OnSource_CollectionChanged; }
                
                _Source = value;
                
                if (Source != null)
                { _Source.CollectionChanged += OnSource_CollectionChanged; }
                
                OnSource_CollectionChanged();
            }
        }

        protected ComboBox cmbKeys;

        protected void InitializeComponent()
        {
            this.cmbKeys = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // cmbKeys
            // 
            this.cmbKeys.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbKeys.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbKeys.Location = new System.Drawing.Point(0, 0);
            this.cmbKeys.Name = "cmbKeys";
            this.cmbKeys.Size = new System.Drawing.Size(150, 21);
            this.cmbKeys.TabIndex = 0;
            // 
            // SerializedObjectDictionaryDropDown
            // 
            this.Controls.Add(this.cmbKeys);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "SerializedObjectDictionaryDropDown";
            this.Size = new System.Drawing.Size(150, 21);
            this.ResumeLayout(false);

        }

        public SerializedObjectDictionaryComboBox()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        { base.OnLoad(e); }

        protected virtual void OnSelectedKeyChanged()
        { NotifySelectedKeyChanged(); }

        public void ForceRefresh()
        { OnSource_CollectionChanged(); }

        protected virtual void OnSource_CollectionChanged()
        {
            if (DesignMode)
            { return; }

            cmbKeys.SelectedIndexChanged -= cmbKeys_SelectedIndexChanged;
            try
            {
                if (this.Source == null)
                {
                    cmbKeys.DataSource = null;
                    ClearSelection();
                    return;
                }

                string previousSelectedKey = cmbKeys.Text;
                cmbKeys.DataSource = Source.Keys.OrderBy(key => key).ToArray();
                
                if (string.IsNullOrWhiteSpace(previousSelectedKey) == true)
                { 
                    ClearSelection();
                    OnSelectedKeyChanged();
                }
                else if (Source.ContainsKey(previousSelectedKey) == false)
                { 
                    ClearSelection();
                    OnSelectedKeyChanged();
                }
                else
                { TrySelectKey(previousSelectedKey); }
            }
            finally
            { cmbKeys.SelectedIndexChanged += cmbKeys_SelectedIndexChanged; }

        }

        void cmbKeys_SelectedIndexChanged(object sender, EventArgs e)
        { OnSelectedKeyChanged(); }

        [Browsable(false)]
        public bool HasSelectedKey
        {
            get
            {
                if (Source == null)
                { return false; }
                if (cmbKeys.SelectedIndex == -1)
                { return false; }

                return true;
            }
        }

        [Browsable(false)]
        public string SelectedKey
        {
            get
            {
                if (HasSelectedKey == false)
                { return null; }
                if (Source == null)
                { return null; }
                return cmbKeys.Text;
            }
        }

        public string EnsureGetSelectedKey()
        {
            string selectedKey = this.SelectedKey;
            if (string.IsNullOrWhiteSpace(selectedKey))
            { throw new InvalidOperationException("The requested operatio requires a selected key"); }
            return selectedKey;
        }

        [Browsable(false)]
        public T SelectedValue
        {
            get
            {
                string selectedKey = SelectedKey;
                if (string.IsNullOrWhiteSpace(selectedKey))
                { return null; }

                T selectedValue;
                if (Source.TryGetValue(selectedKey, out selectedValue) == false)
                { return null; }

                return selectedValue;
            }
        }

        public T EnsureGetSelectedValue()
        {
            T selectedValue = this.SelectedValue;
            if (selectedValue == null)
            { throw new InvalidOperationException("The requested operation requires a selected key"); }
            
            return selectedValue;
        }

        public bool TrySelectKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            { 
                ClearSelection();
                return true;
            }
            
            cmbKeys.Text = key;
            return ((string)(cmbKeys.SelectedItem)).Equals(key);
        }

        public void EnsureSelectKey(string key)
        {
            if (TrySelectKey(key) == false)
            { throw new InvalidOperationException("Unable to select key " + key); }
        }

        public void ClearSelection()
        { 
            cmbKeys.SelectedIndex = -1;
            cmbKeys.Text = null;
        }

        protected void OnSource_CollectionChanged(object sender, EventArgs e)
        { OnSource_CollectionChanged(); }
    }
}
