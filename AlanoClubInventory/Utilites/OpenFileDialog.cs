using AlanoClubInventory.Models;

using PdfSharp;

using PdfSharp.Pdf;

using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;

using RtfPipe;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;
using System.Windows.Xps.Serialization;
namespace AlanoClubInventory.Utilites
{
    //       Filter = "Image Files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg"save
    //        Filter = "Rich Text Format (*.rtf)|*.rtf|XAML (*.xaml)|*.xaml|Text Files (*.txt)|*.txt"
    public class OpenFileDialog
    {
        private Microsoft.Win32.OpenFileDialog FileDialog { get; set; }
        private Microsoft.Win32.SaveFileDialog SaveDialog { get; set; }

        public async Task<string> OpenFile(string path, string filter)
        {
            if (FileDialog == null)
            {
                FileDialog = new Microsoft.Win32.OpenFileDialog();
            }
            if (!(string.IsNullOrEmpty(path)))
            {
                if (Directory.Exists(path))
                {
                    FileDialog.InitialDirectory = path;
                }
            }
            FileDialog.Filter = filter;
            if (FileDialog.ShowDialog() == true)
            {
                return FileDialog.FileName;
            }
            return string.Empty;
        }

        public async Task<RichTextBox> OpenRichTextFile(string path, string filter)
        {
            var rTxt = new RichTextBox();
            if (FileDialog == null)
            {
                FileDialog = new Microsoft.Win32.OpenFileDialog();
            }
            FileDialog.Filter = filter;
            if (FileDialog.ShowDialog() == true)
            {
                using (FileStream fs = new FileStream(FileDialog.FileName, FileMode.Open))
                {
                    TextRange range = new TextRange(rTxt.Document.ContentStart,
                                                    rTxt.Document.ContentEnd);
                    string ext = Path.GetExtension(FileDialog.FileName);
                    if (ext == ".rtf")
                        range.Load(fs, DataFormats.Rtf);
                    else if (ext == ".xaml")
                        range.Load(fs, DataFormats.Xaml);
                    else
                        range.Load(fs, DataFormats.Text);

                }

            }
            return rTxt;
        }

        public async Task SaveFileRichText(RichTextBox textBox, string filter)
        {
            if (SaveDialog == null)
            {
                SaveDialog = new Microsoft.Win32.SaveFileDialog();
            }
            SaveDialog.Filter = filter;
            if (SaveDialog.ShowDialog() == true)
            {
                string ext = System.IO.Path.GetExtension(SaveDialog.FileName).ToLower();
                if (ext == null)
                {
                    ext = ".txt";
                    SaveDialog.FileName += ext;
                }
                else if (ext == ".pdf")
                {

                    await SaveFilePDFlFile(textBox, SaveDialog.FileName);
                }
                else if (ext != ".rtf" && ext != ".xaml" && ext != ".txt")
                {
                    FlowDocument document = textBox.Document;
                    await SaveFileHtmlFile(textBox, SaveDialog.FileName);
                }

                else
                {
                    using (FileStream fs = new FileStream(SaveDialog.FileName, FileMode.Create))
                    {
                        TextRange range = new TextRange(textBox.Document.ContentStart,
                                                        textBox.Document.ContentEnd);


                        if (ext == ".rtf")
                            range.Save(fs, DataFormats.Rtf);
                        else if (ext == ".xaml")
                            range.Save(fs, DataFormats.Xaml);
                        else
                            range.Save(fs, DataFormats.Text);

                    }
                }

            }


        }
        public async Task SaveFileHtmlFile(RichTextBox document, string htmlFileName)
        {
            TextRange range = new TextRange(document.Document.ContentStart, document.Document.ContentEnd);
            string output = string.Empty;
            string rtf = string.Empty;
            using (MemoryStream ms = new MemoryStream())
            {
                range.Save(ms, DataFormats.Rtf);
                ms.Position = 0;
                rtf = new StreamReader(ms).ReadToEnd();
            }

            // Convert RTF → HTML using RtfPipe (NuGet)
            if (string.IsNullOrWhiteSpace(rtf))
                rtf = "<html><body><p>(No content)</p></body></html>";

            output = Rtf.ToHtml(rtf);
            File.WriteAllText(htmlFileName, output);
        }

        public async Task<string> RichTextToHtmlFile(RichTextBox document)
        {
            TextRange range = new TextRange(document.Document.ContentStart, document.Document.ContentEnd);
            string output = string.Empty;
            string rtf = string.Empty;
            using (MemoryStream ms = new MemoryStream())
            {
                range.Save(ms, DataFormats.Rtf);
                ms.Position = 0;
                rtf = new StreamReader(ms).ReadToEnd();
            }

            // Convert RTF → HTML using RtfPipe (NuGet)
            if (string.IsNullOrWhiteSpace(rtf))
                rtf = "<html><body><p>(No content)</p></body></html>";

            return Rtf.ToHtml(rtf);

        }

        public async Task SaveFilePDFlFile(RichTextBox docRT, string pdfFileName)
        {
            string htmlContent = await RichTextToHtmlFile(docRT);
            htmlContent = "<html><head><meta charset='UTF-8'/></head><body>" + htmlContent + "</body></html>";
            htmlContent = htmlContent.Replace("<br>", "");
            using (FileStream fs = new FileStream(pdfFileName, FileMode.Create))
            {
                iTextSharp.text.Document doc = new iTextSharp.text.Document();
                doc.SetPageSize(iTextSharp.text.PageSize.A4);
                doc.AddAuthor("Butte Alano Club");
                doc.AddCreator("Alano Club Inventory System");
                doc.AddTitle("Exported PDF Document");
                doc.AddSubject("Exported from RichTextBox");
                doc.AddKeywords("Alano, Inventory, PDF, Export");
                doc.AddCreationDate();
                PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                doc.Open();
                using (var sr = new StringReader(htmlContent))
                {
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, sr);
                }
                doc.Close();
            }


        }
        public async Task<string> SaveFile(string filter)
        {
            if (SaveDialog == null)
            {
                SaveDialog = new Microsoft.Win32.SaveFileDialog();
            }
            SaveDialog.Filter = filter;
            if (SaveDialog.ShowDialog() == true)
            {
               return SaveDialog.FileName;
            }
            return string.Empty;


        }
    }
    }