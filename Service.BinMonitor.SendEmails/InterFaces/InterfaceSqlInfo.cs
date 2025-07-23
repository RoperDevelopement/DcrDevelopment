using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edocs.Service.BinMonitor.SendEmails.InterFaces
{
   public interface InterfaceSqlInfo
    {
        string SqlDBName
        { get; set; }
        string SqlServerName
        { get; set; }
        string SqlDBUserName
        { get; set; }
        string SqlDBPassWord
        { get; set; }
    }
  public  interface IEmailCategories
    {
        int CategoryId
        { get; set; }
        string EmailDur
        { get; set; }
        string EmailsTo
        { get; set; }
        DateTime LastTimeEmailSent
        { get; set; }
    }
    public interface IEmail
    {
        string EmailServer
        { get; set; }
        string EmailFrom
        { get; set; }
        string EmailTo
        { get; set; }
        string EmailCC
        { get; set; }
        int EmailPort
        { get; set; }

        string EmailPw
        { get; set; }
        string EmailSubject
        { get; set; }
         string TextCC
        { get; set; }
         string TextTO
        { get; set; }
    }
    public interface IEmailDaily
    {
       string EmailTo
        { get; set; }
      string EmailCC
        { get; set; }
      string EmailFrequency
        { get; set; }
      DateTime LastTimeEmailSent
        { get; set; }
    }
}
