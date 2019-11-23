using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PolyWars.Logic
{
    enum ButtonDown
    {
        RIGHT = 1,
        UP = 2,
        LEFT = 4,
        DOWN = 8
    }
    class Input1
    {
        Dictionary<Key, ButtonDown> keyBindings = new Dictionary<Key, ButtonDown>();
        public ButtonDown PressedKeys;
        public Input1()
        {
            PressedKeys = new ButtonDown();

            keyBindings[Key.W] = ButtonDown.UP;
            keyBindings[Key.A] = ButtonDown.LEFT;
            keyBindings[Key.S] = ButtonDown.DOWN;
            keyBindings[Key.D] = ButtonDown.RIGHT;
            keyBindings[Key.Up] = ButtonDown.UP;
            keyBindings[Key.Left] = ButtonDown.LEFT;
            keyBindings[Key.Down] = ButtonDown.DOWN;
            keyBindings[Key.Right] = ButtonDown.RIGHT;
        }

        public ButtonDown checkInput()
        {
            ThreadController.MainThreadDispatcher.Invoke(() =>
           {
               PressedKeys &= 0;
               foreach (var keyBinding in keyBindings)
               {
                   PressedKeys |= (Keyboard.IsKeyDown(keyBinding.Key) ? keyBinding.Value : 0);
               }
           });
            return PressedKeys;
        }
    }
}
