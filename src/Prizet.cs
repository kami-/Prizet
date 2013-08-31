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
            // Get command-line arguments, and try to to launch game.
            var cmdOptions = new CommandOptions();
            if (CommandLine.Parser.Default.ParseArguments(args, cmdOptions))
            {
                try
                {
                    var gameLauncher = new GameLauncher(cmdOptions, new YamlMapper(Environment.UserName, cmdOptions.ConfigFilePath));
                    gameLauncher.Launch();
                }
                catch (Exception e)
                {
                    Console.WriteLine("An error has occured! See details below: ");
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
