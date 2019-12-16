using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PolyWars.Client.Logic {

    /// <summary>
    /// Basic bit flipping keys
    /// </summary>
    public enum ButtonDown {
        RIGHT = 0b000001,
        UP = 0b000010,
        LEFT = 0b000100,
        DOWN = 0b001000,
        SHOOT = 0b010000,
        DEBUG = 0b100000
    }

    public class Input {

        private Key debugKey = Key.F3;

        public ButtonDown PressedKeys { get; set; }

        Dictionary<Key, ButtonDown> keyBindings = new Dictionary<Key, ButtonDown>();
        public Input() {
            PressedKeys = new ButtonDown();

            keyBindings[Key.W] = ButtonDown.UP;
            keyBindings[Key.A] = ButtonDown.LEFT;
            keyBindings[Key.S] = ButtonDown.DOWN;
            keyBindings[Key.D] = ButtonDown.RIGHT;
            keyBindings[Key.Up] = ButtonDown.UP;
            keyBindings[Key.Left] = ButtonDown.LEFT;
            keyBindings[Key.Down] = ButtonDown.DOWN;
            keyBindings[Key.Right] = ButtonDown.RIGHT;
            keyBindings[Key.Space] = ButtonDown.SHOOT;
            keyBindings[debugKey] = ButtonDown.DEBUG;
        }

        public ButtonDown queryInput() {
            try {
                UIDispatcher.Invoke(() => {
                    PressedKeys &= 0;
                    if(Application.Current != null && Application.Current.MainWindow.IsKeyboardFocused) {
                        foreach(KeyValuePair<Key, ButtonDown> keyBinding in keyBindings) {
                            PressedKeys |= Keyboard.IsKeyDown(keyBinding.Key) ? keyBinding.Value : 0;
                        }
                    }
                });
                return PressedKeys;
            } catch(NullReferenceException) { } // can sometimes occur if application is closed, and the static Application class itself is null. Does not need handling
             catch(TaskCanceledException) { } // occurs when the application get closed, and the UIDispatcher no longer exists. Does not need handling

            return PressedKeys;
        }
    }
}
