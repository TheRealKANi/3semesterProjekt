using PolyWars.Model;
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
        private Dictionary<Key, Action> keyBindings = new Dictionary<Key, Action>();

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
            keyBindings[Key.W] = new Action(MoveUp);
            keyBindings[Key.A] = new Action(MoveLeft);
            keyBindings[Key.S] = new Action(MoveDown);
            keyBindings[Key.D] = new Action(MoveRight);
            keyBindings[Key.Up] = new Action(MoveUp);
            keyBindings[Key.Left] = new Action(MoveLeft);
            keyBindings[Key.Down] = new Action(MoveDown);
            keyBindings[Key.Right] = new Action(MoveRight);

            EventController.KeyboardEvents.KeyPressedEventHandler += onKeyStateChanged;

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
        private void MoveUp()
        {
            Movement ^= 0b0010;
        }
        private void MoveDown()
        {
            Movement ^= 0b1000;
        }

        private void MoveRight()
        {
            Movement ^= 0b0001;
        }

        private void MoveLeft()
        {
            Movement ^= 0b0100;
        }

        /// <summary>
        /// This method checks that only valid keys are pressed, which are those that is binded, all unbinded keypresses is ignored
        /// </summary>
        public void onKeyStateChanged(object sender, KeyEventArgs e)
        {
            if((e.KeyStates & KeyStates.Down) > 0 && !e.IsRepeat || e.KeyStates == KeyStates.None)  {
                try {
                    keyBindings[e.Key].Invoke();
                    EventController.KeyboardEvents.InputChangedEventHandler?.Invoke(this, new InputChangedEventArgs(Movement));
                } catch(KeyNotFoundException) {
                    // ignore unbound keypresses
                } 
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

