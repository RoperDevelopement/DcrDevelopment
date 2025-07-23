/*
 * User: Sam Brinly
 * Date: 10/15/2013
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using EdocsUSA.Controls;
using EdocsUSA.Utilities;
using EdocsUSA.Utilities.Extensions;
using System.Diagnostics;
using ETL = EdocsUSA.Utilities.Logging.TraceLogger;
namespace Scanquire.Public
{
    /// <summary>Dialog window to allow users to select and configure a twain device.</summary>
	public class SQTwainSettingsDialog : TableLayoutPanelInputDialog
	{
		Properties.SQTwain DefaultSettings = Properties.SQTwain.Default;
	
		private Twain _Twain;
		public Twain Twain
		{
			get { return _Twain; }
			set { _Twain = value; }
		}
		
        /// <summary>Name of the last selected device name.</summary>
		protected string PreviousDataSourceName
		{
			get { return DefaultSettings.PreviousDataSourceName; }
			set 
			{
				DefaultSettings.PreviousDataSourceName = value;
				DefaultSettings.Save();
			}
		}
		
		private string SettingsRootPath = Path.Combine(SettingsManager.AllUsersSettingsDirectoryPath, "SQTwain");		
		
        /// <summary>Listing of all available devices.</summary>
		protected ComboBox ScannersComboBox;
        /// <summary>Listing of all available settings for the device selected in ScannersComboBox.</summary>
		protected ListBox SettingsListBox;
        /// <summary>Adds a new setting to the device selected in ScannersComboBox.</summary>
		protected Button AddSettingsButton;
        /// <summary>Removes the setting selected in SettingsListBox from the device selected in ScannersComboBox.</summary>
		protected Button RemoveSettingsButton;
        /// <summary>List of available image processors.</summary>
		protected ListBox FiltersListBox;
        /// <summary>
        /// If checked, device's native UI should be displayed on acquire.
        /// If not checked, device's native UI should not be displayed on acquire.
        /// </summary>
		protected CheckBox ShowUICheckBox;
        /// <summary>
        /// If checked, should apply the last settings used.
        /// </summary>
		protected CheckBox UsePreviousSettingsCheckBox;
				
        /// <summary>Name of the device selected.</summary>
		public string SelectedScanner
		{
			get { return (string)ScannersComboBox.SelectedItem; }
		}
		
        /// <summary>Name of the setting configuration selected for the specified device.</summary>
		public string SelectedSetting
		{ get { return (string)SettingsListBox.SelectedItem; } }
		
        /// <summary>
        /// If true, device's native UI should be displayed on acquire.
        /// If false, device's native UI should not be displayed on acquire.
        /// </summary>
		public bool ShowUI 
		{
			get { return ShowUICheckBox.Checked; } 
			set { ShowUICheckBox.Checked = false; }
		}
		
        /// <summary>If true, use the previous settings.</summary>
		public bool UsePreviousSettings
		{ 
			get { return UsePreviousSettingsCheckBox.Checked; }
			set { UsePreviousSettingsCheckBox.Checked = value; }
		}
		
		private void InitializeComponent()
		{
            ETL.TraceLoggerInstance.TraceInformation("Opening Scanner Form");
			ScannersComboBox = new ComboBox()
			{
				Width = 250,
				DropDownStyle = ComboBoxStyle.DropDownList,
				Sorted = true
			};
			ScannersComboBox.SelectedIndexChanged += delegate(object sender, EventArgs e) 
			{ 
				if (SelectedScanner.IsNotEmpty())
				{ Twain.SetActiveDataSource(SelectedScanner); }
				LoadSettings();
			};
			TableLayoutPanel.Controls.Add(ScannersComboBox, 0, 0);
			
			Label ScannerSettingsCaption = new Label()
			{
				Text = "Settings",
				TextAlign = ContentAlignment.BottomLeft
			};
			AddControl(ScannerSettingsCaption, 0, 1);
			SettingsListBox = new ListBox()
			{
				Width = 250,
				Height = 200,
				SelectionMode = SelectionMode.One,
				Sorted = true
			};
			SettingsListBox.DoubleClick += delegate(object sender, EventArgs e) 
			{ this.DialogResult = DialogResult.OK; };
			SettingsListBox.SelectedIndexChanged += new EventHandler(SettingsListBox_SelectedIndexChanged);
			TableLayoutPanel.Controls.Add(SettingsListBox, 0, 2);
			
			Label ImageFiltersCaption = new Label()
			{
				Text = "Image Filters",
				TextAlign = ContentAlignment.BottomLeft
			};
			AddControl(ImageFiltersCaption, 1, 1);
			FiltersListBox = new ListBox()
			{
				Enabled = false,
				Width = 250,
				Height = 200,
				SelectionMode = SelectionMode.MultiSimple
			};
			AddControl(FiltersListBox, 1, 2);
			
			AddSettingsButton = new Button()
			{
				Text = "Add Setting",
				Width = 250
			};
			AddSettingsButton.Click += delegate(object sender, EventArgs e) { AddNewSetting(); };
			AddControl(AddSettingsButton, 0, 3);
			
			RemoveSettingsButton = new Button()
			{
				Text = "Remove Selected Setting",
				Width = 250
			};			
			
			RemoveSettingsButton.Click += delegate(object sender, EventArgs e) { RemoveSelectedSetting(); };
			AddControl(RemoveSettingsButton, 0, 4);
			
			ShowUICheckBox = new CheckBox()
			{ 
				Checked = true,
				Text = "Show UI"
			};
			TableLayoutPanel.Controls.Add(ShowUICheckBox, 0, 5);
			
			UsePreviousSettingsCheckBox = new CheckBox()
			{
				Checked = false,
				Text = "Use Previous Settings",
				AutoSize = true
			};
			TableLayoutPanel.Controls.Add(UsePreviousSettingsCheckBox, 0, 6);
			
			this.MinimumSize = new Size(525, 525);
			
			ScannersComboBox.DataSource = Twain.DataSourcesNames;
			FiltersListBox.Items.Add(string.Empty);
			foreach (string key in SQImageProcessors.Instance.Keys)
			{ FiltersListBox.Items.Add(key); }

            

            Twain.ActiveDataSourceChanged += delegate(object sender, EventArgs e)
			{ ScannersComboBox.SelectedText = Twain.ActiveDataSourceName; };
			
			Twain.DataSourcesChanged += delegate(object sender, EventArgs e)
			{ ScannersComboBox.DataSource = Twain.DataSourcesNames; };

			if ((PreviousDataSourceName.IsNotEmpty()) && (Twain.DataSourcesNames.Contains(PreviousDataSourceName)))
			{ 
				ScannersComboBox.SelectedItem = PreviousDataSourceName;
			}
			else ScannersComboBox.SelectedText = Twain.ActiveDataSourceName;

            
            
            CreateDataSourcesFolders();

        }

        
        protected void CreateDataSourcesFolders()
        {
            string tsFolder = Path.Combine(SettingsManager.ApplicationSettingsDirectoryPath, AutoQaCheckBlankImages.AutoQaCheckBlankImagesInstance.GetAppConfigSqlSetting("TwainSettingsFolder", "Scanquire.Public.dll.config", @"Config\Twain Settings"));
            ETL.TraceLoggerInstance.TraceInformation($"Creating datasources folder:{tsFolder}");
            foreach (string ds in Twain.DataSourcesNames)
            {
                if (!(Directory.Exists(string.Format(@"{0}\{1}", SQTwainSettings.Instance.DirectoryPath, ds))))
                {
                    Directory.CreateDirectory(string.Format(@"{0}\{1}", SQTwainSettings.Instance.DirectoryPath, ds));
                    ETL.TraceLoggerInstance.TraceInformation(string.Format(@"Creating directory {0}\{1}", SQTwainSettings.Instance.DirectoryPath, ds));
                    var dir = new DirectoryInfo(tsFolder);
                    foreach (var file in dir.EnumerateFiles("*.*"))
                    {
                        string destFName = string.Format(@"{0}\{1}\{2}", SQTwainSettings.Instance.DirectoryPath,ds,file.Name);
                        ETL.TraceLoggerInstance.TraceInformation(string.Format(@"Copying file source file {0} to dest file {1}",file.FullName,destFName));
                        File.Copy(file.FullName, destFName); 
                    }
                    ScannersComboBox.SelectedText = ds;
                    SQTwainSettings.Instance.Refresh();
                    LoadSettings();
                }
                else
                    ETL.TraceLoggerInstance.TraceInformation(string.Format(@"Directory {0}\{1} all ready exists", SQTwainSettings.Instance.DirectoryPath, ds));

            }
        }
		protected void SettingsListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			FiltersListBox.ClearSelected();
			
			SQTwainSetting setting = SQTwainSettings.Instance[SelectedScanner][SelectedSetting];
			if (setting == null) return;
            ETL.TraceLoggerInstance.TraceInformation($"Scan operator selected scanner {SelectedScanner} using setting:{SelectedSetting}");
			foreach (string filter in setting.ImageProcessorNames)
			{ FiltersListBox.SelectedIndices.Add(FiltersListBox.FindString(filter)); }
		}
		
		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
            ETL.TraceLoggerInstance.TraceInformation($"Showing form:{this.Name}");
			UsePreviousSettings = false;
			ShowUI = false;
			SettingsListBox.Focus();
		}
		
		public SQTwainSettingsDialog(Twain twain) : base(2, 7)
		{ 
			Twain = twain;
			InitializeComponent();
		}
		
		
		protected override void OnClosed(EventArgs e)
		{
            ETL.TraceLoggerInstance.TraceInformation($"Closing form:{this.Name}");
            PreviousDataSourceName = SelectedScanner;
			base.OnClosed(e);
		}
		
        /// <summary>Loads all settings from SQTwainSettings into SettingsListBox.</summary>
		protected void LoadSettings()
		{
            ETL.TraceLoggerInstance.TraceInformation("Loading scanner settings");
            SettingsListBox.Items.Clear();

            if (SQTwainSettings.Instance[SelectedScanner] == null)
            {
                ETL.TraceLoggerInstance.TraceInformation("No settings"); ;
            }
            else
            {
                foreach (string key in SQTwainSettings.Instance[SelectedScanner].Keys)
                {
                    ETL.TraceLoggerInstance.TraceInformation($"Adding scanner setting:{key}"); 
                    SettingsListBox.Items.Add(key);
                }
                SettingsListBox.Enabled = SettingsListBox.Items.Count > 0;
                ShowUICheckBox.Checked = SettingsListBox.Items.Count == 0;
                
            }
		}
		
        /// <summary>Dialog to create a new setting configuration for a device.</summary>
		protected class NewSettingDialog : TableLayoutPanelInputDialog
		{
			/// <summary>Contains the name of the new setting.</summary>
            public ValidatingTextBox<string> SettingName;
	        /// <summary>Listing of all available image processors to add to the setting.</summary>
			public ListBox ImageProcessorsListBox;
			
            /// <summary>The ImageProcessors the user has selected to apply to the setting.</summary>
			public IEnumerable<string> SelectedImageProcessors
			{
				get
				{
					foreach (object index in ImageProcessorsListBox.SelectedItems)
					{ yield return (string)index; }
				}
				set
				{
					ImageProcessorsListBox.ClearSelected();
					foreach (string key in value)
					{
						ImageProcessorsListBox.SelectedIndices.Add(ImageProcessorsListBox.FindString(key));
					}
				}
			}
			
			private void InitializeComponent()
			{
				AddControl(new CaptionLabel("Name"), 0, 0);
				SettingName = new ValidatingTextBox<string>()
				{
					Width = 250,
					RequiresValue = true
				};
				AddControl(SettingName, 1, 0);
				
				AddControl(new CaptionLabel("Filter"), 0, 1);
				ImageProcessorsListBox = new ListBox()
				{
					Width = 250,
					Height = 200,
					SelectionMode = SelectionMode.MultiSimple
				};
                
                //Load the listing of available imageProcessors.
				foreach (string key in SQImageProcessors.Instance.Keys)
				{ ImageProcessorsListBox.Items.Add(key); }
				ImageProcessorsListBox.SelectedItem = string.Empty;
				AddControl(ImageProcessorsListBox, 1, 1);
			}
			
			public NewSettingDialog() : base(2, 2) 
			{ InitializeComponent(); }
		}
		
		protected void AddNewSetting()
		{
            ETL.TraceLoggerInstance.TraceInformation($"Setting active device to scanner:{SelectedScanner}");
            //Set the active device to the selected scanner.
            Twain.SetActiveDataSource(SelectedScanner);
			
            //Request the current configuration value from the device.
			byte[] customDSData = null;
			if (SelectedSetting.IsNotEmpty()) customDSData = SQTwainSettings.Instance[SelectedScanner][SelectedSetting].CustomDSData;
			customDSData = Twain.GetCustomDSData(customDSData);
			
            //Initialize a new settings dialog.
			NewSettingDialog dlg = new NewSettingDialog();
			if (SelectedSetting.IsNotEmpty()) dlg.SettingName.Value = SelectedSetting;

            //If any filters were selected, add them to the setting list.
			if (FiltersListBox.SelectedIndices.Count > 0)
			{
				List<string> selectedFilters = new List<string>();
				foreach (int index in FiltersListBox.SelectedIndices)
				{
					selectedFilters.Add(FiltersListBox.Items[index].ToString());
				}
				dlg.SelectedImageProcessors = selectedFilters;
			}
			//Show the dialog.
			if (dlg.ShowDialog() != DialogResult.OK)
            {
                ETL.TraceLoggerInstance.TraceError($"Cancled adding new scanner setting dialog:{dlg.Name}");
                throw new OperationCanceledException();
            }
                
			
            //Retrieve the settings.
			string settingName = dlg.SettingName.Value;
			IEnumerable<string> imageProcessorNames = dlg.SelectedImageProcessors;
			
            //If the setting name provided by the user exists, prompt to overwrite or cancel.
			if (SQTwainSettings.Instance[SelectedScanner].ContainsKey(settingName))
			{
				if (MessageBox.Show(this, "Overwrite existing settings", "Overwrite", MessageBoxButtons.OKCancel) != DialogResult.OK)
				{
                    ETL.TraceLoggerInstance.TraceWarning($"Cancled updating existing scanner setting:{settingName}");
                    throw new OperationCanceledException();
                }
                else
                {
                    ETL.TraceLoggerInstance.TraceWarning($"UpDate existing scanner setting:{settingName}");
                }
			}
			
            //Create the new setting
			SQTwainSetting setting = new SQTwainSetting(customDSData);
			
			foreach(string filterName in dlg.SelectedImageProcessors)
			{ setting.ImageProcessorNames.Add(filterName); }

            //Add the setting to the repository.
			SQTwainSettings.Instance[SelectedScanner][settingName] = setting;

            //Reload the avialbe setting list and select the new one.
			LoadSettings();
			SettingsListBox.SelectedItem = settingName;
		}
		
        /// <summary>Delete the currently selected setting.</summary>
		protected void RemoveSelectedSetting()
		{
			if (MessageBox.Show(this, "Are you sure you want to delete the selected setting?\nThis action cannot be undone.", "Confirm overwrite", MessageBoxButtons.YesNo) != DialogResult.Yes)
			{
                ETL.TraceLoggerInstance.TraceWarning($"Cancel deleting scanner setting:{SelectedSetting}");
                throw new OperationCanceledException();
            }
            ETL.TraceLoggerInstance.TraceWarning($"Deleted scanner setting:{SelectedSetting}");
            SQTwainSettings.Instance[SelectedScanner].Remove(SelectedSetting);
			LoadSettings();
		}
	}
}
