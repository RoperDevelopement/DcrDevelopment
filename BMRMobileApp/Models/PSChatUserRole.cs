using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMRMobileApp.InterFaces;
using SQLite;
namespace BMRMobileApp.Models
{
    public class PSChatUserRole: IPSChatUserRoles
    {
         [SQLite.PrimaryKey, SQLite.AutoIncrement, SQLite.Unique]
        public int ID { get; set; }
        [SQLite.NotNull]
       public string PSUserRole { get; set; } // e.g., Admin, Member, Moderator
        [SQLite.NotNull]
        public string PSRoleDescription { get; set; } // Description of the role
       
        public string PSRolePermissions { get; set; } // Permissions associated with the role (e.g., Read, Write, Delete)
    }
}
 
