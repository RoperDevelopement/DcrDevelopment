Select tid.ID as ID,t.TagName as TagName,t.TagColor as TagColor
from PSUserTask ps join PSTaskTagIDs tid on tid.ID=ps.ID join PSTaskTags t on tid.TagID= t.ID
where ps.IsTaskComplete = 0