using PolyWars.API;
using PolyWars.Model;
using System;
using System.Diagnostics;

namespace PolyWars.Logic
{
    class InputController
    {
        private InputController() { }

        private static InputController instance;
        public static InputController Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new InputController();
                }
                return instance;
            }
        }
        /// <summary>
        /// Grabs input from a player
        /// </summary>
        public Input1 Input { get; private set; }

        /// <summary>
        /// Initializes input
        /// </summary>

        public Triangle Player { get; private set; }

        public void initInput(Triangle player)
        {
            Input = new Input1();
            Player = player;
            //EventController.KeyboardEvents.InputChangedEventHandler += applyInput;
        }
        public void queryInput()
        {
            ButtonDown input = Input.checkInput();
            Player.RPS =
                ((int)(input & ButtonDown.LEFT) >> 2) * Player.MaxRPS -
                (int)(input & ButtonDown.RIGHT) * Player.MaxRPS;
            Player.Velocity =
                ((int)(input & ButtonDown.UP) >> 1) * Player.MaxVelocity -
                ((int)(input & ButtonDown.DOWN) >> 3) * Player.MaxVelocity;
        }
        public void applyInput(object sender, InputChangedEventArgs args)
        {
            // TODO Grab input and apply to shape
            if (sender is Input1 input)
            {
                Player.RPS =
                    (int)(args.Movement & ButtonDown.LEFT) * Player.MaxRPS -
                    (int)(args.Movement & ButtonDown.RIGHT) * Player.MaxRPS;
                Player.Velocity =
                    (int)(args.Movement & ButtonDown.UP) * Player.MaxVelocity -
                    (int)(args.Movement & ButtonDown.DOWN) * Player.MaxVelocity;
            }
        }
    }
}