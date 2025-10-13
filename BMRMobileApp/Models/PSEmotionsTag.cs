using BMRMobileApp.InterFaces;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BMRMobileApp.Models
{
   public class PSEmotionsTag:IEmotionsTag, IPSTablesIndexID
    {
        [AutoIncrement, PrimaryKey, Unique]
        public int ID { get; set; }
        [NotNull]
        public string Emotion { get; set; }
        [NotNull]
        public string EmotionIcon { get; set; }
        [NotNull]
        public string EmotionsColor { get; set; }
    }
}
