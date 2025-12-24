using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlanoClubInventory.Utilites;


namespace AlanoClubInventory.Models
{
   public class ReadJsonFile : IDisposable

    {
        public async Task<T> GetJsonData<T>(string model) where T : new()
        {

            string path = Utilites.ALanoClubUtilites.AlanoClubJsFile;
            if(!(string.IsNullOrWhiteSpace(path)))
            {
                var jsonString = File.ReadAllText(path);
            var jSonObj = JObject.Parse(jsonString);
            var str = jSonObj[model]?.ToString();
            var retJson= JsonConvert.DeserializeObject<T>(str);

            return retJson;
            }
            return default(T);
        }
        public void Dispose()
        {


            // Suppress finalization to avoid calling the finalizer if already disposed
            GC.SuppressFinalize(this);
        }


    }
}
