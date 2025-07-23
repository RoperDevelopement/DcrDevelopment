using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edocs.Interfaces
{
    interface IPSEStudentRecord
    {
    }
    public interface IUserLogin
    {
        string UserName
        { get; set; }
        string Password
        { get; set; }
    }
    interface IBSBArchives
    {
        int ArchiveID
        { get; set; }
        string ArchiveDate
        { get; set; }
        string FileUrl
        { get; set; }
        string ArchiveTitle
        { get; set; }
        string ArchiveCollection
        { get; set; }
        string ArchiveDescription
        { get; set; }
        string KeyWord
        { get; set; }
    }
}
