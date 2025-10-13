using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlanoClubInventory.Interfaces;
namespace AlanoClubInventory.Models
{
    public class CategoryModelID : IID,ICategories
    {
       public int ID { get; set; }
       public string CategoryName { get; set; }
       public string MemberPrice { get; set; }
       public string NonMemberPrice { get; set; }
    }
}
