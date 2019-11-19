﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PolyWars.Model
{
    class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        Func<object, bool> canExecute;
        Action<object> execute;

        public RelayCommand(Func<object, bool> canExecute, Action<object> execute) {
            this.canExecute = canExecute;
            this.execute = execute;
        }

        public bool CanExecute(object parameter) {
            return canExecute.Invoke(parameter);
        }

        public void Execute(object parameter) {
            execute(parameter);
        }
    }
}
