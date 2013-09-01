using System;
using System.Collections.Generic;
using System.IO;

namespace Prizet
{
    /// <summary>
    /// Represents a simple logger object used to write log entries to a file.
    /// </summary>
    class SimpleLogger : ILogger
    {
        private IList<LogEntry> _LogEntries = new List<LogEntry>();

        /// <summary>
        /// Stores the log entries to be writen to log file.
        /// </summary>
        public IList<LogEntry> LogEntries { get { return _LogEntries; } }

        /// <summary>
        /// Log file folder path.
        /// </summary>
        public string LogPath { get; set; }

        /// <summary>
        /// Log file name.
        /// </summary>
        public string LogFileName { get; set; }

        /// <summary>
        /// Initializes a new instance of SimpleLogger with the log file folder path and log file name.
        /// </summary>
        /// <param name="logPath">Log file folder path.</param>
        /// <param name="logFileName">Log file name.</param>
        public SimpleLogger(string logPath, string logFileName)
        {
            this.LogPath = logPath;
            this.LogFileName = logFileName;
        }

        public void AddEntry(LogEntry entry)
        {
            this._LogEntries.Add(entry);
        }

        public void Flush()
        {
            if (!File.Exists(this.LogPath))
            {
                Directory.CreateDirectory(this.LogPath);
            }

            using (var logFile = File.AppendText(String.Format("{0}\\{1}", this.LogPath, this.LogFileName)))
            {
                foreach (var entry in this.LogEntries)
                {
                    logFile.WriteLine(String.Format("{0}, {1}, {2}, {3}", entry.TimwGenerated.ToString("yyyy-MM-dd HH:mm:ss"), entry.UserName, entry.EntryType.ToString(), entry.Message.Replace(@"\n\r", ";")));
                }
            }
        }
    }
}
