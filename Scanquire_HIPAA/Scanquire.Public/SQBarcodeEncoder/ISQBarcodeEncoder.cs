using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Scanquire.Public
{
    /// <summary>Interface for encoding and decoding barcode values</summary>
    public interface ISQBarcodeEncoder
    {
        /// <summary>Attempt to encode a command to a barcode value.</summary>
        /// <param name="command">The command to encode.</param>
        /// <param name="barcodeText">If successful, the encoded text value to apply to the barcode.</param>
        /// <param name="caption">If successful, the human-readable caption to apply to the barcode.</param>
        /// <returns>True on success, False on failure.</returns>
        bool TryEncode(ISQCommand command, out string barcodeText, out string caption);

        /// <summary>Attempt to decode a barcode value to a series of commands.</summary>
        /// <param name="text">The encoded text to decode.</param>
        /// <param name="commands">If successful, a list containing all commands decoded from the barcode text.</param>
        /// <returns>True on success, False on failure.</returns>
        bool TryParse(string text, out IEnumerable<ISQCommand> commands);
    }
}
