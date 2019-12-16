using PolyWars.Client.Logic.Utility;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace PolyWars.Client.Logic {
    /// <summary>
    /// Controlls game logic with consideration to clients ability to render frames
    /// </summary>
    class Ticker {
        public static int Fps { get; set; }
        private bool stopTickerThread;
        private const double baselineFps = 1000d / 60; // miliseconds per frame at 60 fps 
        private double frameDeltaTime;
        private Thread thread;
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

        public void Stop() {
            stopTickerThread = true;
        }

        public void onFrameDisplayed(object sender, EventArgs e) {
            frameDisplayed = true;
        }

        private void Tick() {
            GameController.tickTimer.Restart();
            while(!stopTickerThread) {
                waitForNextFrame(GameController.tickTimer);
                frameDeltaTime = DeltaTime(GameController.tickTimer.Elapsed.TotalMilliseconds);
                if(GameController.DebugFrameTimings) {
                    FrameDebugTimer.startFrameTimer();
                }
                frameDisplayed = false;
                try {
                    InputController.applyInput();
                    GameController.calculateFrame(frameDeltaTime);
                } catch(TaskCanceledException) { // occurs when the application is closed, and the UIDispatcher thread no longer exists. Does not need to be handled
                    
                }
                GameController.tickTimer.Restart();
                if(GameController.DebugFrameTimings) {
                    FrameDebugTimer.stopFrameTimer();
                }
            }
        }

        private void waitForNextFrame(Stopwatch tickTimer) {
            if(GameController.DebugFrameTimings) {
                FrameDebugTimer.startFpsLimitTimer();
            }
            while(!frameDisplayed || tickTimer.Elapsed.TotalMilliseconds <= 1000d / 480) {
                Thread.Sleep(1);
            }
            if(GameController.DebugFrameTimings) {
                FrameDebugTimer.stopFpsLimitTimer();
            }
        }
    }
}
