using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace PolyWars.Logic {
    static class UIDispatcher {
        static UIDispatcher() {
            UIThreadDispatcher = Application.Current.Dispatcher;
        }
        private static Dispatcher UIThreadDispatcher;

        public static void Invoke(Action a) {
            try {
                UIThreadDispatcher.Invoke( () => a.Invoke());
            } catch(TaskCanceledException e) {
                Debug.WriteLine("UIDispatcher: Task got canceled");
            }
        }
        public static T Invoke<T>(Func<T> a) {
            return UIThreadDispatcher.Invoke(() => { return a.Invoke(); });
        }
    }
}
