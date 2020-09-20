using System;
using System.Collections.Generic;
using System.Text;
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
        public Action _action { get; set; }
        /// <summary>
        /// A event handler of RelayCommand
        /// </summary>
        public event EventHandler CanExecuteChanged = (sender, e) => { };

        /// <summary>
        /// Command is always fired
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter) => true;

        /// <summary>
        /// Constructor of relay command, and action is passed
        /// </summary>
        /// <param name="action"></param>
        public RelayCommand(Action action)
        {
            _action = action;
        }

        /// <summary>
        /// A action is executed
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            _action();
        }
    }
}
