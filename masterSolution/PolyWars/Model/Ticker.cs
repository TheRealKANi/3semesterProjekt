using PolyWars.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace PolyWars.Model {
    class Ticker {
        public static int Fps { get; set; }
        private bool stopTickerThread;
        Thread thread;

        public Ticker() {
            thread = new Thread( Tick ) {
                IsBackground = true
            };
            
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

        private void Tick() {
            // TODO Remove/Refactor try catch block

            
            GameController.tickTimer.Restart();
            while( !stopTickerThread ) {

                // TODO DEBUG - Starts Frame Timer
                Logic.Utility.FrameDebugTimer.startFrameTimer();
                InputController.Instance.applyInput();

                //waitForNextFrame( GameController.tickTimer );
                try {
                    //TickerEventHandler?.Invoke( this, new TickEventArgs( DeltaTime( GameController. ) ) );
                    GameController.calculateFrame();
                    //GameController.calculateFps();
                    GameController.tickTimer.Restart();
                } catch( TaskCanceledException ) {
                    // TODO Do we need to handle this?
                }
                // TODO DEBUG - Stops Frame Timer
                Logic.Utility.FrameDebugTimer.stopFrameTimer();
            }
        }

        private void waitForNextFrame( Stopwatch tickTimer ) {
            TimeSpan sleepDuration = new TimeSpan(1);
            while( tickTimer.Elapsed.TotalMilliseconds <= ( 935d / 120 ) ) {
                Thread.Sleep( sleepDuration  );
            }
        }
    }
}
