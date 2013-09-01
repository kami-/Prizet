using System;

namespace Prizet
{
    /// <summary>
    /// Log entry types.
    /// </summary>
    public enum LogEntryType { Error, Information };

    /// <summary>
    /// Represents a log entry used in the log.
    /// </summary>
    class LogEntry
    {
        private DateTime _TimwGenerated;

        /// <summary>
        /// Local time when the log entry was created.
        /// </summary>
        public DateTime TimwGenerated { get { return _TimwGenerated; } }

        private string _UserName;

        /// <summary>
        /// Username who generated the log entry.
        /// </summary>
        public string UserName { get { return _UserName; } }

        private string _Message;

        /// <summary>
        /// Message associated with the log entry.
        /// </summary>
        public string Message { get { return _Message; } }
        
        private LogEntryType _EntryType;

        /// <summary>
        /// Type of log entry.
        /// </summary>
        public LogEntryType EntryType { get { return _EntryType; } }

        /// <summary>
        /// Initializes a new instance of LogEntry with username, message and log entry type.
        /// </summary>
        /// <param name="userName">Username who generated the log entry.</param>
        /// <param name="message">Message associated with the log entry.</param>
        /// <param name="entryType">Type of log entry.</param>
        public LogEntry(string userName, string message, LogEntryType entryType)
        {
            this._TimwGenerated = DateTime.Now;
            this._UserName = userName;
            this._Message = message;
            this._EntryType = entryType;
        }
    }
}
