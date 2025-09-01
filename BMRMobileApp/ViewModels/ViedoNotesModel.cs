using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMRMobileApp.ViewModels
{
    public class ViedoNotesModel
    {
        public string VideoPath { get; set; }
        public string Emotion { get; set; }
        //public string Transcription { get; set; }
      //  public bool HasTranscription => !string.IsNullOrWhiteSpace(Transcription);
        public string ViedoTaken { get; set; }
        public int ViedoID { get; set; }
        public string  ViedoName { get; set; }
    }
}
