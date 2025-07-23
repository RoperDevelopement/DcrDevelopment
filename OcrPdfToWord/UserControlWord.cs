using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WordMS = Microsoft.Office.Interop.Word;
using Ms.Word;
namespace OcrPdfToWord
{
    public partial class UserControlWord : UserControl
    {
        WordMS._Document wordDocument; //Word.Document  class
        WordMS._Application wordApplication; //Word.Application class
        private Object saveOptions; //word save options
        public UserControlWord()
        {
            InitializeComponent();
        }
        public void MsWordDocument()
        {
            wordDocument = null;
            wordApplication = null;
         
        }
        public void OpenWordDocument(Object fileName)
        {
            try
            {
                MsWordDocument();
                if (string.IsNullOrEmpty((string)fileName))
                {
                    fileName = string.Empty;
                    throw new Exception("Error: Word file name cannot be blank");
                }

                if (wordApplication == null)
                {
                    try
                    {
                        // wordApplication = new WordMS.Application();
                        wordApplication = new WordMS.Application();
                        if (wordApplication == null)
                            throw new Exception("Could not create an instace of word");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    //wordApplication = new WordMS.ApplicationClass();


                }

                wordApplication.Visible = true;
                object docVisable = false;
                wordDocument = wordApplication.Documents.Open(ref fileName,ref WordConstants.OMSWORDFALSE,  ref WordConstants.OMSWORDFALSE, ref WordConstants.OMSWORDMISSING, ref WordConstants.OMSWORDMISSING, ref WordConstants.OMSWORDMISSING, ref WordConstants.OMSWORDMISSING, ref WordConstants.OMSWORDMISSING, ref WordConstants.OMSWORDMISSING, ref WordConstants.OMSWORDMISSING, ref WordConstants.OMSWORDMISSING, WordConstants.OMSWORDMISSING, ref WordConstants.OMSWORDMISSING, ref WordConstants.OMSWORDFALSE, ref WordConstants.OMSWORDMISSING, ref WordConstants.OMSWORDMISSING);
                wordDocument.Activate();
                InitVar();
                SetWordOptions();
                wordDocument.AutoFormatOverride = false;
            }
            catch (Exception ex)
            {
                throw new Exception("Error opening word doc " + (string)fileName + " " + ex.Message);
            }

        }
        public WordMS._Application WordApplication
        {
            get
            {

                return wordApplication;
            }
            set
            {

                wordApplication = value;
            }
        }

        private void SetWordOptions()
        {
            WordApplication.Application.Options.CheckGrammarAsYouType = false;
            WordApplication.Application.Options.CheckGrammarWithSpelling = false;
            WordApplication.Application.Options.CheckSpellingAsYouType = false;
            WordApplication.Application.Options.CreateBackup = false;
            WordApplication.Application.Options.FormatScanning = false;
            WordApplication.Application.Options.IgnoreUppercase = false;
            WordApplication.Application.Options.SaveNormalPrompt = false;
            WordApplication.Application.Options.SavePropertiesPrompt = false;
            WordApplication.Application.Options.SuggestSpellingCorrections = false;
            WordApplication.ActiveDocument.Application.DisplayAlerts = WordMS.WdAlertLevel.wdAlertsNone;
            WordApplication.Application.Options.BackgroundSave = false;
            WordApplication.Application.Options.CreateBackup = false;
            WordApplication.Application.Options.SaveInterval = 0;
            wordApplication.Application.Options.Pagination = false;
            wordApplication.Application.DisplayAutoCompleteTips = false;
            wordApplication.Application.DisplayRecentFiles = false;
            wordApplication.Application.DisplayScreenTips = false;
            wordApplication.Application.DisplayScrollBars = false;
            
                WordApplication.ScreenUpdating = false;
        }

        private void InitVar()
        {

            //FontName = wordApplication.Selection.Font.Name;
            //FontSize = wordApplication.Selection.Font.Size;
            //saveOptions = (Object)WordMS.WdSaveOptions.wdDoNotSaveChanges;
            //FontColor = wordApplication.Selection.Font.Color;
            //FontType = WordFontType.None;
            //totalBookMarks = 0;
            //ContinueFormatList = false;

        }
    }
}
