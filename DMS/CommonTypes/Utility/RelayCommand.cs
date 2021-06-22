using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CommonTypes.Utility
{
    public class RelayCommand<T> : ICommand
    {
        private Action<T> _Execute = null;
        private Predicate<T> _CanExecute = null;

        public RelayCommand(Action<T> execute, Predicate<T> canExecute = null)
        {
            _Execute = execute;
            _CanExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            _Execute?.Invoke((T)parameter);
        }

        public bool CanExecute(object parameter)
        {
            return _CanExecute == null ? true : _CanExecute.Invoke((T)parameter);
        }

    }
}
