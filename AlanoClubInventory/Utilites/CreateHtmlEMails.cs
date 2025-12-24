using AlanoClubInventory.Models;
using iTextSharp.tool.xml.html.head;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace AlanoClubInventory.Utilites
{
    public class CreateHtmlEMails
    {
        public async Task<StringBuilder> CreateHeader(DateTime uplloadDate, string header, string imgResource)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine($"<html lang={AlanoCLubConstProp.DoubleQuotes}en{AlanoCLubConstProp.DoubleQuotes} xmlns={AlanoCLubConstProp.DoubleQuotes}http://www.w3.org/1999/xhtml{AlanoCLubConstProp.DoubleQuotes}>");
            sb.AppendLine("<head>");
            sb.AppendLine($"<meta charset={AlanoCLubConstProp.DoubleQuotes}utf-8{AlanoCLubConstProp.DoubleQuotes}/>");
            sb.AppendLine("<title>Butte Alano Club</title>");
            sb.AppendLine("<style>");
            sb.AppendLine("table {");
            sb.AppendLine("border: 1px solid black;");
            sb.AppendLine("border-collapse: collapse;width:100%; background-color: burlywood;");
            sb.AppendLine("}");
            sb.AppendLine("th, td {");
            sb.AppendLine("border: 1px solid #ccc;");
            sb.AppendLine("padding: 10px;");
            sb.AppendLine("transition: background-color 0.3s ease;");
            sb.AppendLine("}");
            sb.AppendLine("tr:hover {");
            sb.AppendLine("background-color: #f0f8ff;");
            sb.AppendLine("}");
                    sb.AppendLine("</style>");

            sb.AppendLine("</head>");
            sb.AppendLine($"<body style={AlanoCLubConstProp.DoubleQuotes}background-color:lightgray{AlanoCLubConstProp.DoubleQuotes}>");
            sb.AppendLine($"<h1 style={AlanoCLubConstProp.DoubleQuotes}text-align:center; margin-left: auto; margin-right: auto;{AlanoCLubConstProp.DoubleQuotes}>{header}</h1>");
            var resImg = ALanoClubUtilites.GetImageFromResouceFile(imgResource);
            var img = ALanoClubUtilites.BitmapImageToString(resImg);
            sb.AppendLine($"<div style ={AlanoCLubConstProp.DoubleQuotes}display:display: block; margin-left: auto; margin-right: auto; width: 100px; height: auto;{AlanoCLubConstProp.DoubleQuotes}>");
            sb.AppendLine($"<img src='data:image/png;base64,{img}' style={AlanoCLubConstProp.DoubleQuotes} display: block; margin-left: auto; margin-right: auto; width: 50px; height: auto;{AlanoCLubConstProp.DoubleQuotes}/>");
            sb.AppendLine("</div>");
           
            return sb;
            
        }
        public async Task<StringBuilder> CloseHtmlFile(StringBuilder sb)
        {
            sb.AppendLine($"</body>");
            sb.AppendLine($"</html>");
            return sb;
        }
        public async Task<StringBuilder> SendCodeHtmlFile(string userName,string code,StringBuilder sb)
        {
            sb.AppendLine($"</body>");
            sb.AppendLine($"</html>");
            return sb;
        }
        public async Task<StringBuilder> AddSendCodeHtmlFile(string userName, string code, StringBuilder sb)
        {
        
            sb.AppendLine($"<p style={AlanoCLubConstProp.DoubleQuotes}text-align:center{AlanoCLubConstProp.DoubleQuotes}> User Name: {userName}</p>");
            sb.AppendLine($"<p style={AlanoCLubConstProp.DoubleQuotes}text-align:center{AlanoCLubConstProp.DoubleQuotes}>Code: {code}</p>");
            


            return sb;
        }


        public async Task<StringBuilder> SendNewUsereHtmlFile(ALanoClubUsersModel clubUsersModel, StringBuilder sb)
        {

            sb.AppendLine($"<p style={AlanoCLubConstProp.DoubleQuotes}text-align:center{AlanoCLubConstProp.DoubleQuotes}>First Name: {clubUsersModel.UserFirstName}</p>");
            sb.AppendLine($"<p style={AlanoCLubConstProp.DoubleQuotes}text-align:center{AlanoCLubConstProp.DoubleQuotes}>Lase Name: {clubUsersModel.UserLastName}</p>");
            sb.AppendLine($"<p style={AlanoCLubConstProp.DoubleQuotes}text-align:center{AlanoCLubConstProp.DoubleQuotes}>Email Address Name: {clubUsersModel.UserEmailAddress}</p>");
            sb.AppendLine($"<p style={AlanoCLubConstProp.DoubleQuotes}text-align:center{AlanoCLubConstProp.DoubleQuotes}>User Name: {clubUsersModel.UserName}</p>");
            sb.AppendLine($"<p style={AlanoCLubConstProp.DoubleQuotes}text-align:center{AlanoCLubConstProp.DoubleQuotes}>PassWord {clubUsersModel.UserPasswordString}</p>");
            sb.AppendLine($"<p style={AlanoCLubConstProp.DoubleQuotes}text-align:center{AlanoCLubConstProp.DoubleQuotes}>Phone Number: {clubUsersModel.UserPhoneNumber}</p>");
            sb.AppendLine($"<p style={AlanoCLubConstProp.DoubleQuotes}text-align:center{AlanoCLubConstProp.DoubleQuotes}>Admin: {clubUsersModel.IsAdmin}</p>");




            return sb;

        }
        public async Task<StringBuilder> CreateSig(StringBuilder sb)
        {
            var acInfo = await Utilites.ALanoClubUtilites.GetAlnoClubInfo();
            sb.AppendLine($"<p style={AlanoCLubConstProp.DoubleQuotes}text-align:left{AlanoCLubConstProp.DoubleQuotes}>Email Generated by {LoginUserModel.LoginInstance.UserIntils}</p>");
            sb.AppendLine($"<p style={AlanoCLubConstProp.DoubleQuotes}text-align:left{AlanoCLubConstProp.DoubleQuotes}>Thank You For Your Business</p>");
            sb.AppendLine($"<p style={AlanoCLubConstProp.DoubleQuotes}text-align:left{AlanoCLubConstProp.DoubleQuotes}>{acInfo.ClubName}</p>");
            sb.AppendLine($"<p style={AlanoCLubConstProp.DoubleQuotes}text-align:left{AlanoCLubConstProp.DoubleQuotes}>{acInfo.ClubPhone}</p>");
            sb.AppendLine($"<a href={AlanoCLubConstProp.DoubleQuotes}{acInfo.FBLink}{AlanoCLubConstProp.DoubleQuotes}>{acInfo.FacebookLink}</a>");
            sb.AppendLine($"<p style={AlanoCLubConstProp.DoubleQuotes}text-align:left{AlanoCLubConstProp.DoubleQuotes}>{acInfo.ClubEmail}</p>");
       //     sb.AppendLine($"<a href={acInfo.ClubEmail}>Send Email</a>");
              return sb;
        }

    }
}
