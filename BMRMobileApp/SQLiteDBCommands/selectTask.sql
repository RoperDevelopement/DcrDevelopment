SELECT ps.ID as ID,
TaskName as TaskName,
TaskDescription as TaskDescription,
TaskDateAdded as TaskDateAdded,
TaskDateDone as TaskDateDone,
IsTaskComplete as IsTaskComplete,
TaskDueDate as TaskDueDate,
TaskNotfication as  TaskNotfication,
case
when ps.TaskEmotionsID > 0 then et.Emotion
else 'N/A'
end as Emotion,
case
when ps.TaskEmotionsID > 0 then  et.EmotionIcon
else 'N/A'
end as EmotionIcon,
case
when ps.TaskEmotionsID > 0 then et.EmotionsColor
else 'N/A'
end as EmotionsColor,
cat.TaskCategoryName as TaskCategoryName
from PSUserTask ps inner JOIN PSEmotionsTag et on ps.TaskEmotionsID=et.ID
inner join PSTaskCatgory cat on ps.TaskCategoryID = cat.ID
where ps. IsTaskComplete = 0
