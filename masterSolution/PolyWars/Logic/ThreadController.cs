using System;
using System.Windows;
using System.Windows.Threading;

namespace PolyWars.Logic {
    static class UIDispatcher {
        static UIDispatcher() {
            UIThreadDispatcher = Application.Current.Dispatcher;
        }
        private static Dispatcher UIThreadDispatcher;

        public static void Invoke(Action a) {
            UIThreadDispatcher.Invoke( () => a.Invoke());
        }
        public static T Invoke<T>(Func<T> a) {
            return UIThreadDispatcher.Invoke(() => { return a.Invoke(); });
        }
    }
}
