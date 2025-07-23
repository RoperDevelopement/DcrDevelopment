using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
using NYPMigration.Utilities;
namespace NYPMigration
{
    class MainProgram
    {
        static void Main(string[] args)
        {
            //   for(int i=0;i<10;i++)
            // NYPRecords.NYPRecordsInstance.GetNypLabReqs();
            try
            {

            
            MigrateNYPRecs migrateNYP = new MigrateNYPRecs();
            migrateNYP.GetInputArgs(args).ConfigureAwait(false).GetAwaiter().GetResult();
                Console.WriteLine("Press any key");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
