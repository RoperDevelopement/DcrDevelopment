using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BinMonitor.Common
{
    public partial class MasterCategoryPermissionControl : UserControl
    {
        public string Caption
        {
            get { return lblCaption.Text; }
            set { lblCaption.Text = value; }
        }

        public MasterCategoryPermissions GetValue()
        {
            if (ValidateConfiguration() == false)
            { return null; }
            else
            {
                MasterCategoryPermissions perm = new MasterCategoryPermissions();
                perm.CanCreate = chkCanCreate.Checked;
                perm.CanAddComment = chkCanAddComments.Checked;
                perm.CanAssign = chkCanAssign.Checked;
                perm.CanCheckOut = chkCanCheckOut.Checked;
                perm.CanCheckIn = chkCanCheckIn.Checked;
                perm.CanClose = chkCanClose.Checked;
                return perm;
            }
        }

        public void LoadFromExisting(MasterCategoryPermissions permissions)
        {
            if (permissions == null)
            { 
                Clear();
                return;
            }

            chkCanCreate.Checked = permissions.CanCreate;
            chkCanAddComments.Checked = permissions.CanAddComment;
            chkCanAssign.Checked = permissions.CanAssign;
            chkCanCheckOut.Checked = permissions.CanCheckOut;
            chkCanCheckIn.Checked = permissions.CanCheckIn;
            chkCanClose.Checked = permissions.CanClose;
        }

        public MasterCategoryPermissions EnsureGetValue()
        {
            MasterCategoryPermissions perm = GetValue();
            if (perm == null)
            { throw new InvalidOperationException("The specified configuration is invalid."); }
            return perm;
        }

        public bool ValidateConfiguration()
        {
            return true;
        }

        public void Clear()
        {
            chkCanCreate.Checked = false;
            chkCanAddComments.Checked = false;
            chkCanAssign.Checked = false;
            chkCanCheckOut.Checked = false;
            chkCanCheckIn.Checked = false;
            chkCanClose.Checked = false;
        }

        public MasterCategoryPermissionControl()
        {
            InitializeComponent();
        }
    }
}