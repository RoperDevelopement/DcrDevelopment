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
namespace Edocs.Diocese.Of.Helena.Archiver
{
    public partial class DioceseofHelenaArchiverDialog : Form
    {
        public DioceseofHelenaArchiverDialog()
        {
            InitializeComponent();
        }
        public string GetProgramDataFolder
        {
            get
            {
                string pdFolder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "DOHArchiverSettings");
                return (pdFolder);
            }
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
        public string City
        {
            get { return cBoxCity.Text; }
            set { cBoxCity.Text = value; }
        }
        public string Church
        {
            get { return cBoxChurch.Text.Replace(".", " "); }
            set { cBoxChurch.Text = value; }
        }
        public string BookType
        {
            get { return cBoxBookType.Text; }
            set { cBoxBookType.Text = value; }
        }
        public string DateRangeSDate
        {
            get { return dtDateRangeStart.Text.Replace("/", "-"); }
            set { value = dtDateRangeStart.Text; }
        }
        public int LargeDoc
        {
            get; set;
        }
        public string DateRangeEDate
        {
            get { return dateRangeEnd.Text.Replace("/", "-"); }
            set { value = dateRangeEnd.Text; }
        }

        public string TotalScanned
        {
            get { return txtBoxTotalScanned.Text; }
            set { value = txtBoxTotalScanned.Text; }
        }
        public string EdocsCustomerID
        { get; set; }
        private bool CheckInput()
        {
            if (string.IsNullOrWhiteSpace(cBoxBookType.Text))
            {
                MessageBox.Show("Need Book Type", "Missing Book Type", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (string.IsNullOrWhiteSpace(cBoxChurch.Text))
            {
                MessageBox.Show("Need Church", "Missing Church", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (string.IsNullOrWhiteSpace(cBoxCity.Text))
            {
                MessageBox.Show("Need City", "Missing City", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }


        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!(CheckInput()))
                return;
            if (!(GetDocPrice().ConfigureAwait(false).GetAwaiter().GetResult()))
                return;
            AddNewStr();
            this.DialogResult = DialogResult.OK;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private async Task<bool> GetDocPrice()
        {
            int indexBlank = cmbBoxPrices.Text.IndexOf(" ");
            string docPrice = cmbBoxPrices.Text.Substring(0, indexBlank).Trim();
            int results = 0;
            if (int.TryParse(docPrice, out results))
            {
                LargeDoc = results;
                return true;
            }
            else
                MessageBox.Show($"Invalid Document Price {cmbBoxPrices.Text}");
            return false;
        }
        private void DioceseofHelenaArchiverDialog_Shown(object sender, EventArgs e)
        {
            GetBookTypes();
            GetChurches();
            GetCities();
            
            txtBoxTotalScanned.Text = TraceLogger.TraceLoggerInstance.GetTotalImagesScannedString(IncludeBlankDocs);
        }
        private void GetChurches()
        {

            if (cBoxChurch.Items.Count == 0)
            {
                string ch = System.IO.File.ReadAllText(System.IO.Path.Combine(GetProgramDataFolder, ChurchName));
                cBoxChurch.BeginUpdate();
                foreach (string c in ch.Split(','))
                {
                    cBoxChurch.Items.Add(c.Trim());
                }
                cBoxChurch.EndUpdate();
            }
        }

        public string ArchiverSettings
        {
            get;set;
        }
        private void GetCities()
        {
            
            if (cBoxCity.Items.Count == 0)
            {
                string citys = System.IO.File.ReadAllText(System.IO.Path.Combine(GetProgramDataFolder, ChurchCity));
                cBoxCity.BeginUpdate();
                foreach (string c in citys.Split(','))
                {
                    cBoxCity.Items.Add(c.Trim());
                }
                cBoxCity.EndUpdate();
            }
        }

        private void GetBookTypes()
        {
           
            if (cBoxBookType.Items.Count == 0)
            {
                string bt = System.IO.File.ReadAllText(System.IO.Path.Combine(GetProgramDataFolder, ChurchBookType));
                cBoxBookType.BeginUpdate();
                foreach (string b in bt.Split(','))
                {
                    cBoxBookType.Items.Add(b.Trim());
                }
                cBoxBookType.EndUpdate();
            }
        }
        private void AddNewStr()
        {

            if (LookForString(ChurchName, cBoxChurch.Text))
                cBoxChurch.Items.Add(cBoxChurch.Text);
            if (LookForString(ChurchCity, cBoxCity.Text))
                cBoxCity.Items.Add(cBoxCity.Text);
            if (LookForString(ChurchBookType, cBoxBookType.Text))
                cBoxBookType.Items.Add(cBoxBookType.Text);

        }
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

        private void addChurch_Click(object sender, EventArgs e)
        {
            AddArchiverInfo cityChuckBt = new AddArchiverInfo();
            cityChuckBt.LabelText = "Add or Delete New Churches:";
            cityChuckBt.CityChurchBookType = System.IO.File.ReadAllText(System.IO.Path.Combine(GetProgramDataFolder, ChurchName));
            if (cityChuckBt.ShowDialog() == DialogResult.OK)
            {
                System.IO.File.WriteAllText(System.IO.Path.Combine(GetProgramDataFolder, ChurchName), cityChuckBt.CityChurchBookType);
                cBoxChurch.BeginUpdate();
                cBoxChurch.Items.Clear();
                cBoxChurch.EndUpdate();
                GetChurches();

            }

        }

        private void addCIty_Click(object sender, EventArgs e)
        {
            AddArchiverInfo cityChuckBt = new AddArchiverInfo();
            cityChuckBt.LabelText = "Add or Delete new Cities:";
            cityChuckBt.CityChurchBookType = System.IO.File.ReadAllText(System.IO.Path.Combine(GetProgramDataFolder, ChurchCity));
           if(cityChuckBt.ShowDialog() == DialogResult.OK)
            {
                System.IO.File.WriteAllText(System.IO.Path.Combine(GetProgramDataFolder, ChurchCity), cityChuckBt.CityChurchBookType);
                cBoxCity.BeginUpdate();
                cBoxCity.Items.Clear();
                cBoxCity.EndUpdate();
                GetCities();

            }
           
        }

        private void addBT_Click(object sender, EventArgs e)
        {
            AddArchiverInfo cityChuckBt = new AddArchiverInfo();
            cityChuckBt.LabelText = "Add or Delete new Book Types:";
            cityChuckBt.CityChurchBookType = System.IO.File.ReadAllText(System.IO.Path.Combine(GetProgramDataFolder, ChurchBookType));
            if (cityChuckBt.ShowDialog() == DialogResult.OK)
            {
                System.IO.File.WriteAllText(System.IO.Path.Combine(GetProgramDataFolder, ChurchBookType), cityChuckBt.CityChurchBookType) ;
                cBoxBookType.BeginUpdate();
                cBoxBookType.Items.Clear();
                cBoxBookType.EndUpdate();
                GetBookTypes();

            }
        }

        private void editAddDocPricesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditAddPrices editAddPrices = new EditAddPrices();
            editAddPrices.SqlConnection = SqlConnection;
            editAddPrices.EdocsCustomerID = EdocsCustomerID;
            editAddPrices.ShowDialog();
            cmbBoxPrices.BeginUpdate();
            cmbBoxPrices.Items.Clear();
            GetPrices().ConfigureAwait(false).GetAwaiter().GetResult();
            cmbBoxPrices.EndUpdate();
        }

        private void DioceseofHelenaArchiverDialog_Load(object sender, EventArgs e)
        {
            AddCmbBoxPrices().ConfigureAwait(false).GetAwaiter().GetResult();
        }
        private async Task AddCmbBoxPrices()
        {
            if(cmbBoxPrices.Items.Count == 0)
            {
                cmbBoxPrices.BeginUpdate();
                GetPrices().ConfigureAwait(false).GetAwaiter().GetResult();
                cmbBoxPrices.EndUpdate();
                cmbBoxPrices.SelectedIndex = 0;

            }
        }


        public async Task GetPrices()
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(SqlConnection))
                {
                    string sqlCmd = $"SELECT[ID] as EdocsCustomerID ,[PricePerDocument] as PricePerDocument,[DocumentSize] as DocumentSize  FROM[EdocsITS].[dbo].[EdocsDocumentPrices] where [EdocsCustomerID] = {EdocsCustomerID}";
                    using (SqlCommand command = new SqlCommand(sqlCmd, connection))
                    {
                        command.CommandType = System.Data.CommandType.Text;
                        connection.Open();

                        //  StringBuilder jsonResult = new StringBuilder();
                        using (SqlDataReader reader = command.ExecuteReaderAsync().ConfigureAwait(false).GetAwaiter().GetResult())
                        {
                            if (reader.HasRows)

                            {
                                //    dataGridDocPrices.Rows[0].Cells[0].Selected = true;
                                //   dataGridDocPrices.BeginEdit(true);
                                cmbBoxPrices.Items.Add("Select Document Price");
                                while (reader.Read())
                                {
                                   cmbBoxPrices.Items.Add($"{reader[0].ToString()} {reader[2].ToString()} ${reader[1].ToString()}");
                                }
                                // dataGridDocPrices.EndEdit();
                            }
                        }

                        //  Console.WriteLine(jsonResult.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception($"Error {ex.Message}");
            }


        }


    }
}
