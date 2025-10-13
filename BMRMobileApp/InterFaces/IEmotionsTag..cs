using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//https://www.healthline.com/health/list-of-emotions#sadness
namespace BMRMobileApp.InterFaces
{
    public interface IEmotionsTag 
    {
        string Emotion { get; set; }
        string EmotionIcon { get; set; }
        string EmotionsColor { get; set; }
    }
    public interface ISharedExperienceTags
    {
        string SharedExperienceTags { get; set; }
    }
    public interface ISharedExperience : IPSTablesIndexID, IUserID
    {

        string SharedExperiencesIcon { get; set; }
        string SharedExpericeCopingMechanisms { get; set; }

    }
    public interface ISharedExperienceID
    {
        int SharedExperienceID { get; set; }
    }

    public interface ISupportWebSites : IPSTablesIndexID
    {
        string SupportWebSites { get; set; }
        string SupportedSiteName { get; set; }
        int DeleteResource {  get; set; }
    }
    public interface IPickerChart
    {
       string Label { get; set; }
        string ValueLabel {  get; set; }
    }

}

