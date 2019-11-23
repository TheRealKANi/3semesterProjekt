using PolyWars.API;
using PolyWars.Model;
using System;
using System.Diagnostics;

namespace PolyWars.Logic {
    class InputController {

        /// <summary>
        /// Grabs input from a player
        /// </summary>
        public Input Input { get; private set; }

        /// <summary>
        /// Initializes input
        /// </summary>

        public Triangle Player { get; private set; }

        public void initInput(Triangle player) {
            Input = new Input();
            Player = player;
            EventController.KeyboardEvents.InputChangedEventHandler += applyInput;
        }
        public void applyInput(object sender, InputChangedEventArgs args) {
            // TODO Grab input and apply to shape
            if(sender is Input input) {
                Player.RPS =
                    (args.Movement & 0b0001) * Player.MaxRPS  -
                    ((args.Movement & 0b0100) >> 2) * Player.MaxRPS;
                Player.Velocity =
                    ((args.Movement & 0b0010) >> 1) * Player.MaxVelocity -
                    ((args.Movement & 0b1000) >> 3) * Player.MaxVelocity;
                Debug.WriteLine($"\nRPS: {Player.RPS}\nVelocity: {Player.Velocity}\n");
            }
        }
    }
}