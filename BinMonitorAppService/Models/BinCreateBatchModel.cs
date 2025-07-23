using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace BinMonitorAppService.Models
{
    public class BinCreateBatchModel
    {
        public Guid BatchID
        { get; set; }
        [Required]
        [Display(Name = "Bin ID")]
        public string BinID
        { get; set; }
        [Required]
        [Display(Name = "Category")]
        public string MasterCategorie
        { get; set; }

        public string CreatedBy
        { get; set; }
        [Display(Name = "Assign Registration")]
        public string AssignedBy
        { get; set; }
        [Display(Name = "Assign Processing")]
        public string AssignedTo
        { get; set; }
        public DateTime StatedAt
        { get; set; }
        public string CompletedBy
        { get; set; }
        public DateTime CompletedAt
        { get; set; }
        [MaxLength(5000)]
        public string Conetnts
        { get; set; }
        public bool IsRegistered
        { get; set; }
        
    }
}
