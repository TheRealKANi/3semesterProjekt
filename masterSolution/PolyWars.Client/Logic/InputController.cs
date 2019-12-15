using PolyWars.API;
using PolyWars.API.Model.Interfaces;
using PolyWars.Client.Logic.Utility;
using PolyWars.Network;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PolyWars.Client.Logic {
    /// <summary>
    /// Setup for input handling and debug output
    /// </summary>
    public static class InputController {
        private static bool shootFlag;
        private static Stopwatch shootTimer;
        private static bool hasRun = false; // Ensures that show tick average only runs once
        public static Input Input { get; private set; }

        static InputController() {
            Input = new Input();
            shootTimer = new Stopwatch();
            shootFlag = false;
        }

        public static void applyInput() {
            ButtonDown input = Input.queryInput();
            if(!GameController.IsPlayerDead) {
                IMoveable shape = GameController.Player.PlayerShip;

                shape.RPM =
                    ((int) (input & ButtonDown.LEFT) >> 2) * shape.MaxRPM -
                    (int) (input & ButtonDown.RIGHT) * shape.MaxRPM;

                shape.Velocity =
                    ((int) (input & ButtonDown.UP) >> 1) * shape.MaxVelocity -
                    ((int) (input & ButtonDown.DOWN) >> 3) * shape.MaxVelocity;


                bool shootPressed = (int) (input & ButtonDown.SHOOT) >> 4 > 0;
                if(shootPressed && !shootFlag && (!shootTimer.IsRunning || shootTimer.Elapsed.TotalSeconds >= 1)) {
                    shootTimer.Restart();
                    Task.Run(() => NetworkController.GameService.playerShoots(Constants.standardShotDamage));
                    shootFlag = true;
                } else if(!shootPressed) {
                    shootFlag = false;
                }
            }


            // Only run timeings output when DebugFrameTimings is true
            if(GameController.DebugFrameTimings && (int) (input & ButtonDown.DEBUG) >> 5 > 0 && hasRun == false) {
                FrameDebugTimer.outpuFrameTimerResults();
                FrameDebugTimer.outputMoveShapeTimerResults();
                FrameDebugTimer.outputCollisionTimerResults();
                FrameDebugTimer.outputFpsLimitTimerResults();
                hasRun = true;
            }
        }
    }
}