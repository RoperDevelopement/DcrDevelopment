/*
 * User: Sam Brinly
 * Date: 2/13/2013
 */
using System;
using DTKBarReader;
using ETL = EdocsUSA.Utilities.Logging.TraceLogger;
namespace Scanquire.Public
{
    /// <summary>Wrapper around DTK's barcode reader to apply the license key.</summary>
	public class BarcodeReaderEx : BarcodeReader
	{
		private const string DTK_LICENSE_KEY = "WREXXH7GD92SZLPNFJGK";

      
        public BarcodeReaderEx() : base()
		{
            ETL.TraceLoggerInstance.TraceInformation($"Opening barcode reader using key:{DTK_LICENSE_KEY}");
         //  LicenseManager.AddLicenseKey(DTK_LICENSE_KEY);
			//this.SetRegistrationCode(DTK_LICENSE_KEY);
		}
	}
}
