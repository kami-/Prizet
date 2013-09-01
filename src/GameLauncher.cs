using System;
using System.Diagnostics;
using System.Text;
using System.Linq;

namespace Prizet
{
    /// <summary>
    /// Represents on object to launch the game with game arguments and command-line arguments.
    /// </summary>
    class GameLauncher
    {
        /// <summary>
        /// Command-line options given by the user.
        /// </summary>
        private CommandOptions cmdOptions;

        /// <summary>
        /// Mapper for getting the game arguments.
        /// </summary>
        private IGameArgumentsMapper gameArgsMapper;

        /// <summary>
        /// Logger for logging errors and usage.
        /// </summary>
        private ILogger logger;

        /// <summary>
        /// Initializes a new instance of GameLauncher with command options and game argument mapper.
        /// </summary>
        /// <param name="cmdOptions">Command-line arguments.</param>
        /// <param name="gameArgsMapper">GameArgument mapper.</param>
        /// <param name="logger">Logger for logging errors and usage.</param>
        public GameLauncher(CommandOptions cmdOptions, IGameArgumentsMapper gameArgsMapper, ILogger logger)
        {
            this.cmdOptions = cmdOptions;
            this.gameArgsMapper = gameArgsMapper;
            this.logger = logger;
        }

        /// <summary>
        /// Launches the game.
        /// </summary>
        public void Launch()
        {
            var gameArgs = GetGameArguments();

            Process game = new Process();
            game.StartInfo.FileName = gameArgs.AppPath;
            game.StartInfo.Arguments = GetLaunchArguments(gameArgs);
            game.StartInfo.Verb = "runas";
            game.Start();

            logger.AddEntry(new LogEntry(Environment.UserName, "Succesfully launched game.", LogEntryType.Information));

        }

        /// <summary>
        /// Returns the argument string to launch the game with.
        /// </summary>
        /// <returns>Argument string.</returns>
        private string GetLaunchArguments(GameArguments gameArgs)
        {
            var args = new StringBuilder();

            // Add Six Updater arguments
            foreach (var param in gameArgs.AppParams)
            {
                if (!String.IsNullOrEmpty(param) && param != "-")
                {
                    args.AppendFormat("{0} ", param);
                }
            }

            // Add -mod argument if there are any mods
            if (!String.IsNullOrEmpty(gameArgs.Mods) || gameArgs.Folders.Count() > 0)
            {

                args.Append("\"-mod=");

                // Add six updater defined mods
                if (!String.IsNullOrEmpty(gameArgs.Mods))
                {

                    args.AppendFormat("{0};", gameArgs.Mods);
                }

                // Add preset mods
                if (gameArgs.Folders.Count() > 0)
                {
                    foreach (var folder in gameArgs.Folders)
                    {
                        // If modpath is empty, then we use the default app mod path
                        var modPath = folder.Path;
                        if (String.IsNullOrEmpty(folder.Path))
                        {
                            modPath = gameArgs.AppModPath;
                        }

                        // Add each mod defined in a folder
                        foreach (var mod in folder.Mods)
                        {
                            args.AppendFormat(@"{0}\{1};", modPath, mod);
                        }
                    }
                }

                // Close the -mod argument
                args.Append("\"");
            }

            return args.ToString();
        }

        /// <summary>
        /// Get game arguments from mapper.
        /// </summary>
        /// <returns>Mapped game arguments.</returns>
        private GameArguments GetGameArguments()
        {
            try
            {
                return gameArgsMapper.GetGameArguments();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
