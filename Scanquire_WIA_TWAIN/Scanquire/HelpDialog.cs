using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EDL = EdocsUSA.Utilities.Logging;
namespace Scanquire
{
    public partial class HelpDialog : Form
    {
        public HelpDialog()
        {
            InitializeComponent();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            //If not already loaded, load the help file.
            if (WebBrowser.Url == null)
            { LoadHelpFile(); }
        }

        protected void LoadHelpFile()
        {
            //TODO: Move file path to settings?
            string helpFilePath = new FileInfo(@".\help.html").FullName;
            Uri helpFileUri = new Uri(helpFilePath, UriKind.Absolute);
            EDL.TraceLogger.TraceLoggerInstance.TraceInformation($"OPening help file:{helpFileUri.AbsolutePath}");
            Debug.WriteLine(helpFileUri.AbsolutePath);
            WebBrowser.Navigate(helpFileUri);
        }

        private void NavigateHomeButton_Click(object sender, EventArgs e)
        {
            LoadHelpFile();
        }
    }
}
