using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMRMobileApp.InterFaces;
using SQLite;
namespace BMRMobileApp.Models
{
    public class PSUserMood : IPSUserMood
    {
        [AutoIncrement, PrimaryKey, Unique]
        public int ID { get; set; }
        [NotNull]
        public double SentimentScore { get; set; }
        [NotNull]
        public string TimeMood { get; set; }
        [NotNull]
        public string Mood { get; set; }
        [NotNull]
        public string BackgroundColor { get; set; }
        [NotNull]
        public string MoodTag { get; set; }



    }
}
