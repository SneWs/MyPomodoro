using System;
using System.Windows.Input;

namespace MyPomodoro.MVVM
{
    public class RelayCommand<TArgument> : ICommand
        where TArgument : class
    {
        private readonly Func<TArgument, bool> _canExecute;
        private readonly Action<TArgument> _execute;

        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action<TArgument> execute)
            : this(execute, x => true)
        { }

        public RelayCommand(Action<TArgument> execute, Func<TArgument, bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute(parameter as TArgument);
        }

        public void Execute(object parameter)
        {
            _execute(parameter as TArgument);
        }
    }
}
