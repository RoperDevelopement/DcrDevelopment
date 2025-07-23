using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using EdocsUSA.Utilities.Extensions;
using System.Threading.Tasks;
using System.Diagnostics;
using EdocsUSA.Utilities;
using ETL = EdocsUSA.Utilities.Logging.TraceLogger;
using System.Reflection;
using System.Runtime.InteropServices;
using Newtonsoft.Json.Linq;

namespace Scanquire.Public.UserControls
{
    /// <summary>User dropdown control to select a single SQArchiver from all configured SQArchivers.</summary>
    public partial class SQArchiverSelector : UserControl
    {
        private readonly SynchronizedBindingList<KeyValuePair<string, ISQArchiver>> _Archivers = new SynchronizedBindingList<KeyValuePair<string, ISQArchiver>>();
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SynchronizedBindingList<KeyValuePair<string, ISQArchiver>> Archivers
        { get { return _Archivers; } }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ISQArchiver SelectedArchiver
        {
            get
            {
                ISQArchiver archiver;
                TryGetSelectedArchiver(out archiver);
                return archiver;
            }
            set
            {
                KeyValuePair<string, ISQArchiver> item = Archivers.FirstOrDefault(itm => (itm.Value == value));
                if (item.Equals(default(KeyValuePair<string, ISQArchiver>)))
                { ETL.TraceLoggerInstance.TraceWarning("Specified archiver is not present in the list"); }
                else
                { CurrentArchiverComboBox.SelectedItem = item; }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SelectedArchiverName
        {
            get
            {
                string name;
                TryGetSelectedArchiverName(out name);
                return name;
            }
            set
            {
                KeyValuePair<string, ISQArchiver> item = Archivers.FirstOrDefault(itm => (itm.Key == value));
                if (item.Equals(default(KeyValuePair<string, ISQArchiver>)))
                { ETL.TraceLoggerInstance.TraceWarning("Sepecified archiver is not present in the list"); }
                else
                { CurrentArchiverComboBox.SelectedItem = item; }
            }
        }

        public SQArchiverSelector()
        {
            InitializeComponent();
            ETL.TraceLoggerInstance.TraceInformation($"Opening dialog{this.Name}");
            CurrentArchiverComboBox.DisplayMember = "Key";
            CurrentArchiverComboBox.ValueMember = "Value";
            CurrentArchiverComboBox.DataSource = _Archivers;
        }

        public bool TryGetSelectedArchiver(out ISQArchiver archiver)
        {
            string name;
            return TryGetSelectedArchiverInfo(out name, out archiver);
        }

        public bool TryGetSelectedArchiverName(out string name)
        {
            ISQArchiver archiver;
            return TryGetSelectedArchiverInfo(out name, out archiver);
        }

        public bool TryGetSelectedArchiverInfo(out string name, out ISQArchiver archiver)
        {
            object selectedItem = CurrentArchiverComboBox.SelectedItem;
            if (selectedItem == null)
            {
                ETL.TraceLoggerInstance.TraceError("No Archive selected");
                name = null;
                archiver = null;
                return false;
            }
            else
            {
                KeyValuePair<string, ISQArchiver> info = (KeyValuePair<string, ISQArchiver>)(selectedItem);
                name = info.Key;
                archiver = info.Value;
                ETL.TraceLoggerInstance.TraceInformation($"Scan operator selected archiver:{name}  value:{archiver}");
                return true;
            }
        }

        public async Task<object> GetArchiverProperty(string archiverKey, string propertyValue)
        {
            ISQArchiver value = null;
            SQArchivers.Instance.TryGetValue(archiverKey, out value);

            object objectPropValue = value.GetType().GetProperty(propertyValue).GetValue(value, null);
            return objectPropValue;
        }

        public async Task<Dictionary<string, string>> GetDropDownPost()
        {
            string configFile = System.IO.Path.Combine($"{SQArchivers.Instance.DirectoryPath}", "DropListOrder.txt");
            ETL.TraceLoggerInstance.TraceInformation($"GetDropDownPost for drop down list file {configFile}");
            if (!(System.IO.File.Exists(configFile)))
            {
                ETL.TraceLoggerInstance.TraceWarning($"Drop down list file {configFile} not found");
                return null;
            }

            Dictionary<string, string> retDic = new Dictionary<string, string>();

            using (System.IO.StreamReader sr = new System.IO.StreamReader(configFile))
            {
                string instr = string.Empty;
                while ((instr = sr.ReadLine()) != null)
                {
                    if (!(string.IsNullOrWhiteSpace(instr)))
                    {
                        string[] spStr = instr.Split(',');
                        if (!(retDic.ContainsKey(instr.ToLower())))
                            retDic.Add(spStr[0].ToLower(), spStr[1]);
                    }
                }
            }


            return retDic;
        }
        /// <summary>
        /// Refresh the listing of available Archivers from SQArchivers.Instance
        /// </summary>
        public async Task LoadAllArchivers()
        {

            Dictionary<string, string> posDropDown = GetDropDownPost().ConfigureAwait(false).GetAwaiter().GetResult();
            ETL.TraceLoggerInstance.TraceInformation("Refreshing the listing of available Archivers from SQArchivers.Instance");
            _Archivers.Clear();
            Task loadTask = new Task(() =>
             {
                 foreach (KeyValuePair<string, ISQArchiver> archiver in SQArchivers.Instance)
                 {
                     if (posDropDown != null)
                     {
                         if (posDropDown.ContainsKey(archiver.Key.ToLower()))
                         {

                             if (int.TryParse(posDropDown[archiver.Key.ToLower()].ToString(), out int post))
                             {
                                 int archCount = _Archivers.Count();
                                 if ((archCount == 0) || (post > archCount))
                                     _Archivers.Add(archiver);

                                 else
                                     _Archivers.Insert(post, archiver);
                                 continue;
                             }
                         }

                         _Archivers.Add(archiver);

                     }
                     else
                         _Archivers.Add(archiver);

                 }

             });
            loadTask.Start(TaskScheduler.FromCurrentSynchronizationContext());
            await loadTask;
        }


    }
}
