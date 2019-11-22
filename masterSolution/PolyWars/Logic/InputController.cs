using PolyWars.API;
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

        public void applyInput( IShape playerShape ) {
            // TODO Grab input and apply to shape
            int Movement = input.Movement;
        }
    }
}