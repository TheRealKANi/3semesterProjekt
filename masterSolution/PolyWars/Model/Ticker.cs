using PolyWars.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace PolyWars.Model {
    class Ticker {
        public EventHandler<PropertyChangedEventArgs> CanvasChangedEventHandler;
        public EventHandler<TickEventArgs> TickerEventHandler;
        public static int Fps { get; set; }
        private bool stopTickerThread;
        int ticks;
        DateTime tickLast;
        DateTime tickStart;
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

            double DeltaTime( double tickTime2 ) {
                return 1d + ( ( 600_000_000 - tickTime2 ) / 600_000_000 );
            }


            while( !stopTickerThread ) {
                // TODO DEBUG - Starts Frame Timer
                Logic.Utility.FrameDebugTimer.startFrameTimer();
                tickStart = DateTime.Now;
                double tickTime = ( tickStart - tickLast ).Ticks;
                InputController.Instance.applyInput();

                TickerEventHandler?.Invoke( this, new TickEventArgs( DeltaTime( tickTime ) ) );
                //waitForNextFrame( tickLast );
                try {
                    CanvasChangedEventHandler?.Invoke( this, new PropertyChangedEventArgs( "ArenaCanvas" ) );
                    tickLast = DateTime.Now;

                } catch( TaskCanceledException ) {
                    // TODO Do we need to handle this?
                }
                // TODO DEBUG - Stops Frame Timer
                Logic.Utility.FrameDebugTimer.stopFrameTimer();
            }
        }

        private void waitForNextFrame( DateTime lastTick ) {
            int msDelay = 2;
            while( ( DateTime.Now.Ticks - lastTick.Ticks ) <= ( 10_000_000d / 60 ) ) {
                ticks = ( int ) ( ( 1d / 60 ) - ( ( double ) ( DateTime.Now - lastTick ).Ticks / 2 ) );
                if( ticks > msDelay * 10_000 ) {
                    Thread.Sleep( ticks >= 0 ? ticks : 0 );
                } else if( ticks > 0 ) {
                    Thread.Sleep( 1 );
                }
            }
        }
    }
}
