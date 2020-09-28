using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text.Json;

namespace Toute
{
    /// <summary>
    /// Extensions for a Dialogs
    /// </summary>
    public static class DialogExtensions
    {
        /// <summary>
        /// Creates a dialog, and extract needed informations from a file
        /// </summary>
        /// <returns>GameModel with parameters</returns>
        public static GameModel FindGame()
        {
            //Create new OpenFileDialog
            var Dialog = new Microsoft.Win32.OpenFileDialog();

            //Open new dialog, awaiting to chose a file, or close window
            Dialog.ShowDialog();

            //If there is any chosen file...
            if(!(string.IsNullOrEmpty(Dialog.FileName)) && !(string.IsNullOrEmpty(Dialog.SafeFileName)))
            {
                //Make a unique path to the image
                string pathToImage = string.Concat(@"..\..\..\Files\ImagesOfFiles\",
                    Guid.NewGuid().ToString(),
                    Dialog.SafeFileName.Remove(Dialog.SafeFileName.IndexOf('.')),
                    ".png");

                //Make a unique path for a file
                string fileName = string.Concat(@"Files\",
                    Guid.NewGuid().ToString(),
                    Dialog.SafeFileName.Remove(Dialog.SafeFileName.IndexOf('.')), ".txt");


                //TODO: Add logger when catching error
                try
                {
                    //Extract Icon from a file
                    Icon icon = Icon.ExtractAssociatedIcon(Dialog.FileName);

                    //Convert icon to Bitmap
                    using Bitmap bmp = icon.ToBitmap();

                    //Save it to the given path
                    bmp.Save(pathToImage, ImageFormat.Png);
                }
                catch(Exception ex)
                {
                    //Let developer know
                    Debugger.Break();
                    //Write a exception to debug
                    Debug.WriteLine($"{ex}");
                }

                //Create new GameModel
                var gameModel = new GameModel
                {
                    //Set title
                    Title = Dialog.SafeFileName.Remove(Dialog.SafeFileName.IndexOf('.')),

                    //Set image
                    PathToImage = pathToImage,

                    //Set path to File
                    PathToFile = fileName,

                    //Set path to the game
                    PathToGame = Dialog.FileName
                };

                //TODO: Add logger when catching error
                try
                {
                    //Serialize a gameModel to Json
                    var gameModelSeralized = JsonSerializer.Serialize(gameModel);

                    //Create a file of given path
                    FileManaging.CreateAFile(fileName);

                    //Add gameModel in JsonString format, to the file
                    FileManaging.AddTextToFile(fileName , gameModelSeralized);
                }
                catch(Exception ex)
                {
                    //Let developer know
                    Debugger.Break();
                    //Write a exception to debug
                    Debug.WriteLine($"{ex}");
                }


                //return GameModel
                return gameModel;
            }

            //If no file was chosen, return null
            return null;

        }
    }
}
