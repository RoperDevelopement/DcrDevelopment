using BMRMobileApp.InterFaces;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BMRMobileApp.Models
{
    public class PSCopingMechanismsl : ISharedExperience, ISharedExperienceID
    {
        [AutoIncrement, PrimaryKey, Unique]
        public int ID { get; set; }
        [NotNull]
        public int SharedExperienceID { get; set; }
        [NotNull]
        public int PSUserID { get; set; }
        [NotNull]
        public string SharedExperiencesIcon { get; set; }
        [NotNull]
        public string SharedExpericeCopingMechanisms
        {
            get; set;
        }
    }
}
