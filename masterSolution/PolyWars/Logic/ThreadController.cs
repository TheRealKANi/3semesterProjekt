﻿using System.Windows.Threading;

namespace PolyWars.Logic {
    static class ThreadController {
        static ThreadController() {
            MainThreadDispatcher = Dispatcher.CurrentDispatcher;
        }
        public static Dispatcher MainThreadDispatcher { get; private set; }
    }
}