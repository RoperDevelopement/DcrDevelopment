using BMRMobileApp.InterFaces;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BMRMobileApp.Models
{
    public class PSRecourceSites : ISupportWebSites
    {
        [AutoIncrement, PrimaryKey, Unique]
        public int ID { get; set; }
        [NotNull]
        public string SupportedSiteName { get; set; }
        [NotNull]
        public string SupportWebSites { get; set; }
        [NotNull]
        public int DeleteResource {get;set;}
    }
}
