using System;
using System.Windows.Input;

namespace Toute
{
    /// <summary>
    /// A parametrized relay command class, that helps handle
    /// parametrized commands in application
    /// </summary>
    public class ParametrizedRelayCommand : ICommand
    {
        /// <summary>
        /// A action of type object thats will be fired
        /// </summary>
        private readonly Action<object> action;

        /// <summary>
        /// A event handler of ParametrizedRelayCommand
        /// </summary>
        public event EventHandler CanExecuteChanged = (sender, e) => { };

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="action">Action will be sent with object parameter</param>
        public ParametrizedRelayCommand(Action<object> action)
        {
            this.action = action;
        }

        /// <summary>
        /// Command is always fired
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns>Always true</returns>
        public bool CanExecute(object parameter) => true;

        /// <summary>
        /// A action is executed
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            action(parameter);
        }
    }
}
