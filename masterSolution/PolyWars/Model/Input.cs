using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PolyWars.Logic {

    public enum ButtonDown {
        RIGHT = 1,
        UP = 2,
        LEFT = 4,
        DOWN = 8,
        SPACE = 16,
        DEBUG = 32
    }

    public class Input {

        private Key debugKey = Key.F3;
        Dictionary<Key, ButtonDown> keyBindings = new Dictionary<Key, ButtonDown>();

        public ButtonDown PressedKeys { get; set; }

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
            keyBindings[Key.Space] = ButtonDown.SPACE;
            keyBindings[debugKey] = ButtonDown.DEBUG;
        }

        public ButtonDown queryInput() {
            try {
                ThreadController.MainThreadDispatcher.Invoke(() => {
                    PressedKeys &= 0;

                    if(Keyboard.FocusedElement is Frame frame && frame.Content.ToString().Contains("PolyWars")) {
                        foreach(KeyValuePair<Key, ButtonDown> keyBinding in this.keyBindings) {
                            PressedKeys |= Keyboard.IsKeyDown(keyBinding.Key) ? keyBinding.Value : 0;
                        }
                    }
                });
                return PressedKeys;
            } catch(TaskCanceledException) {

            }
            return PressedKeys;
        }
    }
}
