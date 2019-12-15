using System;
using System.Windows.Input;

namespace PolyWars.Client.Model {
    /// <summary>
    /// Commands are used to separate the semantics and the object that invokes a command from the logic that executes the command.
    /// </summary>
    class RelayCommand : ICommand {
        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested += value; }
        }
        Predicate<object> canExecute;
        Action<object> execute;

        public RelayCommand(Predicate<object> canExecute, Action<object> execute) {
            this.canExecute = canExecute;
            this.execute = execute;
        }

        public RelayCommand(Predicate<object> canExecute, Action execute) : this(canExecute, (o) => execute.Invoke()) { } //parameterless execute
        public RelayCommand(Action<object> execute) : this(null, execute) { } // no canExecute method
        public RelayCommand(Action execute) : this(null, execute) { } // no canExecute method and parameterless execute

        public bool CanExecute(object parameter) {
            if(canExecute == null) return true;
            return canExecute.Invoke(parameter);
        }

        public void Execute(object parameter) {
            execute(parameter);
        }
    }
}
