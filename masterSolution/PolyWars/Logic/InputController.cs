using PolyWars.Model;

namespace PolyWars.Logic {

    class InputController {

        private InputController() { }
        private static InputController INSTANCE;

        public static InputController Instance {
            get {
                if( INSTANCE == null ) {
                    INSTANCE = new InputController();
                }
                return INSTANCE;
            }
        }
        /// <summary>
        /// Grabs input from a player
        /// </summary>
        public Input Input { get; private set; }

        /// <summary>
        /// Initializes input
        /// </summary>

        public Triangle Player { get; private set; }

        public void initInput( Triangle player ) {
            Input = new Input();
            Player = player;
            //EventController.KeyboardEvents.InputChangedEventHandler += applyInput;
        }


        public void queryInput() {
            ButtonDown input = Input.checkInput();
            Player.RPS =
                ( ( ( int ) ( input & ButtonDown.LEFT ) >> 2 ) * Player.MaxRPS ) -
                ( ( int ) ( input & ButtonDown.RIGHT ) * Player.MaxRPS );

            Player.Velocity =
                ( ( ( int ) ( input & ButtonDown.UP ) >> 1 ) * Player.MaxVelocity ) -
                ( ( ( int ) ( input & ButtonDown.DOWN ) >> 3 ) * Player.MaxVelocity );
        }


        public void applyInput( object sender, InputChangedEventArgs args ) {
            if( sender is Input input ) {
                Player.RPS =
                    ( ( int ) ( args.Movement & ButtonDown.LEFT ) * Player.MaxRPS ) -
                    ( ( int ) ( args.Movement & ButtonDown.RIGHT ) * Player.MaxRPS );
                Player.Velocity =
                    ( ( int ) ( args.Movement & ButtonDown.UP ) * Player.MaxVelocity ) -
                    ( ( int ) ( args.Movement & ButtonDown.DOWN ) * Player.MaxVelocity );
            }
        }
    }
}