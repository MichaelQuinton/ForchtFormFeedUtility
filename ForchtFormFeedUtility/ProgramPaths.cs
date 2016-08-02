namespace ForchtFormFeedUtility
{
    using System.IO;

    internal class ProgramPaths
    {
        public string OriginalFullPath { get; }
        public string OriginalFileName { get; private set; }
        private string AssemblyDirectory { get; }
        public DirectoryInfo WorkingDirectory { get; private set; }

        /// <summary>
        /// Uses the argument passed in to build all the pathing references needed
        /// for the program.
        /// </summary>
        /// <param name="arg"></param>
        public ProgramPaths(string arg)
        {
            OriginalFullPath = arg;
            OriginalFileName = Path.GetFileName(OriginalFullPath);
            AssemblyDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetAssembly(typeof(Program)).Location);
            WorkingDirectory = new DirectoryInfo(AssemblyDirectory + @"\Working\");
        }
    }
}