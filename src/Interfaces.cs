using System;

namespace Prizet
{
    /// <summary>
    /// Interface for mapping data source to GameArguments.
    /// </summary>
    interface IGameArgumentsMapper
    {
        /// <summary>
        /// Returns GameArguments mapped to the data source.
        /// </summary>
        /// <returns>Mapped GameArguments</returns>
        GameArguments GetGameArguments();
    }

    /// <summary>
    /// Thrown when the data source to map is not found.
    /// </summary>
    class DataSourceNotFoundException : Exception
    {
        private string[] _CheckedFiles;
        /// <summary>
        /// Files that have been checked
        /// </summary>
        public string[] CheckedFiles { get { return _CheckedFiles; } }

        /// <summary>
        /// Initializes a new instance of DataSourceNotFoundException with custom message and checked file paths.
        /// </summary>
        /// <param name="message">Exception message.</param>
        /// <param name="checkedFiles">Array the checked config file paths.</param>
        public DataSourceNotFoundException(string message, string[] checkedFiles)
            : base(message)
        {
            _CheckedFiles = checkedFiles;
        }
    }
}
