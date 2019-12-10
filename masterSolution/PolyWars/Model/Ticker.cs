using PolyWars.Logic;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace PolyWars.Model {
    class Ticker {
        public static int Fps { get; set; }
        private bool stopTickerThread;
        private const double baselineFps = 1000d / 60; // miliseconds per frame at 60 fps 
        private double frameDeltaTime;
        Thread thread;
        public static bool frameDisplayed { get; set; }

        public Ticker() {
            thread = new Thread(Tick) {
                IsBackground = true
            };
            frameDisplayed = false;

        }
        public double DeltaTime(double milliseconds) {
            return milliseconds / baselineFps;
        }
        public void Start() {
            stopTickerThread = false;
            thread.Start();
        }

        /// <summary>
        /// Stops the thread Ticker
        /// </summary>
        public void Stop() {
            stopTickerThread = true;
        }
        public void onFrameDisplayed(object sender, EventArgs e) {
            frameDisplayed = true;
        }

        private void Tick() {
            // TODO Remove/Refactor try catch block


            GameController.tickTimer.Restart();
            while(!stopTickerThread) {
                waitForNextFrame(GameController.tickTimer);
                frameDeltaTime = DeltaTime(GameController.tickTimer.Elapsed.TotalMilliseconds);
                // TODO DEBUG - Starts Frame Timer
                Logic.Utility.FrameDebugTimer.startFrameTimer();

                frameDisplayed = false;
                try {
                    InputController.applyInput();
                    GameController.calculateFrame(frameDeltaTime);
                } catch(TaskCanceledException) {
                    // TODO Do we need to handle this?
                }
                // TODO DEBUG - Stops Frame Timer
                GameController.tickTimer.Restart();
                Logic.Utility.FrameDebugTimer.stopFrameTimer();
            }
        }

        private void waitForNextFrame(Stopwatch tickTimer) {
            Logic.Utility.FrameDebugTimer.startFpsLimitTimer();
            while(!frameDisplayed || tickTimer.Elapsed.TotalMilliseconds <= (1000d / 480)) {
                Thread.Sleep(1);
            }
            Logic.Utility.FrameDebugTimer.stopFpsLimitTimer();
        }
    }
}
