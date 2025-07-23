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
    public class EdocsITSHelpers
    {
        public const string QUOTE = "\"";
        private static System.Diagnostics.Stopwatch watch = null;
        public static void StartStopWatch()
        {
            watch = new System.Diagnostics.Stopwatch();
            watch.Start();
        }
        public static string StopStopWatch()
        {
            try
            {
                watch.Stop();
                return ($"{watch.ElapsedMilliseconds} ms");
            }
            catch (Exception ex)
            {
                return $"Error getting end time {ex.Message}";
            }
        }
        public static string StopStopWatchTotalTime()
        {
            try
            {
                watch.Stop();
                return ($"{watch.Elapsed}");
            }
            catch (Exception ex)
            {
                return $"Error getting end time {ex.Message}";
            }
        }

        public static string RepStr(string instr, string repStr, string newstr)
        {
            if (!(string.IsNullOrWhiteSpace(instr)))
                return (instr.Replace(repStr, newstr).Trim());
            return string.Empty;
        }

        public static string[] SplitStr(string inStr, char spliOn)
        {
            return inStr.Split(spliOn);
        }
        public static bool UserAuth(ISession session)
        {
            string auth = GetSessionVariables.SessionVarInstance.GetJsonSessionObject<string>(session, EdocsITSConstants.IsUserAuth).ConfigureAwait(false).GetAwaiter().GetResult();
            if (bool.TryParse(auth, out bool result))
                return result;
            return false;
        }
        public static DateTime StrToDate(string inStrDate)
        {
            if (DateTime.TryParse(inStrDate, out DateTime results))
                return results;
            return DateTime.Now;
        }
        public static async Task<int> GenerateRandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
        public static bool CheckAuth(ISession session)
        {
            if (!(EdocsITSHelpers.UserAuth(session)))
                return false;
            return true;
        }
    }
}
