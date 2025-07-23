/*
 * User: Sam Brinly
 * Date: 3/25/2013
 */
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;
using EdocsUSA.Controls;
using EdocsUSA.Utilities;
using Scanquire.Public;

namespace DemoArchivers
{
	/// <summary>
	/// Description of EmailArchiver.
	/// </summary>
	public class EmailArchiver : SQArchiverBase
	{
		protected class EmailInputDialog : TableLayoutPanelInputDialog
		{
			public ValidatingTextBox<string> ReplyTo;
			public ValidatingTextBox<string> Recipients;
			public ValidatingTextBox<string> Subject;
			public ValidatingTextBox<string> Body;
			public ValidatingTextBox<string> AttachmentName;
			
			private void InitializeComponent()
			{
				this.Text = "Email Archiver";
				this.Width = 500;
				
				AddControl(new CaptionLabel("Your Address"), 0, 0);
				ReplyTo = new ValidatingTextBox<string>()
				{ 
					Width = 450,
					RequiresValue = true,
					CharacterCasing = CharacterCasing.Upper
				};
				AddControl(ReplyTo, 1, 0);
				
				AddControl(new CaptionLabel("Recipients"), 0, 1);
				Recipients =new ValidatingTextBox<string>()
				{
					Width = 450,
					RequiresValue = true,
					CharacterCasing = CharacterCasing.Upper
				};
				AddControl(Recipients, 1, 1);
				
				AddControl(new CaptionLabel("Subject"), 0, 2);
				Subject = new ValidatingTextBox<string>()
				{ 
					Width = 450,
					RequiresValue = true 
				};
				AddControl(Subject, 1, 2);
				
				AddControl(new CaptionLabel("Body"), 0, 3);
				Body = new ValidatingTextBox<string>()
				{ 
					Width = 450,
					RequiresValue = true
				};
				AddControl(Body, 1, 3);
				
				AddControl(new CaptionLabel("Attachment Name"), 0, 4);
				AttachmentName = new ValidatingTextBox<string>()
				{ 
					RequiresValue = true,
					Width = 450,
				};
				AddControl(AttachmentName, 1, 4);
			}
			
			public EmailInputDialog() : base(2, 5)
			{
				InitializeComponent();
			}
			
			public void Clear()
			{
				Recipients.Clear();
				Subject.Clear();
				Body.Clear();
				AttachmentName.Clear();
			}
			
			protected override void OnShown(EventArgs e)
			{
				base.OnShown(e);
				if (ReplyTo.Value.IsEmpty())
				{ ReplyTo.Focus(); }
				else { Recipients.Focus(); }
			}
		}
		
		protected string EmailUserName = "automated@edocsusa.com";
		protected string EmailFrom = "automated@edocsusa.com";
		protected string EmailPassword = "6746edocs";
		protected string EmailServer = "mail32.webcontrolcenter.com";
		
		protected EmailInputDialog InputDialog = new EmailInputDialog();
		
		public EmailArchiver() 
		{
			
		}
		
		public override void Send(SQFile file)
		{
			if (InputDialog.ShowDialog() == DialogResult.OK)
			{
				Application.DoEvents();
				string fileName = Path.ChangeExtension(InputDialog.AttachmentName.Value, file.FileExtension);
				MailMessage message = new MailMessage();
				message.Sender = new MailAddress(EmailUserName);
				message.From = new MailAddress(EmailFrom);
				foreach (string recipient in InputDialog.Recipients.Value.Split(';'))
				{ message.To.Add(recipient); }
				
				message.ReplyToList.Add(new MailAddress(InputDialog.ReplyTo.Value));
				
				message.Subject = InputDialog.Subject.Value;
				message.Body = InputDialog.Body.Value;
				message.IsBodyHtml = false;
				SmtpClient mailClient = new SmtpClient(EmailServer);
				mailClient.Port = 8889;
				mailClient.Credentials = new NetworkCredential(EmailUserName, EmailPassword);
				
				using (MemoryStream attachmentStream = new MemoryStream(file.Data))
				{
					message.Attachments.Add(new Attachment(attachmentStream, fileName));
					try
					{
						mailClient.Send(message);
					}
					catch (Exception ex)
					{ 
						Trace.TraceError(ex.Message); 
						Trace.TraceError(ex.StackTrace);
						throw ex;
					}
					
				}
				
				InputDialog.Clear();
			}
			else throw new OperationCanceledException(); 
		}
	}
}
