using PolyWars.Logic;
using PolyWars.Network;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace PolyWars.Model {
    class Ticker {
        public static int Fps { get; set; }
        private bool stopTickerThread;
        Thread thread;
        public static bool frameDisplayed;

        public Ticker() {
            thread = new Thread(Tick) {
                IsBackground = true
            };
            frameDisplayed = false;

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

                // TODO DEBUG - Starts Frame Timer
                Logic.Utility.FrameDebugTimer.startFrameTimer();

                waitForNextFrame( GameController.tickTimer );
                frameDisplayed = false;
                try {
                    InputController.applyInput();
                    GameController.calculateFrame();
                    //GameController.calculateFps();
                    GameController.tickTimer.Restart();
                } catch(TaskCanceledException) {
                    // TODO Do we need to handle this?
                }
                // TODO DEBUG - Stops Frame Timer
                Logic.Utility.FrameDebugTimer.stopFrameTimer();
            }
        }

        private void waitForNextFrame(Stopwatch tickTimer) {
            TimeSpan sleepDuration = new TimeSpan(5);

            while(!frameDisplayed /* && tickTimer.Elapsed.TotalMilliseconds <= (1000d / 120) */) {
                Thread.Sleep(sleepDuration);
            }
            tickTimer.Restart();
        }
    }
}
