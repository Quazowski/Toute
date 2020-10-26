using System;
using System.Collections.Generic;
using System.Drawing;
using Toute.Core;

namespace Toute
{
    /// <summary>
    /// Helpers for a Dialogs
    /// </summary>
    public static class DialogHelpers
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
            if (!(string.IsNullOrEmpty(Dialog.FileName)) && !(string.IsNullOrEmpty(Dialog.SafeFileName)))
            {
                //Extract Icon from a file
                Icon icon = Icon.ExtractAssociatedIcon(Dialog.FileName);

                //Create new GameModel
                var gameModel = new GameModel
                {
                    Title = Dialog.SafeFileName.Remove(Dialog.SafeFileName.IndexOf('.')),
                    FileId = Guid.NewGuid().ToString(),
                    BytesImage = icon.IconToBytes(),
                    Paths = new List<StringDataModel> { new StringDataModel { Value = Dialog.FileName } },

                };

                return gameModel;
            }

            //If no file was chosen, return null
            return null;
        }

        /// <summary>
        /// Dialog helper that help to change path for the game
        /// </summary>
        /// <returns>New path</returns>
        public static GameModel SetNewIconAndPathForFile()
        {
            //Create new OpenFileDialog
            var Dialog = new Microsoft.Win32.OpenFileDialog();

            //Open new dialog, awaiting to chose a file, or close window
            Dialog.ShowDialog();

            //If there is any chosen file...
            if (!(string.IsNullOrEmpty(Dialog.FileName)) && !(string.IsNullOrEmpty(Dialog.SafeFileName)))
            {
                //Extract Icon from a file
                Icon icon = Icon.ExtractAssociatedIcon(Dialog.FileName);

                //Create new GameModel
                var gameModel = new GameModel
                {
                    BytesImage = icon.IconToBytes(),
                    Paths = new List<StringDataModel> { new StringDataModel { Value = Dialog.FileName } },
                };

                return gameModel;
            }

            //If no file was chosen, return null
            return null;
        }
    }
}
