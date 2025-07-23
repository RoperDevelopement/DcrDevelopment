using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EdocsUSA.Utilities;
using System.Threading.Tasks;
using System.Data.SqlClient;
using EdocsUSA.Utilities.Logging;
using Scanquire.Public.UserControls;
namespace Edocs.Dillion.VCC.Archiver
{
    public partial class VCCArchiverrDialog : Form
    {
        public VCCArchiverrDialog()
        {
            InitializeComponent();
            VCCImageViewer.SplitContainer.SplitterDistance = 90;
            VCCImageViewer.ThumbnailToolStrip.Visible = false;

        }
        public string GetProgramDataFolder
        {
            get
            {
                string pdFolder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "DOHArchiverSettings");
                return (pdFolder);
            }
        }
        public SQImageListViewer VCCWOImage
        {
            get { return VCCImageViewer; }
            set { VCCImageViewer = value; }
        }
        public string SqlConnection
        { get; set; }
        public bool ShowTotalDocsScanned
        { get; set; }
        public bool IncludeBlankDocs
        { get; set; }
        public string ImageFolder
        { get; set; }

        public string ChurchCity
        { get; set; }
        public string ChurchName
        { get; set; }
        public string ChurchBookType
        { get; set; }
        
      
        public int LargeDoc
        {
            get; set;
        }
       

      
        public string EdocsCustomerID
        { get; set; }
       

      

         
        
        
        

       
         
      
        private bool LookForString(string fileName,string searchString)
        {
            string strFile = System.IO.File.ReadAllText(System.IO.Path.Combine(GetProgramDataFolder, fileName));
            bool contains = strFile.Contains(searchString);
            if(!(contains))
            {
                strFile = $"{strFile},{searchString}";
                System.IO.File.WriteAllText(System.IO.Path.Combine(GetProgramDataFolder, fileName), strFile);
                return true;

            }
            return false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void txtBoxInvoiceNumber_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
