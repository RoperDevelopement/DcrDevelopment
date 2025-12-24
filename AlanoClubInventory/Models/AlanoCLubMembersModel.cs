using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlanoClubInventory.Interfaces;
namespace AlanoClubInventory.Models
{
    public class AlanoCLubMembersModel:IMembers
    {
        public int MemberID { get; set; }
        public string MemberFirstName { get; set; }
        public string MemberLastName { get; set; }
        public string MemberEmail { get; set; }
        public string MemberPhoneNumber { get; set; }
       public DateTime SobrietyDate { get; set; }
        //    public DateTime MembershipStartDate { get; set; }
        //    public DateTime MembershipEndDate { get; set; }
        public bool IsActiveMember { get; set; }
        public bool IsBoardMember { get; set; }
    }
}
