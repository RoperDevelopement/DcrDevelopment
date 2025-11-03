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
       public const string Donations="Donations";
       public const string Rent = "Rent";
       public const string CoffeeClub = "Club";
        //public const string RegXInv = @"\b(Club|Rent|Group|Events|Donations|Clubs|Groups)\b";
        public const string RegXInv = @"(Club|Rent|Group|Events|Donations|Clubs|Groups|Family|Single|membership)";
        public const string CarrageReturnLineFeed = "\r\n";
    }
}
