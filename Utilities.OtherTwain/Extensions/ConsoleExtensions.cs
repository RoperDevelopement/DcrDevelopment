/*
 * User: Sam Brinly
 * Date: 7/23/2014
 */
using System;

namespace EdocsUSA.Utilities.Extensions
{
	/// <summary>
	/// Description of ConsoleExtensions.
	/// </summary>
	public static class ConsoleExtensions
	{
		public static string ReadPassword()
		{
			 string pass = string.Empty;
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);

                // Backspace Should Not Work
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    pass += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                    {
                        pass = pass.Substring(0, (pass.Length - 1));
                        Console.Write("\b \b");
                    }
                }
            }
            // Stops Receving Keys Once Enter is Pressed
            while (key.Key != ConsoleKey.Enter);
            Console.WriteLine(string.Empty);
            return pass;
		}
	}
}
