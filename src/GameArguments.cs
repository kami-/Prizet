using System.Collections.Generic;

namespace Prizet
{
    /// <summary>
    /// Represents an object used to launch the game.
    /// </summary>
    class GameArguments
    {
        /// <summary>
        /// The application's path. Can be found as ":app_path" in six_updater.yml.
        /// </summary>
        public string AppPath { get; set; }

        /// <summary>
        /// Command-line arguments. Can be found as ":app_params".
        /// </summary>
        public IEnumerable<string> AppParams { get; set; }

        /// <summary>
        /// Six Updater's mod path, containing the mods. Used as defualt value if mod's path is not declared. Can be found as ":app_modpath" in six_updater.yml.
        /// </summary>
        public string AppModPath { get; set; }

        /// <summary>
        /// Additional mods for the "-mod" argument. Can be found as ":mods" in six_updater.yml.
        /// </summary>
        public string Mods { get; set; }

        /// <summary>
        /// The mods of the last used preset. Can be found as ":folders" in six_updater.yml.
        /// </summary>
        public IEnumerable<Folder> Folders { get; set; }
    }
}
