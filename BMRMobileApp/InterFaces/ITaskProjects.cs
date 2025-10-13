using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMRMobileApp.InterFaces
{
  public interface ITaskCategory
    {
        string TaskCategoryName { get; set; }
    }
    public interface ITaskIconTags
    {
        string TaskTags { get; set; }
        string Descripotion { get; set; }
    }
    public interface ITaskNameDescription
    {
        string TaskName { get; set; }
        string TaskDescription { get; set; }
    }
    public interface ITaskProjects: ITagID,ITaskNameDescription
    {
        
        
        string TaskDateAdded { get; set; }
        string TaskDateDone { get; set; }
        
        string TaskDueDate {  get; set; }
        
         
        int IsTaskComplete { get; set; }
        
        int TaskCategoryID { get; set; }
        int TaskEmotionsID { get; set; }
        int TaskNotfication { get; set; }
    }
    public interface ITagID
    {
        int TagID { get; set; }
    }
    public interface IToDoTaskTags: IPSTablesIndexID
    {
        string TagName { get; set; }
        string TagColor { get; set; }
    }
    public interface IUserGoals:IPSTablesIndexID
    {
        string UserGoals { get; set; }
    }
}
