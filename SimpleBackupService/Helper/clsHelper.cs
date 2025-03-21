using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Diagnostics;

namespace SimpleBackupService.Helper
{
    public static class clsHelper
    {
        static private string _sourceName = "CustomBackupService";
        private static string _eventLogName = "Application";
        static private string _SourceFolder 
        {
            get
            {
                
                return ConfigurationManager.AppSettings["SourceFolder"];
            }
        }
        static private string _DestinationFolder
        { 
            get
            { 
                return ConfigurationManager.AppSettings["DestinationFolder"];
            }
        }

        public static int CopyUniqueFiles()
        {
            int copiedFiles = 0;
            try
            {
                //Check if the source folder exists
                if (!Directory.Exists(_SourceFolder))
                {
                    _LogTheProcessInEventViewer("Source folder does not exist: " + _SourceFolder, EventLogEntryType.Error);
                    return -1;
                }

                // Ensure destination directory exists
                if (!Directory.Exists(_DestinationFolder))
                {
                    Directory.CreateDirectory(_DestinationFolder);
                }

                // Get all files in the source directory
                string[] files = Directory.GetFiles(_SourceFolder);

                foreach (string file in files)
                {
                    try
                    {
                        string fileName = Path.GetFileName(file);
                        string destFilePath = Path.Combine(_DestinationFolder, fileName);

                        if (!File.Exists(destFilePath))
                        {
                            File.Copy(file, destFilePath);
                            copiedFiles++;
                        }
                    }
                    catch (Exception ex)
                    {
                        _LogTheProcessInEventViewer("Error copying file: " + ex.Message, EventLogEntryType.Warning);
                    }

                }
                //_LogTheProcessInEventViewer($"Backup completed successfully. {copiedFiles} new files copied.", EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                _LogTheProcessInEventViewer(ex.Message, EventLogEntryType.Error);
            }
            return copiedFiles;
        }
        public static void LogTheProcessInEventViewer(int copiedFiles)
        {
            _LogTheProcessInEventViewer($"Backup completed successfully. {copiedFiles} new files copied at date {DateTime.Now}.", EventLogEntryType.Information);
        }

        private static void _LogTheProcessInEventViewer(string message, EventLogEntryType Icone)
        {
            //Command for create source under applications in event log (Power shell): New-EventLog -LogName Application -Source CustomBackupService
            // Create the event source if it does not exist
           // if (!EventLog.SourceExists(_sourceName))
                //System.Security.SecurityException: 'The source was not found, but some or all event logs could not be searched.  Inaccessible logs: Security, State.'
                //Solve: application doesn't have the necessary permissions to check for the existence
                //EventLog.CreateEventSource(_sourceName, _eventLogName);

            // Log an information event
            EventLog.WriteEntry(_sourceName, message, Icone);
        }
    }
}
