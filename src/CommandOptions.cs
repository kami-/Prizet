using CommandLine;
using CommandLine.Text;
using System;

namespace Prizet
{
    /// <summary>
    /// Represents an object used to store command-line arguments.
    /// </summary>
    class CommandOptions
    {
        /// <summary>
        /// Stores the user defined config file path.
        /// </summary>
        [Option('f', "file", Required = false, HelpText = "Custom config file path to be used.")]
        public string ConfigFilePath { get; set; }

        [Option('c', "custom", Required = false, HelpText = "Custom arguments to launch the game with. Without the \"-\", example skipintro showscripterrors. ")]
        public string CustomArguments { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
