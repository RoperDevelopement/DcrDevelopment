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
    public partial class WorkflowStepViewer : UserControl
    {
        public WorkflowStepViewer()
        {
            InitializeComponent();
        }

        public string Caption
        {
            get { return lblCaption.Text; }
            set { lblCaption.Text = value; }
        }

        public void Clear()
        {
            this.BackColor = SystemColors.Control;
            txtStartedAt.Clear();
            txtAssignedBy.Clear();
            txtAssignedTo.Clear();
            txtCompletedAt.Clear();
            txtCompletedBy.Clear();
            txtDuration.Clear();            
        }

        public void LoadFromExisting(WorkflowStep step)
        {
            this.Clear();
            if (step.HasStarted)
            {
                this.BackColor = Color.LightSalmon;
                txtStartedAt.Text = step.StartedAt.ToString();
                txtAssignedBy.Text = step.AssignedBy;
                txtAssignedTo.Text = step.AssignedTo;
                if (step.HasCompleted)
                {
                    this.BackColor = Color.LightSeaGreen;
                    txtCompletedAt.Text = step.CompletedAt.ToString();
                    txtCompletedBy.Text = step.CompletedBy;
                    txtDuration.Text = step.GetDuration().ToString();
                }
            }
            else
            { this.BackColor = SystemColors.Control; }
        }
    }
}
