using PolyWars.API;
using PolyWars.Model;

namespace PolyWars.Logic {

    public class InputController {

        private InputController() { }
        private static InputController instance;

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
            IShape shape = Player.Shape;
            shape.RPM =
                ( ( ( int ) ( input & ButtonDown.LEFT ) >> 2 ) * shape.MaxRPM ) -
                ( ( int ) ( input & ButtonDown.RIGHT ) * shape.MaxRPM );

            shape.Velocity =
                ( ( ( int ) ( input & ButtonDown.UP ) >> 1 ) * shape.MaxVelocity ) -
                ( ( ( int ) ( input & ButtonDown.DOWN ) >> 3 ) * shape.MaxVelocity );
        }
    }
}