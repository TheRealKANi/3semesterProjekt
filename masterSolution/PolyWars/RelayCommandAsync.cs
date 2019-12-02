using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PolyWars {
    class RelayCommandAsync : ICommand {
        Func<Task> execute;
        Predicate<object> canExecute;
        bool isExecuting;

        public RelayCommandAsync(Func<Task> execute) : this(execute, null) { }
        public RelayCommandAsync(Func<Task> execute, Predicate<object> canExecute) {
            this.execute = execute;
            this.canExecute = canExecute;
            isExecuting = false;
        }

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter) {
            if(!isExecuting || execute == null) {
                return true;
            } 
            return !isExecuting && canExecute(parameter);
        }

        public async void Execute(object parameter) {
            isExecuting = true;
            try {
                await execute.Invoke();
            } finally {
                isExecuting = false;
            }
        }
    }
}
