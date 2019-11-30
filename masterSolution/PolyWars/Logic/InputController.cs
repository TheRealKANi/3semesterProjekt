using PolyWars.API;
using PolyWars.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PolyWars.Logic {

    public class InputController {

        private InputController() { }
        private static InputController instance;

        // Ensures that show tick average only runs once
        private bool hasRun = false;

        public static InputController Instance {
            get {
                if( instance == null ) {
                    instance = new InputController();
                }
                return instance;
            }
        }
        /// <summary>
        /// Grabs input from a player
        /// </summary>
        public Input Input { get; private set; }

        /// <summary>
        /// Initializes input
        /// </summary>

        public static Player Player { get; private set; }

        public void initInput( Player player ) {
            Input = new Input();
            Player = player;
        }


        public void applyInput() {
            ButtonDown input = Input.queryInput();
            IMoveable shape = Player.PlayerShip;
            shape.RPM =
                ( ( ( int ) ( input & ButtonDown.LEFT ) >> 2 ) * shape.MaxRPM ) -
                ( ( int ) ( input & ButtonDown.RIGHT ) * shape.MaxRPM );

            shape.Velocity =
                ( ( ( int ) ( input & ButtonDown.UP ) >> 1 ) * shape.MaxVelocity ) -
                ( ( ( int ) ( input & ButtonDown.DOWN ) >> 3 ) * shape.MaxVelocity );


            // TODO Remove Debug Key
            if( ( ( int ) ( input & ButtonDown.DEBUG ) >> 5 ) > 0 && hasRun == false ) {
                Utility.FrameDebugTimer.outpuFrameTimerResults();
                Utility.FrameDebugTimer.outpuMoveShapeTimerResults();
                Utility.FrameDebugTimer.outpuCollisionTimerResults();
                hasRun = true;
            }
            
        }
    }
}