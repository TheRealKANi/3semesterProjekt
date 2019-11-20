using System;

namespace PolyWars.Logic {
    class InputController {
        private Input input;

        public void initInput() {
            input = new Input();
        }

        public int getInput() {
            // TODO Grab input from user and apply to
            // triangle
            return input.getInput();
        }
    }
}