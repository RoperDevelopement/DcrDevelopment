using System;

namespace Scanquire.Public
{
    /// <summary>Defines an arbitrary way to modify images</summary>
	public interface ISQImageProcessor
	{
		void ProcessImage(ref SQImage image);
	}
}
