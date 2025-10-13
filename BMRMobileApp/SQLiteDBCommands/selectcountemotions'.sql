select count(*) as Emotions,et.Emotion
FROM PSJJournalEntry je join PSEmotionsTag et on je.EmotionTagID = et.ID
GROUP by et.Emotion