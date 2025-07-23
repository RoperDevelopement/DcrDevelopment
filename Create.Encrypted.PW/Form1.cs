using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using DEC = Edocs.Encrypt.Decrypt;
namespace Edocs.Create.Encrypted.PW
{
    public partial class MainForm : Form
    {
        bool Exported
        { get; set; }
        public MainForm()
        {
            Exported = true;
            InitializeComponent();

        }

        private void btnEnc_Click(object sender, EventArgs e)
        {
            try
            {
                //int mod4 = txtBoxKey.Text.Length % 4;

                // if (mod4 > 0)
                // {
                //     // txtBoxPassWord.Text += new string('=', 4 - mod4);
                //     MessageBox.Show($"{txtBoxKey.Text} Key has to be multiples of 4 AbcD 12345678 etc..");
                //     return;
                // }
                if (!(CheckEmptyString()))
                    return;
                
              

                if(chkBoxLocalMachine.Checked)
                {
                    DEC.Encrypt_Decrypt.EncryptDecryptKey = txtBoxKey.Text;
                    txtBoxPwEncptDecpt.Text = DEC.Encrypt_Decrypt.EncryptToString(txtBoxPassWord.Text, DataProtectionScope.LocalMachine);
                  
                }
                else
                {
                       txtBoxPwEncptDecpt.Text = DEC.AESSaltEncDec.EncryptString(txtBoxKey.Text,txtBoxPassWord.Text);
                  //  txtBoxPwEncptDecpt.Text = DEC.EncryptDecryptedCipher.EncryptCipherCBCMode(txtBoxPassWord.Text,txtBoxKey.Text );
                    


                }
                Decrypt(txtBoxPwEncptDecpt.Text);
                labPWEncDec.Visible = true;
                txtBoxPwEncptDecpt.Visible = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                if (chkBoxLocalMachine.Checked)
                    MessageBox.Show($"{txtBoxKey.Text} has to be a Base64 Char A-Z,a-z,0-9,+/");
                else
                    MessageBox.Show($"{txtBoxKey.Text} has to be a Base64 Char A-Z,a-z,0-9, 32 lenght");
                txtBoxKey.Focus();
            }
        }
        private string Decrypt(string decrp)
        {
           if (chkBoxLocalMachine.Checked)
            return DEC.Encrypt_Decrypt.DecryptToString(decrp, DataProtectionScope.LocalMachine);
            return DEC.AESSaltEncDec.DecryptString(txtBoxKey.Text,decrp);
        }
        private void btndecrypt_Click(object sender, EventArgs e)
        {
            try
            {
                //txtBoxPwEncptDecpt.Text = string.Empty;
                //int mod4 = txtBoxKey.Text.Length % 4;

                //if (mod4 > 0)
                //{
                //    // txtBoxPassWord.Text += new string('=', 4 - mod4);
                //    MessageBox.Show($"{txtBoxKey.Text} Key has to be multiples of 4 AbcD 12345678 etc..");
                //    return;
                //}
                if (!(CheckEmptyString()))
                    return;
                DEC.Encrypt_Decrypt.EncryptDecryptKey = txtBoxKey.Text;


                labPWEncDec.Text = "Password Decrypted:";
                txtBoxPwEncptDecpt.Text = Decrypt(txtBoxPassWord.Text);
                labPWEncDec.Visible = true;
                txtBoxPwEncptDecpt.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                txtBoxKey.Focus();
                //  MessageBox.Show($"{txtBoxPwEncptDecpt.Text} has to be a Base64 Char A-Z,a-z,0-9,+/=");
            }

        }
        bool CheckEmptyString()
        {
            labPWEncDec.Visible = false;
            txtBoxPwEncptDecpt.Visible = false;

            txtBoxPwEncptDecpt.Text = string.Empty;
            if (string.IsNullOrWhiteSpace(txtBoxKey.Text))
            {
                MessageBox.Show("Need a Key");
                txtBoxKey.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtBoxPassWord.Text))
            {
                MessageBox.Show("Need a Password");
                txtBoxPassWord.Focus();
                return false;
            }

            int mod4 = txtBoxKey.Text.Length % 4;

            if (mod4 > 0)
            {
                // txtBoxPassWord.Text += new string('=', 4 - mod4);
                MessageBox.Show($"{txtBoxKey.Text} Key has to be multiples of 4 AbcD 12345678 etc.. and has to be a Base64 Char A-Z,a-z,0-9,+/");
                txtBoxKey.Focus();
                return false;
            }
            return true;
        }
        void ExportData()
        {
            if ((string.IsNullOrWhiteSpace(txtBoxKey.Text)) || (string.IsNullOrWhiteSpace(txtBoxPwEncptDecpt.Text)))
            {
                MessageBox.Show("Nothing to Export");
                return;
            }
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                SaveData(saveFileDialog1.FileName);
                Exported = true;
            }
        }
        void SaveData(string fName)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Key,{txtBoxKey.Text}");
            sb.AppendLine($"Password,{txtBoxPassWord.Text}");
            sb.AppendLine($"PWEncrypted,{txtBoxPwEncptDecpt.Text}");
            System.IO.File.WriteAllText(fName, sb.ToString());
        }
        private void btnExport_Click(object sender, EventArgs e)
        {
            ExportData();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (!(Exported))
            {
                if (MessageBox.Show("Save Encrypted Data", "Not Saved", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    ExportData();
            }
            this.Close();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void txtBoxKey_TextChanged(object sender, EventArgs e)
        {
            Exported = false;
            labKeyLength.Text = $"Key Length: {txtBoxKey.Text.Length}";
        }

        private void txtBoxPassWord_TextChanged(object sender, EventArgs e)
        {
            Exported = false;
        }
    }
}
