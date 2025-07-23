using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Edocs.Logger
{
    public enum LoggingErrorType
    {
        Info,
        Warning,
        Error
        
    }
    public class Logger : IDisposable
    {
        #region members
        public static Logger LogInstance = null; //used to create a new instance of the logging class
        private StreamWriter logFileHnd = null; //the textwriter handle for the log file
        private string logFileName = string.Empty; //the name of the log file
                                                   //  private string logFolder = string.Empty; //the name of the log file
        #endregion
        #region properties

        /// <summary>
        /// Get - returns the name of the log file
        /// Set - sets the name of the log file
        /// </summary>
        public string LogFileName
        {
            get
            {
                return logFileName;
            }
            set
            {
                logFileName = value;
            }
        }

        ///// <summary>
        ///// Get - returns the name of the log folder
        ///// Set - sets the name of the log folder
        ///// </summary>

        //public string LogFolder
        //{
        //    get
        //    {
        //        return logFolder;
        //    }
        //    set
        //    {
        //        logFolder = value;
        //    }
        //}

        #endregion
        #region Methods

        /// <summary>
        /// Prtected constructor for the logging class
        /// </summary>
        /// 
        protected Logger()
        {

        }

        /// <summary>
        /// The access point for the singleton class
        /// </summary>
        /// 
        static Logger()
        {
            if (LogInstance == null)
                LogInstance = new Logger();
        }

        /// <summary>
        /// Writes the information to the log file and the console window.
        /// </summary>
        /// <param name="logInfo">The information to write out</param>
        public void WriteLoggingLogFile(string logInfo, bool writeConsole, LoggingErrorType errorType)
        {
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(LogFileName)))
                    Directory.CreateDirectory(Path.GetDirectoryName(LogFileName));
                if (writeConsole)
                    WriteConsole(logInfo, errorType);


                if (logFileHnd == null)
                    OpenLoggingFile();
                if (logFileHnd != null)
                {
                    if (string.IsNullOrEmpty(logInfo))
                        logFileHnd.WriteLine();
                    else
                        logFileHnd.WriteLine($"{errorType.ToString()}:Timestamp:{DateTime.Now.ToString()}:{logInfo}");
                }

            }
            catch (Exception ex)
            {
                WriteConsole($"Could not write to log file: {ex.ToString()}",LoggingErrorType.Error);
                System.Environment.Exit(1);

            }

        }
        private void WriteConsole(string logInfo, LoggingErrorType errorType)
        {
            try
            {
                if (string.IsNullOrEmpty(logInfo))
                    Console.WriteLine();
                else
                    Console.WriteLine($"{logInfo}:{errorType}");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not write to log file: " + ex.ToString());
                System.Environment.Exit(1);

            }

        }

        /// <summary>
        /// Gets the name of the log file and creates the log file directory if the log file directory does not exists.
        /// </summary>
        /// <returns "string">The name of the log file</returns>
        private string GetLogFileDirectory()
        {
            string logDir = string.Empty;
            try
            {

                //   if (string.IsNullOrWhiteSpace(logFileName))
                // logFileName = $"EdocsLogging_{DateTime.Now.ToString("HH")}_{DateTime.Now.ToString("MM")}_{DateTime.Now.Second}_{DateTime.Now.Year}.log";
                logDir = $"EdocsLogging_{DateTime.Now.ToString("MM_dd_yyyy_hh_mm_ss_tt")}.log";
                logDir = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\EdocsUsaSendEMailLog\\{logDir}";

                if (!Directory.Exists(Path.GetDirectoryName(logDir)))
                    Directory.CreateDirectory(Path.GetDirectoryName(logDir));
            }
            catch (Exception ex)
            {
                WriteConsole($"Could not create log directory {logDir} {ex.Message}",LoggingErrorType.Error);
                logDir = string.Empty;
            }
            return logDir;
        }

        /// <summary>
        /// Close the log file
        /// </summary>
        public void CloseLogFile()
        {
            try
            {
                if (logFileHnd != null)
                {
                    logFileHnd.Flush();
                    logFileHnd.Close();
                    logFileHnd.Dispose();
                    LogFileName = string.Empty;
                }
            }
            finally
            {
                logFileHnd = null;
            }
        }

        /// <summary>
        /// Flush the file stream and close it
        /// </summary>
        public void Dispose()
        {
            try
            {
                if (logFileHnd != null)
                {
                    logFileHnd.Flush();
                    logFileHnd.Close();
                    logFileHnd.Dispose();
                    
                }
            }
            catch
            { }
            finally
            {
                logFileHnd = null;
                LogFileName = string.Empty;
            }
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Gets the name of the log file and then opens the log file.
        /// </summary>
        private void OpenLoggingFile()
        {
            try
            {
                // logFileName = GetLogFileDirectory();
                if (string.IsNullOrEmpty(logFileName))
                    logFileName = GetLogFileDirectory();

                if (!(string.IsNullOrEmpty(logFileName)))
                {
                    if (!Directory.Exists(Path.GetDirectoryName(logFileName)))
                        Directory.CreateDirectory(Path.GetDirectoryName(logFileName));
                }
                if ((logFileHnd == null))
                {
                    if(File.Exists(logFileName))
                        logFileHnd = new StreamWriter(logFileName, true);
                    else
                        logFileHnd = new StreamWriter(logFileName, false);
                }
            }
            catch (Exception ex)
            {
                WriteConsole($"Error opeing log file {ex.Message}",LoggingErrorType.Error);
                System.Environment.Exit(1);
            }
        }

    }
    #endregion Methods

}

