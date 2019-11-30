using System;
using System.Windows.Input;

namespace PolyWars.Model {
    /// <summary>
    /// Commands are used to separate the semantics and the object that invokes a command from the logic that executes the command.
    /// </summary>
    class RelayCommand : ICommand {
        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested += value; }
        }
        Func<object, bool> canExecute;
        Action<object> execute;

        public RelayCommand( Func<object, bool> canExecute, Action<object> execute ) {
            this.canExecute = canExecute;
            this.execute = execute;
        }

        public bool CanExecute( object parameter ) {
            return canExecute.Invoke( parameter );
        }

        public void Execute( object parameter ) {
            execute( parameter );
        }
    }
}
