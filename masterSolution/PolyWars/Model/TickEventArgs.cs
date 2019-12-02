using System;

namespace PolyWars.Logic {
    public class TickEventArgs : EventArgs {
        public TickEventArgs(double deltaTime) {
            this.deltaTime = deltaTime;
        }

        public double deltaTime { get; private set; }
    }
}