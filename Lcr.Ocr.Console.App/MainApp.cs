using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Edocs.Icr.Ocr.Google;
namespace Lcr.Ocr.Console.App
{
    class MainApp
    {
        static void Main(string[] args)
        {
            string google = GoogleLCROCR.Instance.ProcessImage(args[0], "image/png");
            if (!(string.IsNullOrWhiteSpace(google)))
            {
                string fileName = $"{System.IO.Path.Combine(args[2],System.IO.Path.GetFileNameWithoutExtension(args[1]))}.txt";
                System.IO.File.WriteAllText($"{args[2]}", google);
            }
                
        }
    }
}
