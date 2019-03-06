using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Calcs
{
    public class CommandHandlerWithParameter : ICommand
    {
        private Action<Type> _action;
        private bool _canExecute;
        public CommandHandlerWithParameter(Action<Type> action, bool canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _action((Type)parameter);
        }
    }
}
