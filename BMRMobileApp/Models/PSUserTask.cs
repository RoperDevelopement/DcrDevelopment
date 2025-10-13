using BMRMobileApp.InterFaces;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BMRMobileApp.Models
{
    public class PSUserTask : ITaskProjects, IPSID
    {
        [AutoIncrement, PrimaryKey, Unique]
        public int ID { get; set; }
        [NotNull]
        public string TaskName { get; set; }
        [NotNull]
        public string TaskDescription { get; set; }
        [NotNull]
        public string TaskDateAdded { get; set; }
        [NotNull]
        public string TaskDateDone { get; set; }
       
        [NotNull]
        public int IsTaskComplete { get; set; }
        [NotNull]
        public string TaskDueDate { get; set; }
        [NotNull]
        public int TaskCategoryID { get; set; }
        [NotNull]
        public int TaskEmotionsID { get; set; }
        [NotNull]
        public int TaskNotfication { get; set; }
        public int TagID { get; set; }

    }
}
