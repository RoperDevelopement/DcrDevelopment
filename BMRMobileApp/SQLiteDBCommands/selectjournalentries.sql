SELECT je.ID as ID,
je.PSUserID as PSUserID,
je.JournalEntry as JournalEntry,
je.DateAdded as DateAdded,
je.JournalGoalsID,
et.Emotion as Emotion,
et.EmotionIcon as EmotionIcon,
et.EmotionsColor as EmotionsColor

FROM PSJJournalEntry je join PSEmotionsTag et on je.EmotionTagID = et.ID 

where je.DateAdded between strftime('%Y-%m-%d', '2025-09-17') and strftime('%Y-%m-%d', '2025-09-19');

   