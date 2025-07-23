using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Data.SqlClient;
 
using Newtonsoft.Json;


namespace Edocs.Dillion.VCC.Archiver
{
    public partial class EditAddPrices : Form
    {
        public string SqlConnection
        { get; set; }
        public string EdocsCustomerID
        { get; set; }
        private int NextID
        { get; set; }
        public EditAddPrices()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void EditAddPrices_Load(object sender, EventArgs e)
        {
            NextID = 1000;
            GetDocSizesPrices().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public async Task GetDocSizesPrices()
        {
            // IList<PriceDocSizeModel> priceSizeformaiton = new List<PriceDocSizeModel>();
            // PriceDocSizeModel priceDoc = new PriceDocSizeModel();
            // priceDoc.EdocsCustomerID = 100;
            // priceDoc.DocumentSize ="lkll";
            // priceDoc.PricePerDocument = (float)2.5;

            //string jsonString =   GetPices().ConfigureAwait(false).GetAwaiter().GetResult().ToString();
            // var jString = JsonConvert.DeserializeObject<PriceDocSizeModel[]>(jsonString);

            //   priceSizeformaiton = jString.ToList();
            //  IList<PriceDocSizeModel> dr = GetPices().ConfigureAwait(false).GetAwaiter().GetResult();
            GetPices().ConfigureAwait(false).GetAwaiter().GetResult();

            // dataGridDocPrices.Rows.Add(dr[0].ToString(),dr[2].ToString(),dr[1].ToString());



        }

        public async Task AddPricesDocSize()
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(SqlConnection))
                {
                    string sqlCmd = $"INSERT INTO [dbo].[EdocsDocumentPrices] ([EdocsCustomerID],[PricePerDocument],[DocumentSize]) VALUES ('{EdocsCustomerID}',{txtBoxDocPrice.Text},'{txxBoxDocSize.Text}')";
                    using (SqlCommand command = new SqlCommand(sqlCmd, connection))
                    {
                        command.CommandType = System.Data.CommandType.Text;
                        connection.Open();

                        //  StringBuilder jsonResult = new StringBuilder();
                        using (SqlDataReader reader = command.ExecuteReaderAsync().ConfigureAwait(false).GetAwaiter().GetResult())
                        {
                       
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

        public async Task GetPices()
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
                                while (reader.Read())
                                {
                                    dataGridDocPrices.Rows.Add(reader[0].ToString(), reader[2].ToString(),$"${reader[1].ToString()}", "");
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

        private void button2_Click(object sender, EventArgs e)
        {
            float result;
            if (string.IsNullOrWhiteSpace(txxBoxDocSize.Text))
            {
                MessageBox.Show("Need a document size");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtBoxDocPrice.Text))
            {
                MessageBox.Show("Need a document size");
                return;
            }
            bool success = float.TryParse(txtBoxDocPrice.Text, out result);
            if (!(success))
            {
                MessageBox.Show("Need a document price");
                return;
            }
            dataGridDocPrices.Rows.Add(NextID.ToString(), txxBoxDocSize.Text, $"${txtBoxDocPrice.Text}", " ");
            AddPricesDocSize().ConfigureAwait(false).GetAwaiter().GetResult();
            txxBoxDocSize.Text = string.Empty;
            txtBoxDocPrice.Text = string.Empty;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //foreach (DataGridViewRow dgvr in dataGridDocPrices.Rows)
            //{
            //    PriceDocSizeModel priceSizeformaiton = new PriceDocSizeModel();
            //    priceSizeformaiton.EdocsCustomerID = int.Parse(dgvr.Cells[0].Value.ToString());
            //    priceSizeformaiton.DocumentSize = dgvr.Cells[1].Value.ToString();
            //    priceSizeformaiton.PricePerDocument = float.Parse(dgvr.Cells[2].Value.ToString());
            //}
            this.Close();
        }
    }
}

