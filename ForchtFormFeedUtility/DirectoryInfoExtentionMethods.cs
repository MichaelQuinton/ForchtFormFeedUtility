using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForchtFormFeedUtility
{
    public static class DirectoryInfoExtentionMethods
    {
        /// <summary>
        /// Add a method to DirectoryInfo that deletes all files and folders in the directory
        /// </summary>
        /// <param name="di"></param>
        public static void ClearAll(this DirectoryInfo di)
        {
            foreach (FileInfo file in di.GetFiles())
                file.Delete();

            foreach (DirectoryInfo dir in di.GetDirectories())
                dir.Delete(true);
        }
    }
}
