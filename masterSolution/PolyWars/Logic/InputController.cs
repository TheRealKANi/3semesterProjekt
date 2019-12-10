using PolyWars.API;
using PolyWars.API.Model.Interfaces;
using PolyWars.Network;
using System.Threading.Tasks;

namespace PolyWars.Logic {

    public static class InputController {
        private static bool shootFlag;
        static InputController() {
            Input = new Input();
            shootFlag = false;
        }

        // Ensures that show tick average only runs once
        private static bool hasRun = false;


        /// <summary>
        /// Grabs input from a player
        /// </summary>
        public static Input Input { get; private set; }

        /// <summary>
        /// Initializes input
        /// </summary>

        //public static IPlayer Player { get; private set; }

        public static void applyInput() {
            ButtonDown input = Input.queryInput();
            if(!GameController.IsPlayerDead) {
                IMoveable shape = GameController.Player.PlayerShip;
                shape.RPM =
                    (((int) (input & ButtonDown.LEFT) >> 2) * shape.MaxRPM) -
                    ((int) (input & ButtonDown.RIGHT) * shape.MaxRPM);

                shape.Velocity =
                    (((int) (input & ButtonDown.UP) >> 1) * shape.MaxVelocity) -
                    (((int) (input & ButtonDown.DOWN) >> 3) * shape.MaxVelocity);


                bool shootPressed = (int) (input & ButtonDown.SHOOT) >> 4 > 0;
                if(shootPressed && !shootFlag) {
                    Task.Run(() => NetworkController.GameService.playerShoots(Constants.standardShotDamage)).Wait();
                    shootFlag = true;
                } else if(!shootPressed) {
                    shootFlag = false;
                }
            }


            // TODO Remove Debug Key
            if(((int) (input & ButtonDown.DEBUG) >> 5) > 0 && hasRun == false) {
                Utility.FrameDebugTimer.outpuFrameTimerResults();
                Utility.FrameDebugTimer.outputMoveShapeTimerResults();
                Utility.FrameDebugTimer.outputCollisionTimerResults();
                Utility.FrameDebugTimer.outputFpsLimitTimerResults();
                hasRun = true;
            }
        }
    }
}