using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForchtFormFeedUtility
{
    class ProgramPaths
    {
        public string originalFullPath { get; private set; }
        public string originalFileName { get; private set; }
        public string assemblyDirectory { get; private set; }
        public DirectoryInfo workingDirectory { get; private set; }

        /// <summary>
        /// Uses the argument passed in to build all the pathing references needed
        /// for the program.
        /// </summary>
        /// <param name="arg"></param>
        public ProgramPaths(string arg)
        {
            originalFullPath = arg;
            originalFileName = Path.GetFileName(originalFullPath);
            assemblyDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetAssembly(typeof(Program)).Location);
            workingDirectory = new DirectoryInfo(assemblyDirectory + @"\Working\");
        }
    }
}
