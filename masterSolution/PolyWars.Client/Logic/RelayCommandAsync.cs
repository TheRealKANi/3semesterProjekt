using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PolyWars.Client.Logic {
    class RelayCommandAsync : ICommand {
        Func<object, Task> execute;
        Predicate<object> canExecute;
        bool isExecuting;

        public RelayCommandAsync(Func<Task> execute) : this(execute, null) { }
        public RelayCommandAsync(Func<object, Task> execute) : this(execute, null) { }
        public RelayCommandAsync(Func<object, Task> execute, Predicate<object> canExecute) {
            this.execute = execute;
            this.canExecute = canExecute;
        }
        public RelayCommandAsync(Func<Task> execute, Predicate<object> canExecute) {
            this.execute = (o) => { return execute.Invoke(); };
            this.canExecute = canExecute;
            isExecuting = false;
        }

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter) {
            if(!isExecuting && canExecute == null) {
                return true;
            }
            return !isExecuting && canExecute.Invoke(parameter); ;
        }

        public async void Execute(object parameter) {
            isExecuting = true;
            try {
                await execute.Invoke(parameter);
            } finally {
                isExecuting = false;
            }
        }
    }
}
