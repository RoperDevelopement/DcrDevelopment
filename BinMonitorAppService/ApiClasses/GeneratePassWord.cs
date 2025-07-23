using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BinMonitorAppService.ApiClasses
{
    public class GeneratePassWord
    {
        private readonly int AsciiodeNonDigitStart33 = 33;
        //private readonly int AsciiCodeNonDigitEnd33 = 47;
        //private readonly int AsciiCodeDigitStart = 48;
        //private readonly int AsciiCodeDigitEnd = 57;
        //private readonly int AsciiCodeNonDigitStart59 = 58;
        //private readonly int AsciiCodeNonDigitEnd59 = 64;
        //private readonly int AsciiCodeUpperCaseStart = 65;
        //private readonly int AsciiCodeUpperCaseEnd = 90;
        //private readonly int AsciiCodeNonDigitStart91 = 91;
        //private readonly int AsciiCodeNonDigitEnd91 = 96;
        //private readonly int AsciiCodeLowerCaseStart = 97;
        //private readonly int AsciiCodeLowerCaseEnd = 122;
        //private readonly int AsciiCodeNonDigitStart123 = 123;
        private readonly int AsciiCodeNonDigitEnd122 = 126;
        private Random random = new Random();
        //   private readonly string[] AsciiCodes = new string[] { "33-47", "48-57", "58-64", "65-90","91-96","97-122","123-126" };
        private readonly string[] AsciiCodes = new string[] { "48-57", "65-90", "97-122"};
        public static GeneratePassWord GeneratePassWordInstance = null;
        protected GeneratePassWord()
        { }
        static GeneratePassWord()
        {

            if (GeneratePassWordInstance == null)
            {
                GeneratePassWordInstance = new GeneratePassWord();
            }
         
        }
        public string GeneratePw(int passWordLength)
        {
            string retPW = string.Empty;
            int loop = 0;
            while(retPW.Length < passWordLength)
            {
                retPW += GetNewPassword();
                loop++;
            }
            return retPW + "@";
        }
        private string GetNewPassword()
        {
            string retPW = string.Empty;
            
            foreach(string asciiCode in AsciiCodes)
            {
                
                string[] codesAscii = asciiCode.Split('-');
                char assciChar = GenerateRamdomChar(int.Parse(codesAscii[0]), int.Parse(codesAscii[1]));
                retPW += assciChar.ToString();
                
            }
            return retPW;
        }
        private char GenerateRamdomChar(int startChar,int endChar)
        {
            char retChar = '0';
          
            int randomChar = random.Next(startChar, endChar);
            if((randomChar >= AsciiodeNonDigitStart33) && (randomChar <= AsciiCodeNonDigitEnd122))
                     retChar = Convert.ToChar(randomChar);
            return retChar;
        }
    }
}
