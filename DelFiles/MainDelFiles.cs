using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Diagnostics;
namespace DelFiles
{
    class MainDelFiles
    {
        static readonly string ArgDirectory = "/dir:";
        static readonly string ArgFilename = "/fext:";
        static string SearchDirectory
        { get; set; }
        static string FileExtension
        { get; set; }
        static int TotalFilesRead
        { get; set; } = 0;
        static int TotalFilesDeleted
        { get; set; } = 0;
        static int TotalErrors
        { get; set; } = 0;
        static Stopwatch RunTime
        { get; set; }
        static void Main(string[] args)
        {
            try
            {
                RunTime = new Stopwatch();
                RunTime.Start();
                GetInputArgs(args).ConfigureAwait(false).GetAwaiter().GetResult();
              

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TotalErrors++;
            }
            finally
            {
                Console.WriteLine($"Total files found {TotalFilesRead}");
                Console.WriteLine($"Total files deteted {TotalFilesDeleted}");
                Console.WriteLine($"Total errors {TotalErrors}");
                RunTime.Stop();
                Console.WriteLine($"Elapsed Time: {RunTime.Elapsed}");
                Console.WriteLine($"Press any key to exit");
            }

        }


        public static async Task GetInputArgs(string[] args)
        {
            Thread keyListener = new Thread(() =>
            {
                Console.ReadKey(true); // Wait for a key press
                Environment.Exit(0); // Exit the application
            });

            keyListener.Start();


            foreach (string inputArgs in args)
            {
                if (inputArgs.StartsWith(ArgDirectory, StringComparison.OrdinalIgnoreCase))
                {
                    SearchDirectory = inputArgs.Substring(ArgDirectory.Length);
                }
                else if (inputArgs.StartsWith(ArgFilename, StringComparison.OrdinalIgnoreCase))
                {
                    FileExtension = inputArgs.Substring(ArgFilename.Length);
                }
                else
                    throw new Exception($"Invalid arg {inputArgs}");
            }

            DelFiles().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        static async Task DelFiles()
        {
           
            if (!(SearchDirectory.Contains(":\\")))
                SearchDirectory = SearchDirectory + ":\\";
            DateTime date = DateTime.Now;
            Console.WriteLine($"Deleting files for directory {SearchDirectory} for files {FileExtension}");
            var directories = Directory.GetDirectories(SearchDirectory)
                                             .Where(d => new DirectoryInfo(d).Attributes.HasFlag(FileAttributes.Directory));

            foreach (string directory in directories)
            {
                Console.WriteLine(directory);
            }
            //   foreach (string files in Directory.GetFiles(SearchDirectory, FileExtension, SearchOption.TopDirectoryOnly))
            //  {
            //    TotalFilesRead++;
            //   FileInfo info = new FileInfo(files);
            //   if((info.CreationTime.Day != date.Day) && (info.CreationTime.Month != date.Month))
            //     {
           // Console.WriteLine($"Found file {files}");
             //   }
            //}
        }
        static async Task DeleteFile(string fileName)
        {
            Console.WriteLine($"Deleting File {fileName}");
            try
            {
                File.Delete(fileName);
                TotalFilesDeleted++;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Could not delete file {fileName} {ex.Message}");
                TotalErrors++;
            }
        }
    }
}
