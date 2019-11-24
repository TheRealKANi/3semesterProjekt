using PolyWars.Logic;
using System;

namespace PolyWars.Model {
    class InputChangedEventArgs : EventArgs {

        public ButtonDown Movement { get; private set; }

        public InputChangedEventArgs( ButtonDown movement ) {
            this.Movement = movement;
        }
    }
}
