using System;
using System.Drawing;
using System.Windows.Forms;

namespace EdocsUSA.Controls
{
	public class CaptionLabel : Label
	{
		public CaptionLabel(string caption) : base()
		{
			AutoSize = true;
			Text = caption;
			Anchor = AnchorStyles.Right;
		}
	}
}