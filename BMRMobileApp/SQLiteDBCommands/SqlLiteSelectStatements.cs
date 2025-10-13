
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BMRMobileApp.SQLiteDBCommands
{
    public class SqlLiteSelectStatements
    {
        //public static string SelectOpenTask = "SELECT ps.ID as ID,TaskName as TaskName,TaskDescription as TaskDescription,TaskDateAdded as TaskDateAdded,TaskDateDone as TaskDateDone," +
        //                                "IsTaskComplete as IsTaskComplete,TaskDueDate as TaskDueDate,TaskNotfication as  TaskNotfication," +
        //                                "case when ps.TaskEmotionsID > 0 then et.Emotion else 'N/A' end as Emotion," +
        //                                "case when ps.TaskEmotionsID > 0 then et.EmotionIcon else 'N/A' end as EmotionIcon," +
        //                                "case when ps.TaskEmotionsID > 0 then et.EmotionsColor else 'N/A' end as EmotionsColor," +
        //                                "cat.TaskCategoryName as TaskCategoryName from PSUserTask ps inner JOIN PSEmotionsTag et on ps.TaskEmotionsID = et.ID " +
        //                                "inner join PSTaskCatgory cat on ps.TaskCategoryID = cat.ID " +
        //                                "where ps. IsTaskComplete = 0";
        public static string SelectOpenTask = "SELECT ps.ID as ID,TaskName as TaskName,TaskDescription as TaskDescription,TaskDateAdded as TaskDateAdded,TaskDateDone as TaskDateDone," +
                                       "IsTaskComplete as IsTaskComplete,TaskDueDate as TaskDueDate,TaskNotfication as  TaskNotfication,ps.TaskEmotionsID as TaskEmotionsID, " +
                                       "cat.TaskCategoryName as TaskCategoryName from PSUserTask ps  " +
                                       "inner join PSTaskCatgory cat on ps.TaskCategoryID = cat.ID " +
                                       "where ps. IsTaskComplete = 0";
        public static string SelectTagsIDs = "Select tid.ID as ID,t.TagName as TagName,t.TagColor as TagColor" +
                                              " from PSUserTask ps join PSTaskTagIDs tid on tid.ID=ps.ID join PSTaskTags t on tid.TagID= t.ID" +
                                              " where ps.IsTaskComplete = 0";
        public static string SelectEmotions = "Select ps.ID as ID,e.Emotion as Emotion,e.EmotionIcon as EmotionIcon,e.EmotionsColor as EmotionsColor " +
                                              " from PSUserTask ps join PSEmotionsTag e on ps.TaskEmotionsID = e.ID" +
                                               " where ps.IsTaskComplete = 0";
        public static string SqlCloseTask = "Update PSUserTask set isTaskComplete=1 where ID={0}";
        public static string SelectJournalEntries = "SELECT je.ID as ID,je.PSUserID as PSUserID,je.JournalEntry as JournalEntry,je.DateAdded as DateAdded," +
                                                    "je.JournalGoalsID,et.Emotion as Emotion,et.EmotionIcon as EmotionIcon," +
                                                     "et.EmotionsColor as EmotionsColor " +
                                                      " FROM PSJJournalEntry je join PSEmotionsTag et on je.EmotionTagID = et.ID " +
                                                        " where je.DateAdded between strftime('%Y-%m-%d', '{0}') and strftime('%Y-%m-%d','{1}');";
        public static string SelectJournalEntriesGoals = "SELECT ID as ID,PSUserID as PSUserID,JournalEntry as JournalEntry,DateAdded as DateAdded" +
                                                       " FROM PSJounalEntryGoals  jeg" +
                                                       " where jeg.DateAdded between strftime('%Y-%m-%d', '{0}') and strftime('%Y-%m-%d','{1}');";
        public static string SelectEmotionsCount = "select count(*) as ValueLabel ,et.Emotion as Label  ,et.EmotionsColor as EmotionsColor " +
                                                    "FROM PSJJournalEntry je join PSEmotionsTag et on je.EmotionTagID = et.ID" +
                                                    "  where je.DateAdded between strftime('%Y-%m-%d', '{0}') and strftime('%Y-%m-%d','{1}') " +
                                                     " GROUP by et.Emotion";

    }
}
