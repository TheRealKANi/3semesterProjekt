using System;

namespace PolyWars.Client.Logic {
    public class TickEventArgs : EventArgs {
        public TickEventArgs(double deltaTime) {
            this.deltaTime = deltaTime;
        }
        public double deltaTime { get; private set; }
    }
}