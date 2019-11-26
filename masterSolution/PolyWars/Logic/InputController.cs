using PolyWars.API;
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

        public static Player Player { get; private set; }

        public void initInput( Player player ) {
            Input = new Input();
            Player = player;
            //EventController.KeyboardEvents.InputChangedEventHandler += applyInput;
        }


        public void queryInput() {
            ButtonDown input = Input.checkInput();
            IShape shape = Player.Shape;
            shape.RPS =
                ( ( ( int ) ( input & ButtonDown.LEFT ) >> 2 ) * shape.MaxRPS ) -
                ( ( int ) ( input & ButtonDown.RIGHT ) * shape.MaxRPS );

            shape.Velocity =
                ( ( ( int ) ( input & ButtonDown.UP ) >> 1 ) * shape.MaxVelocity ) -
                ( ( ( int ) ( input & ButtonDown.DOWN ) >> 3 ) * shape.MaxVelocity );
        }
    }
}