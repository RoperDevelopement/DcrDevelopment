using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace ConvertToBinary
{
    class Program
    {
        static void Main(string[] args)
        {
            //string inStr = File.ReadAllText(@"l:\SUTR4500.ini");
            // byte[] strTOByte = Encoding.ASCII.GetBytes(inStr);
            // File.WriteAllBytes(@"l:\bytes.txt", strTOByte);
            byte[] b = File.ReadAllBytes(@"C:\Users\mtcha\AppData\Roaming\Canon\TR4500 series\SCNUI.DAT");
            string  s = Encoding.Default.GetString(b);
        }    
    }
}
