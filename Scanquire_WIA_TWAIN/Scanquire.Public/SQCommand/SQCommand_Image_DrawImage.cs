using EdocsUSA.Utilities;
using System;
using System.Drawing;

namespace Scanquire.Public
{
    /// <summary>A command to specify that an image be overlain on the specified image.</summary>
	public class SQCommand_Image_DrawImage : ISQCommand_Image
	{		
		public SQImage Image { get; set; }

        public SQCommand_Image_DrawImage(SQImage image)
        { this.Image = image; }
	}
}
