using System;
using System.Windows;
using System.Windows.Threading;

namespace PolyWars.Logic {
    internal class UnitTestUIDispatcher : UIDispatcher {
        public UnitTestUIDispatcher() {
            uiDispatcher = this;
        }
        protected override void _Invoke(Action a) {
            a.Invoke();
        }
        protected override T _Invoke<T>(Func<T> a) {
            return a.Invoke();
        }
    }
    class UIDispatcher {
        protected UIDispatcher() { }
        static UIDispatcher() {
            uiDispatcher = new UIDispatcher();
            try {
                UIThreadDispatcher = Application.Current.Dispatcher;
            } catch(NullReferenceException) { // occurs when unittests are run, since the program isn't running, and as such, has no dispatcher
            }
        }
        protected static UIDispatcher uiDispatcher;
        private static Dispatcher UIThreadDispatcher;

        public static void Invoke(Action a) {
            uiDispatcher._Invoke(a);
        }
        public static T Invoke<T>(Func<T> a) {
            return uiDispatcher._Invoke(a);
        }

        protected virtual void _Invoke(Action a) {
            UIThreadDispatcher.Invoke(() => a.Invoke());
        }
        protected virtual T _Invoke<T>(Func<T> a) {
            return UIThreadDispatcher.Invoke(() => { return a.Invoke(); });
        }
    }
}