using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlanoClubInventory.Utilites
{
    public class AlanoCLubConstProp
    {
        public const string Dues = "Dues";
        public const string Donations = "Donations";
        public const string Rent = "Rent";
        public const string CoffeeClub = "Club";
        public const string NA = "N/A";
        //public const string RegXInv = @"\b(Club|Rent|Group|Events|Donations|Clubs|Groups)\b";
        public const string RegXInv = @"(Club|Rent|Group|Events|Donations|Clubs|Groups|Family|Single|membership)";
        public const string CarrageReturnLineFeed = "\r\n";
        public const string VailEmailAddress = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        public const string VailPhoneNumber = @"^\(?([0-9]{3})\)?[-.●]?([0-9]{3})[-.●]?([0-9]{4})$";
        public const string DoubleQuotes = "\"";
        public const string AlanoCLubAdmin = "admin";
        public const string AlanoCLubAdminPW = "0317Aa@";
        public const string PasswordPattern = @"[!@#$%^&*(),.?""{}|<>]";
        public const string ACTempSaveFolder = @"ACInventoryWF";
        public const string CRLF = "\r\n";
        public const string PDFBlankLine = "\n";
        public const string AlanoClubDBName = "AlanoClub";
        public const string AlanoClubLogDBName = "AlanoClub_log";
        public const string CreateNewDBScript = "CreateNewDatabase.sql"; 
        public const string ACDataBaseBackupDbName = "AlanoClubBackup";
        public const string ACDataBaseBackupFolderName = "BackDatabases";
        public const string ACDataTaleNames = "[AlanoClub]";
        public const string CreateDbScrip = @"DataBaseTableScripts\CreateDataBase.sql";
        public const string GO = "GO";
        public const string DBStorageFormat = "'AlanoClub'";
        public const string CreateTablesScript = @"DataBaseTableScripts\CreateTables.sql";
        public const string CreateStoredProcsScript = "CreateStoreProd.sql";
        public const string CreateUsersScript = @"DataBaseTableScripts\CreateUsers.sql";
        public const string CreateAlanoClubUser = "[AlanoClubUser]";
        public const string SqlConnBackupDataBase = "{SqlConnBackupDataBase}";
        public const string CreateProc = "CREATE";
        public const int MilliSeconds = 1000;
        public const string SqlQueryStoredProcedures = @"SELECT p.name, m.definition FROM sys.procedures p INNER JOIN sys.sql_modules m ON p.object_id = m.object_id ORDER BY p.name desc";
    }


}


 
