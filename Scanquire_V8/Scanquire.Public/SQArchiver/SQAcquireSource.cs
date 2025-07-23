using System;

namespace Scanquire.Public
{
    /// <summary>The requested image source for an acquire command.</summary>
	public enum SQAcquireSource
	{
		Scanner,
		File,
        Command,
        Custom
	}
}
