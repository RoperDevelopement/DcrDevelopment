using System;
using System.Windows.Forms;
using EdocsUSA.Controls;
using SP = Microsoft.SharePoint.Client;

namespace Scanquire.Public.Sharepoint
{
	public class SharepointCredentialsInputDialog : TableLayoutPanelInputDialog
	{
		public ValidatingTextBox<string> Host = new ValidatingTextBox<string>() 
		{ RequiresValue = true };
		
		public ValidatingTextBox<string> Domain = new ValidatingTextBox<string>()
		{ RequiresValue = true };
		
		public ValidatingTextBox<string> User = new ValidatingTextBox<string>()
		{ RequiresValue = true };
		
		public ValidatingTextBox<string> Password  = new ValidatingTextBox<string>()
		{ 
			RequiresValue = true,
			PasswordChar = '*'
		};
		
		GroupBox AuthModeGroupBox = new GroupBox()
		{ 
			Size = new System.Drawing.Size(130,75),
			Text = "Authentication Mode"
		};
		
		RadioButton FormsAuthModeRad = new RadioButton()
		{
			Text = "Forms",
			Checked = false
		};
		
		RadioButton WindowsAuthModeRad = new RadioButton()
		{
			Text = "Windows",
			Checked = true
		};
		
		public SP.ClientAuthenticationMode AuthenticationMode
		{
			get
			{
				if (FormsAuthModeRad.Checked) return SP.ClientAuthenticationMode.FormsAuthentication;
				else if (WindowsAuthModeRad.Checked) return SP.ClientAuthenticationMode.Default;
				else throw new InvalidOperationException("Invalid authentication mode");
			}
			set
			{
				if (value == SP.ClientAuthenticationMode.FormsAuthentication) FormsAuthModeRad.Checked =true;
				else if (value == SP.ClientAuthenticationMode.Default) WindowsAuthModeRad.Checked = true;
				else throw new ArgumentException("Invalid authentication mode");
			}
		}
		
		private void InitializeComponent()
		{
			AddControl(new CaptionLabel("Host"), 0, 0);
			AddControl(Host, 1, 0);
			AddControl(new CaptionLabel("Domain"), 0, 1);
			AddControl(Domain, 1, 1);
			AddControl(new CaptionLabel("User"), 0, 2);
			AddControl(User, 1, 2);
			AddControl(new CaptionLabel("Password"), 0, 3);
			AddControl(Password, 1, 3);
			AddControl(AuthModeGroupBox, 1, 4);
		}
		
		public SharepointCredentialsInputDialog() : base(2, 5)
		{ InitializeComponent(); }
	}
}
