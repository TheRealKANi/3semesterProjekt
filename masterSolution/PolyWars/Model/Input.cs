using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PolyWars.Logic {

    /// <summary>
    /// Binds controls to user defined keys 
    /// </summary>
    class Input {
        private Dictionary<Key, Action<KeyStates>> keyBindings = new Dictionary<Key, Action<KeyStates>>();
        public Input() {
            Movement = 0;
            initInput();
        }
        /// <summary>
        /// Binds key on actions in this case WASD and Arrow keys are defined to move four directions
        /// </summary>
        /// <returns>
        /// Returns the actions?
        /// </returns>
        public bool initInput() {
            bool result = false;
            keyBindings[Key.W] = new Action<KeyStates>(MoveUp);
            keyBindings[Key.A] = new Action<KeyStates>(MoveLeft);
            keyBindings[Key.S] = new Action<KeyStates>(MoveDown);
            keyBindings[Key.D] = new Action<KeyStates>(MoveRight);
            keyBindings[Key.Up] = new Action<KeyStates>(MoveUp);
            keyBindings[Key.Left] = new Action<KeyStates>(MoveLeft);
            keyBindings[Key.Down] = new Action<KeyStates>(MoveDown);
            keyBindings[Key.Right] = new Action<KeyStates>(MoveRight);

            // TODO  Make verification logic
            if(keyBindings.Count > 0) {
                result = true;
            }
            return result;
        }


        /// <summary>
        /// Changes state when a Key is pressed 
        /// </summary>
        public int Movement { get; set; }
        private void MoveUp(KeyStates state)
        {
            Movement = state == KeyStates.Toggled ? Movement | 2 : Movement & 13;
        }
        private void MoveDown(KeyStates state)
        {
            Movement = state == KeyStates.Toggled ? Movement | 8 : Movement & 7;
        }

        private void MoveRight(KeyStates state)
        {
            Movement = state == KeyStates.Toggled ? Movement | 1 : Movement & 14;
        }
        private void MoveLeft(KeyStates state)
        {
            Movement = state == KeyStates.Toggled ? Movement | 4 : Movement & 11;
        }

        /// <summary>
        /// This method checks that only valid keys are pressed, which are those that is binded, all unbinded keypresses is ignored
        /// </summary>
        public void onKeyStateChanged(object sender, KeyEventArgs e)
        {
            try
            {
                keyBindings[e.Key].Invoke(e.KeyStates); 
            }
            catch (KeyNotFoundException)
            {
                // ignore unbound keypresses
            }
        }

        /// <summary>
        /// When input is received a movement is visible for user
        /// </summary>
        /// <returns></returns>
        public int getInput() {
            return Movement;
        }
    }
}

