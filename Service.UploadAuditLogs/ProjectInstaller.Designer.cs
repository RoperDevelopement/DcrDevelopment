namespace Edocs.Service.UploadAuditLogs
{
    partial class ProjectInstaller
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.serviceProcessInstaller1 = new System.ServiceProcess.ServiceProcessInstaller();
            this.serviceInstaller1 = new System.ServiceProcess.ServiceInstaller();
            // 
            // serviceProcessInstaller1
            // 
            //this.serviceProcessInstaller1.Password = "P@ssw0rd1234";
            //this.serviceProcessInstaller1.Username = "edocsSQLServer\\eDocsAdmin";
            //this.serviceProcessInstaller1.Password = null;
            //this.serviceProcessInstaller1.Username = null;
            //if (string.Compare(System.Environment.MachineName, "edocsSQLServer", false) == 0)
            //{
                this.serviceProcessInstaller1.Password = "edocs6746@";
                this.serviceProcessInstaller1.Username = ".\\edocs";
            //}
            //else if (string.Compare(System.Environment.MachineName, "Queens", false) == 0)
            //{
            //    this.serviceProcessInstaller1.Password = "Edocs6746@";
              //  this.serviceProcessInstaller1.Username = "Queens\\edocs";
            //}
            //else
            //{
            //    this.serviceProcessInstaller1.Password = "edocs6746@";
            //    this.serviceProcessInstaller1.Username = ".\\scanazurecloud\\edocs";
            //}

            this.serviceInstaller1.Description = "Uplaod audit logs to azure cloud";
            this.serviceInstaller1.DisplayName = "Edocs USA Upload audit logs";

            this.serviceInstaller1.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // serviceInstaller1
            // 
            this.serviceInstaller1.ServiceName = "EdocsUSAAuditLogs";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceProcessInstaller1,
            this.serviceInstaller1});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller1;
        private System.ServiceProcess.ServiceInstaller serviceInstaller1;
    }
}