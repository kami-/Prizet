using System;
using System.Collections.Generic;

namespace Prizet
{ 
    /// <summary>
    /// Represents an object storing the path and modfolder of mod. Can be found as ":folder" in six_updater.yml.
    /// </summary>
    class Folder
    {
        /// <summary>
        /// Path to the modfolder. Found as ":path".
        /// </summary>
        public string Path { get; set; }
        
        /// <summary>
        /// Actual modfolder. Found as ":mods".
        /// </summary>
        public IEnumerable<string> Mods { get; set; }
    }
}
