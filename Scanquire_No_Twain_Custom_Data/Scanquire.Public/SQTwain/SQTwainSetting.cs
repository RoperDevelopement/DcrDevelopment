/*
 * User: Sam Brinly
 * Date: 3/27/2013
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ETL = EdocsUSA.Utilities.Logging.TraceLogger;
namespace Scanquire.Public
{
	public class SQTwainSetting
	{
        /// <summary>Device name as it appears in the device list.</summary>
		public string ScannerName { get; set; }
		
        /// <summary>
        /// If true, display the device's native UI when acquiring.
        /// If false, do not display the device's native UI when acquiring.
        /// </summary>
		public bool ShowDeviceUI { get; set; }
		
        /// <summary>Configuration values for the device.  The value is retrieved from the device.</summary>
		public byte[] CustomDSData { get; set; }
		
		private List<string> _ImageProcessorNames = new List<string>();
        /// <summary>List of names of image processors to apply to all acquired images.</summary>
		public List<string> ImageProcessorNames
		{
			get { return _ImageProcessorNames; }
			set { _ImageProcessorNames = value; }
		}
		
        /// <summary>The image processors to apply to all acquired images.</summary>
		public IEnumerable<ISQImageProcessor> ImageProcessors
		{ 
			get 
			{ 
				foreach (string imageProcessorName in ImageProcessorNames)
				{
					if (SQImageProcessors.Instance.ContainsKey(imageProcessorName))
					{ yield return SQImageProcessors.Instance[imageProcessorName]; }
					else{ ETL.TraceLoggerInstance.TraceWarning("Invalid image processor " + imageProcessorName); }
				}
			}
		}
		
		public SQTwainSetting()
		{}
		
		public SQTwainSetting(byte[] customDSData) {CustomDSData = customDSData; }
		
		public SQTwainSetting(byte[] customDSData, IEnumerable<string> imageProcessorNames)
		{
			CustomDSData = customDSData;
			imageProcessorNames = imageProcessorNames.ToList();
		}
		
		public SQTwainSetting(byte[] customDSData, params string[] imageProcessorNames)
		{
			CustomDSData = customDSData;
			ImageProcessorNames = imageProcessorNames.ToList();
		}
	}
}
