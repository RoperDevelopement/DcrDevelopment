using DTKBarReader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scanquire.Public
{
    public interface ISQBarcodeReader
    {
        /// <summary>Read all barcodes from an image.</summary>
        /// <param name="image">The image to read.</param>
        /// <returns>List of barcodes read from the specified image</returns>
        Task<Barcode[]> Read(SQImage image, CancellationToken cToken);

        /// <summary>Read all barcodes from an image.</summary>
        /// <param name="image">The image to read.</param>
        /// <returns>List of barcodes strings from the specified image</returns>
        Task<string[]> ReadText(SQImage image, CancellationToken cToken);
    }

    public abstract class SQBarcodeReaderBase : ISQBarcodeReader
    {
        public abstract Task<Barcode[]> Read(SQImage image, CancellationToken cToken);

        public virtual Task<string[]> ReadText(SQImage image, CancellationToken cToken)
        {
            return Task.Factory.StartNew<string[]>(() =>
            { return Read(image, cToken).Result.Select(barcode => barcode.BarcodeString).ToArray(); });
        }
    }
}
