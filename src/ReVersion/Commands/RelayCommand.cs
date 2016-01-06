using System;
using System.Windows.Input;

namespace ReVersion.Commands
{
    internal class RelayCommand : ICommand
    {
        private readonly Action<object> _action;
        
        public RelayCommand(Action<object> action)
        {
            _action = action;
        }

        public bool CanExecute(object parameter) => true;

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _action(parameter);
        }
    }
}
