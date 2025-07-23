using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft;
using Newtonsoft.Json;
namespace EDocs.Nyp.LabReqs.AppServices.LabReqsConstants
{
    public class GetSessionVariables
    {
        public const string SessionKeyCwid = "Cwid";
        public const string SessionKeyUserProfile = "UserProfile";
        private static GetSessionVariables instance = null;
        public static GetSessionVariables SessionVarInstance
        {
            get
            {
                if (instance == null)
                     instance = new GetSessionVariables();
                return instance;
            }
        }
        private GetSessionVariables()
        {
        }
#pragma warning disable 1998
        public async  Task<string> GetSessionVariable(ISession session, string sessionKeyName)
        {
            string retStr = session.GetString(sessionKeyName);
            return retStr;
        }
        public async Task SetSessionOjbjectAsJson(ISession session,string key,object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        //public async Task SetSessionVariable(ISession session, string key, string value)
        //{
          //  session.SetString(key, );
       // }

        public async Task<T>  GetJsonSessionObject<T>(ISession session,string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
       
    }
}
