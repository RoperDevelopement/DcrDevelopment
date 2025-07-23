using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using System.Web;
namespace Edocs.ITS.AppService.ApisConst
{
    public class GetSessionVariables
    {
        public const string SessionKeyUserName = "UserName";
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
        public async Task<string> GetSessionVariable(ISession session, string sessionKeyName)
        {
            string retStr = session.GetString(sessionKeyName);
            return retStr;
        }
        public async Task SetSessionOjbjectAsJson(ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        //public async Task SetSessionVariable(ISession session, string key, string value)
        //{
        //  session.SetString(key, );
        // }

        public async Task<T> GetJsonSessionObject<T>(ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

    }
    public class Cookies
    {

        private static Cookies instance = null;
        public static Cookies CookiesInstance
        {
            get
            {
                if (instance == null)
                    instance = new Cookies();
                return instance;
            }
        }
        private Cookies()
        {
        }
        public   async Task SetCookie(string cookieKey, string cookieValue,int cookieExpires, IHttpContextAccessor httpContextAccessor)
        {
            DelCookie(cookieKey, httpContextAccessor).ConfigureAwait(false).GetAwaiter().GetResult();
            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddMinutes(cookieExpires);
            options.IsEssential = true;

            httpContextAccessor.HttpContext.Response.Cookies.Append(cookieKey, cookieValue, options);

        }
        public   async Task DelCookie(string cookieKey, IHttpContextAccessor httpContextAccessor)
        {
            if (!(string.IsNullOrWhiteSpace(GetCookie(cookieKey, httpContextAccessor).ConfigureAwait(false).GetAwaiter().GetResult())))
                httpContextAccessor.HttpContext.Response.Cookies.Delete(cookieKey);
        }
        public   async Task<string> GetCookie(string cookieKey, IHttpContextAccessor httpContextAccessor)
        {
            return httpContextAccessor.HttpContext.Request.Cookies[cookieKey];
        }
    }
}
 
