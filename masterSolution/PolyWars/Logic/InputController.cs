using PolyWars.API;
using PolyWars.Model;
using System;
using System.Diagnostics;

namespace PolyWars.Logic
{
    class InputController
    {

        /// <summary>
        /// Grabs input from a player
        /// </summary>
        public Input Input { get; private set; }

        /// <summary>
        /// Initializes input
        /// </summary>

        public Triangle Player { get; private set; }

        public void initInput(Triangle player)
        {
            Input = new Input();
            Player = player;
            EventController.KeyboardEvents.InputChangedEventHandler += applyInput;
        }
        public void applyInput(object sender, InputChangedEventArgs args)
        {
            // TODO Grab input and apply to shape
            if (sender is Input input)
            {
                Player.RPS =
                    (int)(args.Movement & ButtonDown.RIGHT) * Player.MaxRPS -
                    (int)(args.Movement & ButtonDown.LEFT) * Player.MaxRPS;
                Player.Velocity =
                    (int)(args.Movement & ButtonDown.UP) * Player.MaxVelocity -
                    (int)(args.Movement & ButtonDown.DOWN) * Player.MaxVelocity;
            }
        }
    }
}