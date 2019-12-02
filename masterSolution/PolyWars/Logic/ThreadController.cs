using System.Windows;
using System.Windows.Threading;

namespace PolyWars.Logic {
    static class ThreadController {
        static ThreadController() {
            MainThreadDispatcher = Application.Current.Dispatcher;
        }
        public static Dispatcher MainThreadDispatcher { get; private set; }
    }
}
