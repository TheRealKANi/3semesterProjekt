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
        int i = 0;
        public void applyInput(object sender, InputChangedEventArgs args) {
            // TODO Grab input and apply to shape
            if(sender is Input input) {
                if(args.Movement != 0) {
                    Player.Velocity = Player.MaxVelocity;
                }
                Player.HorizontialSpeed = 
                    (int)((args.Movement & 0b0001) * Player.Velocity * Math.Cos((double)Player.Angle / 180 * Math.PI) -
                    ((args.Movement & 0b0100) >> 2) * Player.Velocity * Math.Cos(Math.PI + Math.PI + (double)Player.Angle / 180 * Math.PI));
                
                Player.VerticalSpeed = 
                    (int)(((args.Movement & 0b0010) >> 1) * Player.Velocity * Math.Sin(Math.PI / 2 +(double)Player.Angle / 180 * Math.PI) +
                    ((args.Movement & 0b1000) >> 3) * Player.Velocity * Math.Sin((3d / 2) * Math.PI + (double)Player.Angle / 180 * Math.PI));

                Debug.WriteLine($"\nHorizontal: {Player.HorizontialSpeed}\nVertical: {Player.VerticalSpeed} iteration: {i++}\n");
            }
        }
    }
}