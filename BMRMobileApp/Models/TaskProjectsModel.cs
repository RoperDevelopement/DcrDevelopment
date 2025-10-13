using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMRMobileApp.Models
{
 public   class TaskProjectsModel
    {

        public int ID { get; set; }

        public string TaskName { get; set; }

        public string TaskDescription { get; set; }

        public string TaskDateAdded { get; set; }

        public string TaskDateDone { get; set; }


        public int IsTaskComplete { get; set; }

        public string TaskDueDate { get; set; }

        public string TaskCategoryName { get; set; }
        
        public int TaskEmotionsID { get; set; }
        
        public int TaskNotfication { get; set; }
        public int TagID { get; set; }

    }
}
