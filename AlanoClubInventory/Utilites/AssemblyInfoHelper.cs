using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Reflection;
namespace AlanoClubInventory.Utilites
{


    public static class AssemblyInfoHelper
    {
        public static string GetTitle()
        {
            var attr = (AssemblyTitleAttribute)Attribute.GetCustomAttribute(
                Assembly.GetExecutingAssembly(), typeof(AssemblyTitleAttribute));
            return attr?.Title ?? "Unknown Title";
        }

        public static string GetDescription()
        {
            var attr = (AssemblyDescriptionAttribute)Attribute.GetCustomAttribute(
                Assembly.GetExecutingAssembly(), typeof(AssemblyDescriptionAttribute));
            return attr?.Description ?? "No Description";
        }

        public static string GetCompany()
        {
            var attr = (AssemblyCompanyAttribute)Attribute.GetCustomAttribute(
                Assembly.GetExecutingAssembly(), typeof(AssemblyCompanyAttribute));
            return attr?.Company ?? "Unknown Company";
        }

        public static string GetProduct()
        {
            var attr = (AssemblyProductAttribute)Attribute.GetCustomAttribute(
                Assembly.GetExecutingAssembly(), typeof(AssemblyProductAttribute));
            return attr?.Product ?? "Unknown Product";
        }

        public static string GetVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "Unknown Version";
        }

        public static string GetFileVersion()
        {
            var attr = (AssemblyFileVersionAttribute)Attribute.GetCustomAttribute(
                Assembly.GetExecutingAssembly(), typeof(AssemblyFileVersionAttribute));
            return attr?.Version ?? "Unknown File Version";
        }

        public static string GetInformationalVersion()
        {
            var attr = (AssemblyInformationalVersionAttribute)Attribute.GetCustomAttribute(
                Assembly.GetExecutingAssembly(), typeof(AssemblyInformationalVersionAttribute));
            return attr?.InformationalVersion ?? "Unknown Informational Version";
        }
     
    public static string GetCopyright()
        {
            var attr = (AssemblyCopyrightAttribute)Attribute.GetCustomAttribute(
                Assembly.GetExecutingAssembly(), typeof(AssemblyCopyrightAttribute));
            return attr?.Copyright ?? "No copyright info";
        }

    }
}
