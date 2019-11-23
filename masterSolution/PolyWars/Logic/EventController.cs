using PolyWars.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PolyWars.Logic.EventController
{
    static class KeyboardEvents
    {
        static KeyboardEvents() {
            Keyboard1 k = new Keyboard1();
            Keyboard.AddKeyUpHandler(dp, KeyPressedEventHandler);
            dp.
              
        }
        public static EventHandler<InputChangedEventArgs> InputChangedEventHandler;
        public static EventHandler<KeyEventArgs> KeyPressedEventHandler;
        private static DependencyObject dp = new DependencyObject();
        
    }
    class Keyboard1
    {
        
    }
}
