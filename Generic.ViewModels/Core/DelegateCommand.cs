using System;
using System.Windows.Input;

namespace GenericViewModels.Core
{
    public class DelegateCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool>? _canExecute;

        public DelegateCommand(Action execute, Func<bool>? canExecute) =>
            (_execute, _canExecute) = (execute, canExecute);

        public DelegateCommand(Action execute)
            : this(execute, null) { }

        public event EventHandler? CanExecuteChanged;

        public virtual void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        public bool CanExecute(object parameter) => _canExecute?.Invoke() ?? true;

        public void Execute(object parameter) => _execute.Invoke();
    }
}
