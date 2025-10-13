using Azure;
using Azure.AI.TextAnalytics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMRMobileApp.Models;
namespace BMRMobileApp.Utilites
{
    public class DetectMood
    {
        public async Task<Tuple<string, ConfidenceScoresModel>> DetectMoodAsync(string inputText)
        {
            IList<ConfidenceScoresModel> confidenceScores = new List<ConfidenceScoresModel>();

            try
            {



                var client = new TextAnalyticsClient(new Uri($"{ConfigurationManager.AzureMoodDetection.Endpoint}"),
                                                        new AzureKeyCredential($"{ConfigurationManager.AzureMoodDetection.ApiKey}"));

                var response = client.AnalyzeSentimentAsync(inputText).GetAwaiter().GetResult();
                var sentiment = response.Value.ConfidenceScores;
                confidenceScores.Add(new ConfidenceScoresModel
                {
                    PositiveNegNet = "Positive",
                    Score = Math.Round(sentiment.Positive, 2)
                });
                confidenceScores.Add(new ConfidenceScoresModel
                {
                    PositiveNegNet = "Neutral",
                    Score = Math.Round(sentiment.Neutral, 2)
                });
                confidenceScores.Add(new ConfidenceScoresModel
                {
                    PositiveNegNet = "Negative",
                    Score = Math.Round(sentiment.Negative, 2)
                });
                var maxScore = confidenceScores.OrderByDescending(p => p.Score).FirstOrDefault();
                return Tuple.Create(response.Value.Sentiment.ToString(), maxScore); // "Positive", "Neutral", "Negative"    
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error detecting sentiment: " + ex.Message);
                return Tuple.Create($"Error: {ex.Message}", new ConfidenceScoresModel { PositiveNegNet = "Error", Score = 0 });
                // return Tuple.Create($"Error: {ex.Message}", confidenceScores);


            }
        }
        public async Task<MoodIconsModel> GetMoodAsync(string inputText,string currentMood)
        {
           // MoodIconsModel moodIcons  = new MoodIconsModel();
          //  var sentimentResult = await DetectMoodAsync(inputText);
           // var sentiment = sentimentResult.Item1;
          //  var confidenceScore = sentimentResult.Item2.Score;
          //  var mood = MapSentimentToMood(sentiment, confidenceScore);
          //  var mood = MapSentimentToMood(confidenceScore);
            MoodIconsModel moodIcon = GetColor(currentMood);
         //   moodIcon.SentimentScore = confidenceScore;
            return moodIcon;
        }
        //    public   readonly Dictionary<string, MoodIconsModel> Mood = new()
        //{
        //    { "Happy",new MoodIconsModel {BackGroudColor= Color.FromArgb("#FFE066"), ModIcon= Utilites.EmojiTags.Happy,MoodName="Happy",SentimentScore=0} },     // Sunny yellow
        //    { "Calm",new MoodIconsModel {BackGroudColor= Color.FromArgb("#A3D9A5") ,ModIcon= Utilites.EmojiTags.Calm,MoodName="Calm",SentimentScore=0}},     // Soft green
        //    { "Confused",new MoodIconsModel { BackGroudColor = Color.FromArgb("#B0C4DE"), ModIcon = Utilites.EmojiTags.Confused,MoodName="Confused",SentimentScore=0 } },  // Misty blue
        //    { "Sad", new MoodIconsModel { BackGroudColor = Color.FromArgb("#7F8FA6"), ModIcon = Utilites.EmojiTags.Sad,MoodName="Sad",SentimentScore=0 }},         // Muted gray-blue
        //    { "Angry", new MoodIconsModel { BackGroudColor = Color.FromArgb("#5D6D7E"), ModIcon = Utilites.EmojiTags.Frustrated,MoodName="Angry",SentimentScore=0 } }   // Twilight steel
        //};
            public   readonly Dictionary<string, MoodIconsModel> Mood = new()
        {
            { "Happy",new MoodIconsModel {BackGroudColor= "#FFE066", ModIcon= Utilites.EmojiTags.Happy,MoodName="Happy",SentimentScore=0} },     // Sunny yellow
            { "Calm",new MoodIconsModel {BackGroudColor= "#7BC2BC" ,ModIcon= Utilites.EmojiTags.Calm,MoodName="Calm",SentimentScore=0}},     // Soft green
            { "Confused",new MoodIconsModel { BackGroudColor = "#94B3C4", ModIcon = Utilites.EmojiTags.Confused,MoodName="Confused",SentimentScore=0 } },  // Misty blue
            { "Sad", new MoodIconsModel { BackGroudColor = "#2196F3", ModIcon = Utilites.EmojiTags.Sad,MoodName="Sad",SentimentScore=0 }},         // Muted gray-blue
            { "Angry", new MoodIconsModel { BackGroudColor = "#FF3F00", ModIcon = Utilites.EmojiTags.Frustrated,MoodName="Angry",SentimentScore=0 } } ,  // Twilight steel
            { "Neutral",new MoodIconsModel {BackGroudColor= "#D3D3D3", ModIcon= Utilites.EmojiTags.Neutral,MoodName="Neutral",SentimentScore=0} },     // Sunny yellow
            { "Energized",new MoodIconsModel { BackGroudColor = "#FFFF00", ModIcon = Utilites.EmojiTags.Energetic,MoodName="Energized",SentimentScore=0 } },  // Misty blue
            { "Reflective", new MoodIconsModel { BackGroudColor = "#C7A0CB", ModIcon = Utilites.EmojiTags.Reflective,MoodName="Reflective",SentimentScore=0 }},         // Muted gray-blue
            { "Anxious", new MoodIconsModel { BackGroudColor = "#8C92AC", ModIcon = Utilites.EmojiTags.Anxious,MoodName="Anxious",SentimentScore=0 } } ,  // Twilight steel
            { "Hopeful", new MoodIconsModel { BackGroudColor = "#9500FF", ModIcon = Utilites.EmojiTags.Hopeful,MoodName="Hopeful",SentimentScore=0 } },   // Twilight steel
            { "Joyful",new MoodIconsModel {BackGroudColor= "#FF007F", ModIcon= Utilites.EmojiTags.FaceWithTearsOfJoy,MoodName="Joyful",SentimentScore=0} },     // Sunny yellow
            { "Lonely",new MoodIconsModel {BackGroudColor= "#4B0082" ,ModIcon= Utilites.EmojiTags.Lonely,MoodName="Lonely",SentimentScore=0}},     // Soft green
            { "Grateful",new MoodIconsModel { BackGroudColor = "#FFE5B4", ModIcon = Utilites.EmojiTags.Grateful,MoodName="Grateful",SentimentScore=0 } },  // Misty blue
            { "Peaceful", new MoodIconsModel { BackGroudColor = "#00FF7F", ModIcon = Utilites.EmojiTags.Peaceful,MoodName="Peaceful",SentimentScore=0 }},         // Muted gray-blue
            { "Loved", new MoodIconsModel { BackGroudColor = "#FFDEE9", ModIcon = Utilites.EmojiTags.Loved,MoodName="Loved",SentimentScore=0 } }   // Twilight steel

        };


        public MoodIconsModel GetColor(string moodLabel)
        {
            return Mood.TryGetValue(moodLabel, out var color) ? color : null;
        }
        private string MapToMood(string sentiment, double score)
        {
            
            if (sentiment == "Positive" && score >= 0.75) return "Joyful";
            if (sentiment == "Positive") return "Content";
            if (sentiment == "Neutral") return "Reflective";
            if (sentiment == "Negative" && score <= 0.25) return "Distressed";
            return "Sad";
        }
        public string MapSentimentToMood(double sentimentScore)
        {
            if (sentimentScore >= 0.75)
                return "Happy";
            else if (sentimentScore >= 0.5)
                return "Calm";
            else if (sentimentScore >= 0.4)
                return "Confused";
            else if (sentimentScore >= 0.25)
                return "Sad";
            else
                return "Angry";
        }


    }
}


