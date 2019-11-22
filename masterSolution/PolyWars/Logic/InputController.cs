using System;

namespace PolyWars.Logic {
    class InputController {

        /// <summary>
        /// Grabs input from a player
        /// </summary>
        private Input input;

        /// <summary>
        /// Initializes input
        /// </summary>
        public void initInput() {
            input = new Input();
        }

        /// <summary>
        /// Grabs input from user
        /// </summary>
        /// <returns>
        /// Input is returned
        /// </returns>
        public int getInput() {
            // TODO Grab input from user and apply to
            // triangle
            return input.getInput();
        }
    }
}