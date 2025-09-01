using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BMRMobileApp.Models;
namespace BMRMobileApp.Utilites
{
    public class StateService
    {
        private const string ApiUrl = "https://gist.githubusercontent.com/mshafrir/2646763/raw/states_titlecase.json";
        private List<StateInfoModel> cachedStates;

        public async Task<List<StateInfoModel>> GetStatesAsync()
        {
            if (cachedStates != null)
                return cachedStates;

            using var client = new HttpClient();
            var json = await client.GetStringAsync(ApiUrl);

            var rawStates = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(json);
            cachedStates = rawStates.Select(s => new StateInfoModel
            {
                Name = s["name"],
                Abbreviation = s["abbreviation"]
            }).ToList();

            return cachedStates;
        }
    }
}
