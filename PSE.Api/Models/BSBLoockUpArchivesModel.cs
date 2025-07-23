using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Edocs.Interfaces;
namespace Edocs.PSE.Api.Models
{
    public class BSBLoockUpArchivesModel:IBSBArchives
    {
       public int ArchiveID
        { get; set; }
        [Display(Name = "Date:")]
        public string ArchiveDate
        { get; set; }
       public string FileUrl
        { get; set; }
        [Display(Name = "Title:")]
        public string ArchiveTitle
        { get; set; }
        [Display(Name = "Collection:")]
        public string ArchiveCollection
        { get; set; }
        [Display(Name = "Description:")]
        public string ArchiveDescription
        { get; set; }
        [Display(Name = "KeyWord:")]
        public string KeyWord
        { get; set; }
    }
}
