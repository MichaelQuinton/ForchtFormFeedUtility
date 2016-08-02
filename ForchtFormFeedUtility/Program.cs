namespace ForchtFormFeedUtility
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;

    internal class Program
    {
        private static void Main(string[] args)
        {
            //Use the args passed to the program to build path references
            var p = new ProgramPaths(args[0]);

            //Exit program if the file doesn't exist
            if (!File.Exists(p.OriginalFullPath))
                return;

            //Check for the working directory.  Delete all files and directories
            //if it exists, otherwise create the directory.
            if (p.WorkingDirectory.Exists)
                p.WorkingDirectory.ClearAll();
            else
                p.WorkingDirectory.Create();

            //Unzip the original file contents to the working directory.
            UnzipFilesToWorkingDir(p.OriginalFullPath, p.WorkingDirectory);

            //Merge the files in the working directory together into one file
            MergeFiles(p);

            //Move the file back to the original file location overwritting the original
            File.Copy(Path.Combine(p.WorkingDirectory.FullName, p.OriginalFileName), p.OriginalFullPath, true);

            //Clear the working directory
            p.WorkingDirectory.ClearAll();
        }

        private static void MergeFiles(ProgramPaths p)
        {
            //Get the list of files from the working directory
            var fileList = Directory.GetFiles(p.WorkingDirectory.FullName);

            //Create a stream to the output file
            using (var outFile = new StreamWriter(Path.Combine(p.WorkingDirectory.FullName, p.OriginalFileName), false))
            {
                //Keep track of files to know when the last file is being worked on
                var counter = fileList.Length;
                //Read through each file checking for summary records to be discarded before writing the output
                foreach (var file in fileList)
                {
                    //If this is the last file remove the last form feed line from the data before writing out.
                    if (counter != 1)
                        WriteToFile(ParseFile(file), outFile);
                    else
                    {
                        var holder = ParseFile(file);
                        holder.RemoveAt(holder.Count - 1);
                        WriteToFile(holder, outFile);
                    }
                    counter--;
                }
            }
        }

        /// <summary>
        /// Read through the file checking records to see if they are summary records or not.
        /// </summary>
        /// <returns>A list of lines from the file without summary records</returns>
        private static List<string> ParseFile(string file)
        {
            //Open a stream for the file to read one line at a time
            using (var reader = new StreamReader(file))
            {
                //The individual line
                string line;
                //The group of lines up to and including the line with the form feed
                var lines = new List<string>();
                //The lines from the file minus the discarded summary lines
                var outLines = new List<string>();
                //Flag for summary lines to be discarded
                var isDiscardRecord = false;

                //Read through the file one line at at time
                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                    //If a form feed is found and the group is less than 30 lines possible discard
                    if (!line.Contains((char) 12)) continue;
                    if (lines.Count < 30)
                        //Check the first 10 lines to ensure they are blank
                        for (var i = 0; i < 10; i++)
                        {
                            //If any of the 10 lines is not blank the group is not a summary record
                            if (string.IsNullOrWhiteSpace(lines[i]))
                                isDiscardRecord = true;
                            else
                            {
                                isDiscardRecord = false;
                                break;
                            }
                        }

                    //Add lines to the output list unless they are to be discarded
                    if (!isDiscardRecord)
                        outLines.AddRange(lines);

                    //Clear the group of lines which if not added to outLines discards summary records
                    //and reset the discard flag
                    lines.Clear();
                    isDiscardRecord = false;
                }
                return outLines;
            }
        }

        /// <summary>
        /// Uses 7-zip command line to unzip the files located at the original full path location
        /// into the working directory located with the assembly.
        /// </summary>
        private static void UnzipFilesToWorkingDir(string originalFullPath, DirectoryInfo workingDirectory)
        {
            //Build the argument to pass to 7-zip
            var arugments = "e " + "\"" + originalFullPath + "\"" + " -o" + "\"" + workingDirectory + "\"";

            //Build the 7-Zip process
            var process = new ProcessStartInfo
            {
                FileName = @"C:\Program Files\7-Zip\7z.exe",
                Arguments = arugments,
                WindowStyle = ProcessWindowStyle.Hidden
            };
            //Keep console window hidden
            var x = Process.Start(process);
            //Let 7-Zip exit before continuing
            x?.WaitForExit();
        }

        /// <summary>
        /// Write parsed lines to the output file
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="outFile"></param>
        private static void WriteToFile(IEnumerable<string> lines, TextWriter outFile)
        {
            //Write each line to the output file
            foreach (var line in lines)
                outFile.WriteLine(line);
        }
    }
}