namespace ForchtFormFeedUtility
{
    using System.IO;

    public static class DirectoryInfoExtentionMethods
    {
        /// <summary>
        /// Add a method to DirectoryInfo that deletes all files and folders in the directory
        /// </summary>
        /// <param name="di"></param>
        public static void ClearAll(this DirectoryInfo di)
        {
            foreach (var file in di.GetFiles())
                file.Delete();

            foreach (var dir in di.GetDirectories())
                dir.Delete(true);
        }
    }
}