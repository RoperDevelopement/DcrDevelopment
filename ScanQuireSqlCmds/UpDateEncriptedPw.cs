using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdocsUSA.Utilities;
using System.Security.Cryptography;

namespace ScanQuireSqlCmds
{
   public class UpDateEncriptedPw
    {
        public void DbUserName(string newPw)
        {
            Edocs_Utilities.EdocsUtilitiesInstance.PasswordKey = Properties.Settings.Default.DbKey;
            Properties.Settings.Default.DbUserName = Edocs_Utilities.EdocsUtilitiesInstance.EncryptToString(newPw, DataProtectionScope.LocalMachine);
            Properties.Settings.Default.Save();
        }
        public void DbPw(string newPw)
        {
            Edocs_Utilities.EdocsUtilitiesInstance.PasswordKey = Properties.Settings.Default.DbKey;
            Properties.Settings.Default.DbPassWord = Edocs_Utilities.EdocsUtilitiesInstance.EncryptToString(newPw, DataProtectionScope.LocalMachine);
            Properties.Settings.Default.Save();
        }

    }
}
