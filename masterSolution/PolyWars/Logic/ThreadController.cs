using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace PolyWars.Logic
{
    static class ThreadController
    {
        static ThreadController() {
            MainThreadDispatcher = Dispatcher.CurrentDispatcher;
        }
        public static Dispatcher MainThreadDispatcher { get; private set; } 
    }
}
