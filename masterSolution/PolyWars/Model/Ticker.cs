using PolyWars.API;
using PolyWars.Logic;
using PolyWars.Logic.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PolyWars.Model {
    class Ticker {
        public EventHandler<PropertyChangedEventArgs> CanvasChangedEventHandler;
        public EventHandler<TickEventArgs> TickerEventHandler;
        public static int Fps { get; set; }
        private bool stopTickerThread;
        long ticks;
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

            const double stps = 10_000_000; // system ticks per second (this is the system clocks ticks, not the games ticks)
            const double gtpsBase = 60; // game ticks per second. Movement per game tick is based on this number. Movement per tick is calculated from a baseline of fpsBase ticks per second
            const double stpgt = stps / gtpsBase; // system ticks per game tick base

            double DeltaTime( long ticks ) {
                decimal test = ( decimal ) ticks / (decimal)stps / (decimal)stpgt ;
                return 1;
            }
            
            while( !stopTickerThread ) {
                tickStart = new DateTime(Stopwatch.GetTimestamp());
                InputController.Instance.applyInput();

                waitForNextFrame( lastTick );            
                try {
                    CanvasChangedEventHandler?.Invoke( this, new PropertyChangedEventArgs( "ArenaCanvas" ) );
                    long tickTime = ( tickStart - tickLast ).Ticks;
                    TickerEventHandler?.Invoke( this, new TickEventArgs( DeltaTime( tickTime ) ) );
                    tickLast = new DateTime(Stopwatch.GetTimestamp());
                } catch( TaskCanceledException ) {
                    // TODO Do we need to handle this?
                }
            }
        }

        private void waitForNextFrame( DateTime lastTick ) {
            int msDelay = 2;
            while( ( Stopwatch.GetTimestamp() - lastTick.Ticks ) <= ( 10_000_000d / 60 ) ) {
                ticks = ( 1d / 60  -  ( double ) ( Stopwatch.GetTimestamp() - lastTick.Ticks ) / 2  );
                if( ticks > msDelay * 10_000_000 ) {
                    Thread.Sleep( ticks >= 0 ? ticks : 0 );
                } else if( ticks > 0 ) {
                    Thread.Sleep( 1 );
                }
            }
        }
    }
}
