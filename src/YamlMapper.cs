using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YamlDotNet.RepresentationModel;

namespace Prizet
{
    /// <summary>
    /// Represents an object to map YAML config file to game arguments.
    /// </summary>
    class YamlMapper : IGameArgumentsMapper
    {
        // default and fallback paths for config file
        private const string defaultConfigFilePath = @"C:\users\{username}\appdata\roaming\Six-Updater\six-updater.yml";
        private const string fallBackConfigFilePath = @"C:\users\{username}\appdata\local\temp\Six-Updater\six-updater.yml";

        // default and fallback paths for config file on Win XP machines
        private const string XPDefaultConfigFilePath = @"C:\Documents and Settings\{username}\application data\Six-Updater\six-updater.yml";
        private const string XPFallBackConfigFilePath = @"C:\Documents and Settings\{username}\local settings\temp\Six-Updater\six-updater.yml";

        // config file path
        private string configFilePath = String.Empty;
        // current user's name
        private string userName;

        // The YAML stream used for mapping
        private YamlStream yaml;

        /// <summary>
        /// Initializes a new instance of YamlMapper with the windows username.
        /// </summary>
        /// <param name="userName">The username to check for in the Users folder.</param>
        public YamlMapper(string userName) 
        {
            this.userName = userName;
        }

        /// <summary>
        /// Initializes a new instance of YamlMapper with the windows username and custom config file path to use.
        /// </summary>
        /// <param name="userName">The username to check for in the Users folder.</param>
        /// <param name="configFilePath">Custom confi file path to use.</param>
        public YamlMapper(string userName, string configFilePath)
        {
            this.userName = userName;
            this.configFilePath = configFilePath;
        }

        /// <summary>
        /// Returns the game arguemnts mapped to six-updater-yml file.
        /// </summary>
        /// <returns>Mapped game arguments.</returns>
        public GameArguments GetGameArguments()
        {
            GameArguments gameArgs = null;
            try
            {
                // Open file and initialize YamlStream.
                using (FileStream fs = new FileStream(FindConfigPath(), FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader input = new StreamReader(fs))
                    {
                        yaml = new YamlStream();
                        yaml.Load(input);

                        var rootChildren = (yaml.Documents[0].RootNode as YamlMappingNode).Children;

                        // Read game arguemnts from YAML file.
                        gameArgs = new GameArguments();
                        gameArgs.AppParams   = (GetChildNodeByKey(rootChildren, ":app_params") as YamlSequenceNode).Select(par => (par as YamlScalarNode).Value).ToList();
                        gameArgs.AppPath     = (GetChildNodeByKey(rootChildren, ":app_path") as YamlScalarNode).Value;
                        gameArgs.AppExe      = (GetChildNodeByKey(rootChildren, ":app_exe") as YamlScalarNode).Value;                     
                        gameArgs.Mods        = (GetChildNodeByKey(rootChildren, ":mods") as YamlScalarNode).Value;

                        // If no mod forlder is given, use the game's folder
                        var appModPath = GetChildNodeByKey(rootChildren, ":app_modpath") as YamlScalarNode;
                        if (appModPath == null)
                        {
                            gameArgs.AppModPath = gameArgs.AppPath;
                        }
                        else
                        {
                            gameArgs.AppModPath = appModPath.Value;
                        }

                        var yamlFolders = (GetChildNodeByKey(rootChildren, ":folders") as YamlSequenceNode).Children;
                        gameArgs.Folders = (from fold in yamlFolders
                                               let path = (GetChildNodeByKey((fold as YamlMappingNode).Children, ":path") as YamlScalarNode).Value
                                               let mods = (GetChildNodeByKey((fold as YamlMappingNode).Children, ":mods") as YamlSequenceNode).Select(m => (m as YamlScalarNode).Value).ToList()
                                               select new Folder { Path = path, Mods = mods }).ToList();
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return gameArgs;
        }

        /// <summary>
        /// Returns the path to the config file, if it exists.
        /// </summary>
        /// <returns>Config file path.</returns>
        private string FindConfigPath()
        {
            // Paths to check for the six-updater.yml.
            string[] filePaths = { configFilePath, defaultConfigFilePath, fallBackConfigFilePath, XPDefaultConfigFilePath, XPFallBackConfigFilePath };

            // Replace {username} in the default paths with the current username.
            var realFilePaths = filePaths.Where(p => !String.IsNullOrEmpty(p)).Select(p => p.Replace("{username}", userName));

            // Check if six-updater.yml exists in the given paths.
            var goodFilePath = realFilePaths.Where(p => File.Exists(p)).FirstOrDefault();

            // If not, throw exception with checked paths.
            if (String.IsNullOrEmpty(goodFilePath)) 
                throw new DataSourceNotFoundException("File six-updater.yml was not found." ,realFilePaths.ToArray());

            return goodFilePath;
        }

        /// <summary>
        /// Returns the YamlNode of a node's children with the given key.
        /// </summary>
        /// <param name="children">YamlMappingNode.Children to search in.</param>
        /// <param name="key">The key value to search for.</param>
        /// <returns>YamlNode with the given key.</returns>
        private YamlNode GetChildNodeByKey(IDictionary<YamlNode, YamlNode> children, string key)
        {
            return children.Where(yn => (yn.Key as YamlScalarNode).Value == key).FirstOrDefault().Value;
        }
    }
}
