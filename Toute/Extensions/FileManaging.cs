using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Toute
{
    /// <summary>
    /// Class that hold methods for a managing a files
    /// </summary>
    public static class FileManaging
    {
        #region Files static Methods

        /// <summary>
        /// Creates a file in given path
        /// </summary>
        /// <param name="path">Relative path where to create a file
        /// for example "Files/ImagesOfFiles"</param>
        /// <returns>Boolean value, true if creation was successful</returns>
        public static bool CreateAFile(string path)
        {
            //Gets full path where to create a file
            string pathToGivenFile = GetFullPath(path);

            //Try...
            try
            {
                //If file exists...
                if (!(File.Exists(pathToGivenFile)))
                {
                    //Creates file in given path
                    using (FileStream stream = File.Create(pathToGivenFile)) { };

                    //Returns successful
                    return true;
                }
            }
            //If error accrued...
            catch (Exception ex)
            {
                //Lets developer know
                Debugger.Break();
                //Writes a exception to debug
                Debug.WriteLine($"{ex}");
            }

            //If file creation failed, return unsuccessful
            return false;
        }

        /// <summary>
        /// Adds text to file in given path
        /// </summary>
        /// <param name="path">Relative path to file, where add text
        /// for example "Files/MyText.txt"</param>
        /// <param name="text">The text which add to a file</param>
        public static void AddTextToFile(string path, string text)
        {
            //Get full path to file
            string pathToGivenFile = GetFullPath(path);

            //Try...
            try
            {
                //Write a text to a file
                using StreamWriter streamWriter = File.CreateText(pathToGivenFile);
                 streamWriter.WriteLine(text);
            }
            //If error accrued...
            catch (Exception ex)
            {
                //Lets developer know
                Debugger.Break();
                //Writes a exception to debug
                Debug.WriteLine($"{ex}");
            }

        }

        /// <summary>
        /// Reads all the from a file from a given path
        /// </summary>
        /// <param name="path">Relative path to file, where add text
        /// for example "Files/MyText.txt"</param>
        /// <returns>Text from a file</returns>
        public static string ReadTextFromAFile(string path)
        {
            //Gets a full path
            string pathToGivenFile = GetFullPath(path);

            //Creates a context, with a empty string
            string context = "";

            //Try...
            try
            {
                //Reads all text
                using StreamReader streamReader = File.OpenText(pathToGivenFile);
                    context = streamReader.ReadToEnd();
            }
            //If error accrued...
            catch (Exception ex)
            {
                //Lets developer know
                Debugger.Break();
                //Writes a exception to debug
                Debug.WriteLine($"{ex}");
            }

            //Returns a context from a file
            //or if error accrued returns empty string
            return context;          
        }

        /// <summary>
        /// Reads all .txt files from a directory from a given path
        /// </summary>
        /// <param name="path">Relative path to directory, from where
        /// read all texts, for example "Files"</param>
        /// <returns>List of all .txt files content</returns>
        public static List<string> ReadAllTextFilesFromFolder(string path)
        {
            //Creates new list of strings
            var newList = new List<string>();

            //Gets full path to directory
            string pathToGivenFile = GetFullPath(path);

            //Try...
            try
            {
                //For all .txt files in directory...
                foreach (var file in Directory.GetFiles(pathToGivenFile, "*.txt"))
                {
                    //Adds them to the list
                    newList.Add(File.ReadAllText(file));
                }
            }
            //If error accrued...
            catch (Exception ex)
            {
                //Lets developer know
                Debugger.Break();
                //Writes a exception to debug
                Debug.WriteLine($"{ex}");
            }

            //Returns a list
            return newList;
        }

        /// <summary>
        /// Deletes a file from of given path
        /// </summary>
        /// <param name="path">Relative path to file, which remove
        /// for example Files/MyText.txt</param>
        public static void DeleteFile(string path)
        {
            //Gets full path
            string pathToGivenFile = GetFullPath(path);

            //Try...
            try
            {
                //If file exists...
                if (File.Exists(pathToGivenFile))
                {
                    //Collect and clear GarbageCollector
                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    //Delete a file
                    File.Delete(pathToGivenFile);
                }
            }
            //If error accrued...
            catch (Exception ex)
            {
                //Lets developer know
                Debugger.Break();
                //Writes a exception to debug
                Debug.WriteLine($"{ex}");
            }
        }

        #endregion

        #region Helpers method

        /// <summary>
        /// Get a full path
        /// </summary>
        /// <param name="path">path to which create a file</param>
        /// <returns>Full path</returns>
        private static string GetFullPath(string path)
        {
            //Gets path to main directory
            string pathToDirectory = Path.GetFullPath(@"..\..\..\");

            //Creates a path to where manage a file
            string pathToGivenFile = Path.Combine(pathToDirectory, path);

            //Returns path
            return pathToGivenFile;
        }

        #endregion

    }
}
