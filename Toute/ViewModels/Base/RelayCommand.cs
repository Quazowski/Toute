﻿using System;
using System.Windows.Input;

namespace Toute
{
    /// <summary>
    /// A relay command class, that helps handle
    /// all relay commands in application
    /// </summary>
    public class RelayCommand : ICommand
    {
        /// <summary>
        /// A action thats will be fired
        /// </summary>
        private Action Action { get; set; }

        /// <summary>
        /// A event handler of RelayCommand
        /// </summary>
        public event EventHandler CanExecuteChanged = (sender, e) => { };

        /// <summary>
        /// Command is always fired
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns>Always true</returns>
        public bool CanExecute(object parameter) => true;

        /// <summary>
        /// Constructor of relay command, and action is passed
        /// </summary>
        /// <param name="action"></param>
        public RelayCommand(Action action)
        {
            this.Action = action;
        }

        /// <summary>
        /// A action is executed
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            Action();
        }
    }
}
