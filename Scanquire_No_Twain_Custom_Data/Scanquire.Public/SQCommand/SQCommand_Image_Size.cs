using System;
using System.Drawing;

namespace Scanquire.Public
{
    /// <summary>A command used to specify an image's drawn size.</summary>
    public class SQCommand_Image_Size : ISQCommand_Image
	{
        private readonly SizeF _SizeF;
        /// <summary>Size (in points)</summary>
		public SizeF SizeF { get { return _SizeF; } }

        /// <summary>Width (in points)</summary>
		public float Width { get { return SizeF.Width; } }
		
		/// <summary>Height (in points)</summary>
		public float Height { get { return SizeF.Height; } }
		
		public SQCommand_Image_Size(SizeF size)
		{
			this._SizeF = size;
		}

        public SQCommand_Image_Size(float width, float height)
            : this(new SizeF(width, height))
		{ }
	}
}
