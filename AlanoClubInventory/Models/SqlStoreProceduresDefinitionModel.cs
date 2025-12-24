using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlanoClubInventory.Interfaces;
namespace AlanoClubInventory.Models
{
    public class SqlStoreProceduresDefinitionModel:IStoredProcedures
    {
       public string StoredProcedureName { get; set; }
      public  string StoredProcedureDefinition { get; set; }
    }
}
