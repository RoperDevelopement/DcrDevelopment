using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
namespace Edocs.Upload.Batches.AzureCloud
{
    class AssemblyInfo
    {
        internal static string GetAssemblyDescription()
        {
            object[] assAttribute = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);

            if (assAttribute.Length > 0)
            {


                return ((AssemblyDescriptionAttribute)assAttribute[0]).Description;


            }
            return string.Empty;
        }
        internal static string GetAssemblyVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
        internal static string GetAssemblyTitle()
        {
            object[] assAttribute = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);

            if (assAttribute.Length > 0)
            {
                AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)assAttribute[0];
                if (!(string.IsNullOrEmpty(titleAttribute.Title)))
                    return titleAttribute.Title;

            }
            return string.Empty;
        }
        internal static string GetAssemblyCopyright()
        {
            object[] assAttribute = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);

            if (assAttribute.Length > 0)
            {


                return ((AssemblyCopyrightAttribute)assAttribute[0]).Copyright;

            }
            return " ";
        }
    }
}
