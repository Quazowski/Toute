using System;
using System.Windows.Input;

namespace Toute
{
    public class ParamizedRelayCommand : ICommand
    {
        private readonly Action<object> action;

        public event EventHandler CanExecuteChanged = (sender, e) => { };

        public ParamizedRelayCommand(Action<object> action)
        {
            this.action = action;
        }
        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter)
        {
            action(parameter);
        }
    }
}
