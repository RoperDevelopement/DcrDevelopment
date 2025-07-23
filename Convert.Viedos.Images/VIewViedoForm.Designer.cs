
namespace Edocs.Convert.Viedos.Images
{
    partial class VIewViedoForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VIewViedoForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.axWindowsMediaPlayer1 = new AxWMPLib.AxWindowsMediaPlayer();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.ts10 = new System.Windows.Forms.ToolStripMenuItem();
            this.ts15 = new System.Windows.Forms.ToolStripMenuItem();
            this.ts20 = new System.Windows.Forms.ToolStripMenuItem();
            this.ts25 = new System.Windows.Forms.ToolStripMenuItem();
            this.ts30 = new System.Windows.Forms.ToolStripMenuItem();
            this.ts35 = new System.Windows.Forms.ToolStripMenuItem();
            this.ts40 = new System.Windows.Forms.ToolStripMenuItem();
            this.ts45 = new System.Windows.Forms.ToolStripMenuItem();
            this.ts50 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton2,
            this.toolStripDropDownButton1,
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(800, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // axWindowsMediaPlayer1
            // 
            this.axWindowsMediaPlayer1.Enabled = true;
            this.axWindowsMediaPlayer1.Location = new System.Drawing.Point(159, 99);
            this.axWindowsMediaPlayer1.Name = "axWindowsMediaPlayer1";
            this.axWindowsMediaPlayer1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWindowsMediaPlayer1.OcxState")));
            this.axWindowsMediaPlayer1.Size = new System.Drawing.Size(410, 269);
            this.axWindowsMediaPlayer1.TabIndex = 0;
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.toolStripDropDownButton1.Image = global::Edocs.Convert.Viedos.Images.Properties.Resources.responsible;
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(78, 22);
            this.toolStripDropDownButton1.Text = "&Settings";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ts10,
            this.ts15,
            this.ts20,
            this.ts25,
            this.ts30,
            this.ts35,
            this.ts40,
            this.ts45,
            this.ts50});
            this.toolStripMenuItem1.Image = global::Edocs.Convert.Viedos.Images.Properties.Resources.film;
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(122, 22);
            this.toolStripMenuItem1.Text = "Play Rate";
            // 
            // ts10
            // 
            this.ts10.Checked = true;
            this.ts10.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ts10.Name = "ts10";
            this.ts10.Size = new System.Drawing.Size(89, 22);
            this.ts10.Text = "1.0";
            this.ts10.Click += new System.EventHandler(this.ts10_Click);
            // 
            // ts15
            // 
            this.ts15.Name = "ts15";
            this.ts15.Size = new System.Drawing.Size(89, 22);
            this.ts15.Text = "1.5";
            this.ts15.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // ts20
            // 
            this.ts20.Name = "ts20";
            this.ts20.Size = new System.Drawing.Size(89, 22);
            this.ts20.Text = "2.0";
            this.ts20.Click += new System.EventHandler(this.ts20_Click);
            // 
            // ts25
            // 
            this.ts25.Name = "ts25";
            this.ts25.Size = new System.Drawing.Size(89, 22);
            this.ts25.Text = "2.5";
            this.ts25.Click += new System.EventHandler(this.ts25_Click);
            // 
            // ts30
            // 
            this.ts30.Name = "ts30";
            this.ts30.Size = new System.Drawing.Size(89, 22);
            this.ts30.Text = "3.0";
            this.ts30.Click += new System.EventHandler(this.ts30_Click);
            // 
            // ts35
            // 
            this.ts35.Name = "ts35";
            this.ts35.Size = new System.Drawing.Size(89, 22);
            this.ts35.Text = "3.5";
            this.ts35.Click += new System.EventHandler(this.ts35_Click);
            // 
            // ts40
            // 
            this.ts40.Name = "ts40";
            this.ts40.Size = new System.Drawing.Size(89, 22);
            this.ts40.Text = "4.0";
            this.ts40.Click += new System.EventHandler(this.ts40_Click);
            // 
            // ts45
            // 
            this.ts45.Name = "ts45";
            this.ts45.Size = new System.Drawing.Size(89, 22);
            this.ts45.Text = "4.5";
            this.ts45.Click += new System.EventHandler(this.ts45_Click);
            // 
            // ts50
            // 
            this.ts50.Name = "ts50";
            this.ts50.Size = new System.Drawing.Size(89, 22);
            this.ts50.Text = "5.0";
            this.ts50.Click += new System.EventHandler(this.ts50_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Image = global::Edocs.Convert.Viedos.Images.Properties.Resources.exit128;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(46, 22);
            this.toolStripButton1.Text = "&Exit";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Image = global::Edocs.Convert.Viedos.Images.Properties.Resources.video_player1;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(147, 22);
            this.toolStripButton2.Text = "&Windows Media Player";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // VIewViedoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.axWindowsMediaPlayer1);
            this.Name = "VIewViedoForm";
            this.Text = "&Windows Media Player";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.VIewViedoForm_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AxWMPLib.AxWindowsMediaPlayer axWindowsMediaPlayer1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem ts15;
        private System.Windows.Forms.ToolStripMenuItem ts20;
        private System.Windows.Forms.ToolStripMenuItem ts25;
        private System.Windows.Forms.ToolStripMenuItem ts30;
        private System.Windows.Forms.ToolStripMenuItem ts35;
        private System.Windows.Forms.ToolStripMenuItem ts40;
        private System.Windows.Forms.ToolStripMenuItem ts45;
        private System.Windows.Forms.ToolStripMenuItem ts50;
        private System.Windows.Forms.ToolStripMenuItem ts10;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
    }
}