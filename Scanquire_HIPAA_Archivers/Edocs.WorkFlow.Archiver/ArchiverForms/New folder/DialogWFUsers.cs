using Edocs.WorkFlow.Archiver.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Edocs.WorkFlow.Archiver.ArchiverForms
{
    public partial class DialogWFUsers : Form
    {
        public string SelectUsersId
        { get; set; }
        public IDictionary<int, WFUsersModel> UsersWF
        { get; set; }
        public DialogWFUsers()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (lBoxUsers.SelectedItems.Count > 0)
            {
                lBoxSelectedUsers.BeginUpdate();
                foreach (var sle in lBoxUsers.SelectedItems)
                {
                    if (!(lBoxSelectedUsers.Items.Contains(sle)))
                    lBoxSelectedUsers.Items.Add(sle);
                    Console.WriteLine();
                }
                lBoxSelectedUsers.EndUpdate();
            }
        }

        private void Clear()
        {
            SelectUsersId = string.Empty;
        }
        private void DialogWFUsers_Load(object sender, EventArgs e)
        {
            
            //listBoxStudentModules.Items.Add(listBoxAllModules.SelectedItem); 
            GetWFUsers();
            Clear();



        }
        private async void GetWFUsers()
        {
            if (lBoxUsers.Items.Count == 0)
            {
                lBoxUsers.BeginUpdate();
                lBoxUsers.Items.Clear();

                //   dialogWFUsers.UserLBox.DataSource = UsersWF;
                foreach (KeyValuePair<int, WFUsersModel> pair in UsersWF)
                {
                    lBoxUsers.Items.Add($"{pair.Value.FName} {pair.Value.LName}");
                }



                lBoxUsers.EndUpdate();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            foreach(var userId in lBoxSelectedUsers.Items)
            {
                SelectUsersId += $"{userId}*";
            }
            this.DialogResult = DialogResult.OK;
        }

       

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (lBoxUsers.SelectedItems.Count > 0)
            {
                lBoxSelectedUsers.BeginUpdate();
                foreach (var sle in lBoxUsers.SelectedItems)
                {
                    if ((lBoxSelectedUsers.Items.Contains(sle)))
                        lBoxSelectedUsers.Items.Remove(sle);
                    Console.WriteLine();
                }
                lBoxSelectedUsers.EndUpdate();
            }
        }
    }
}
