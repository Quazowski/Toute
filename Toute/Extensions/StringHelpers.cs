using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Documents;

namespace Toute
{
    /// <summary>
    /// Helpers for the string
    /// </summary>
    public static class StringHelpers
    {
        /// <summary>
        /// Helper that find .exe that match closest in the file. It based on
        /// searching .exe like the directory file name
        /// </summary>
        /// <param name="filePath">Full path where to look for</param>
        /// <returns>full path to the exe file</returns>
        public static string ClosestExeToFileName(string filePath)
        {
            try
            {
                //Convert folder name to simplest name
                var directoryNameFiltered = filePath.Substring(filePath.LastIndexOf('\\') + 1).ToLower().Replace(" ", "");

                //Find all .exe files in the path
                var exeFiles = Directory.GetFiles(filePath, "*.exe", SearchOption.AllDirectories);

                //if there is more than 1 .exe file found
                if (exeFiles.Count() > 1)
                {
                    //Create list for filtered items
                    var filter1 = new List<string>();

                    //For each file that exist
                    foreach (var f1 in exeFiles)
                    {
                        //Check if any of this .exe file contain the name of the directory
                        if (f1.Substring(f1.LastIndexOf('\\') + 1).ToLower().Replace(" ", "").Replace("-", "").Replace("_", "").Contains(directoryNameFiltered))
                        {
                            //If it has, add to filtered items
                            filter1.Add(f1);
                        }
                    }

                    //If filtered items count is 0
                    if (filter1.Count() <= 0)
                    {
                        //Return first item from the list
                        return exeFiles.FirstOrDefault();
                    }
                    //If there is still more items than 1
                    else if (filter1.Count() > 1)
                    {
                        //For each item, check if name of simplest directory and
                        //name of simplest .exe file are the same
                        foreach (var f2 in filter1)
                        {
                            if (f2 == directoryNameFiltered)
                            {
                                return f2;
                            }
                        }

                        //If there is no item with the same name, as directory
                        //return first item from filtered items
                        return filter1.FirstOrDefault();
                    }
                    else
                    {
                        return filter1.FirstOrDefault();
                    }
                }
                //If there is less than 1 item, return first item
                else
                {
                    return exeFiles.FirstOrDefault();
                }
            }
            //Skip if it is unauthorized ex
            catch (UnauthorizedAccessException) { return null; }

        }

        public static List<string> FindPathToExe(string filePath, string[] nameOfExeFiles, CancellationToken token)
        {

            List<string> Paths = new List<string>();

            string[] directories = Directory.GetDirectories(filePath);
            foreach (var directory in directories)
            {
                foreach (var exeFile in nameOfExeFiles)
                {
                    if(token.IsCancellationRequested)
                    {
                        return Paths;
                    }

                    try
                    {
                        Paths.Add(Directory.GetFiles(directory, $"{exeFile}.exe", SearchOption.AllDirectories).FirstOrDefault());
                    }
                    catch (UnauthorizedAccessException) { }
                }
            }

            return Paths;
        }

    }
}
