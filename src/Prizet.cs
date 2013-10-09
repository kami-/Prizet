using System;

namespace Prizet
{
    class Loader
    {
        static void Main(string[] args)
        {
            //Load dlls
            EmbeddedAssemblyLoader eal = new EmbeddedAssemblyLoader();

            Prizet prizet = new Prizet(args);

            return;
        }
    }

    class Prizet
    {
        public Prizet(string[] args)
        {
            // logger
            ILogger logger = new SimpleLogger(String.Format("{0}\\{1}", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Prizet"), "log.txt");
            try
            {
                // Get command-line arguments, and try to to launch game.
                var cmdOptions = new CommandOptions();
                if (CommandLine.Parser.Default.ParseArguments(args, cmdOptions))
                {
                    var gameLauncher = new GameLauncher(cmdOptions, new YamlMapper(Environment.UserName, cmdOptions.ConfigFilePath), logger);
                    gameLauncher.Launch();
                }
            }
            catch (Exception e)
            {
                logger.AddEntry(new LogEntry(Environment.UserName, String.Format("Message: {0}; Trace: {1};", e.Message, e.StackTrace, e.TargetSite.Name), LogEntryType.Error));
            }

            logger.Flush();
        }
    }
}
