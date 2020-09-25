using System.Diagnostics;
using System.IO;

namespace Toute
{
    /// <summary>
    /// Helpers for Process of System.IO namespace
    /// </summary>
    public static class ProcessExtensions
    {
        /// <summary>
        /// Method that will run any file on local machine
        /// </summary>
        /// <param name="process">new process</param>
        /// <param name="path">Path to file</param>
        public static void RunFile(this Process process, string path)
        {
            //Set basic info about process
            ProcessStartInfo info = new ProcessStartInfo(path)
            {
                //Set path to file which open
                Arguments = Path.GetFileName(path),
                // Will use shell when starting the process
                UseShellExecute = true,
                //Set window open style to normal
                WindowStyle = ProcessWindowStyle.Normal,
                // verb to use when opening the application or document specified
                // by the System.Diagnostics.ProcessStartInfo.FileName property.
                Verb = "OPEN"
            };

            //Set process StartInfo to info
            process.StartInfo = info;

            //Run the process
            process.Start();
        }
    }
}
