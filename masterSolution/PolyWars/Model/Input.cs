using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PolyWars.Logic {

    public enum ButtonDown {
        RIGHT = 0b1,
        UP = 0b10,
        LEFT = 0b100,
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
                ThreadController.MainThreadDispatcher.Invoke( () => {
                    PressedKeys &= 0;
                    foreach( KeyValuePair<Key, ButtonDown> keyBinding in this.keyBindings ) {
                        PressedKeys |= Keyboard.IsKeyDown( keyBinding.Key ) ? keyBinding.Value : 0;
                    }
                } );
                return PressedKeys;
            } catch( TaskCanceledException ) {

            }
            return PressedKeys;
        }
    }
}
