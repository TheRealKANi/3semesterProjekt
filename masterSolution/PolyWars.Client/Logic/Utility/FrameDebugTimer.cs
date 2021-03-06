﻿using System.Collections.Generic;
using System.Diagnostics;

namespace PolyWars.Client.Logic.Utility {
    public class FrameDebugTimer {

        private static Stopwatch frameTimer;
        private static List<double> frameTicks;

        private static Stopwatch collisionTimer;
        private static List<double> collisionTicks;

        private static Stopwatch moveShapeTimer;
        private static List<double> moveShapeTicks;

        private static Stopwatch fpsLimitTimer;
        private static List<double> fpsLimitTicks;

        /// <summary>
        /// Initializes timer
        /// </summary>
        static FrameDebugTimer() {
            frameTicks = new List<double>();
            frameTimer = new Stopwatch();

            collisionTicks = new List<double>();
            collisionTimer = new Stopwatch();

            moveShapeTicks = new List<double>();
            moveShapeTimer = new Stopwatch();

            fpsLimitTimer = new Stopwatch();
            fpsLimitTicks = new List<double>();
        }

        /// <summary>
        /// Starts the internal moveShapeTimer
        /// </summary>
        public static void startMoveShapeTimer() {
            moveShapeTimer.Restart();
        }

        /// <summary>
        /// Stops the internal moveShape timer and adds captured time to internal list
        /// </summary>
        public static void stopMoveShapeTimer() {
            if(moveShapeTimer.IsRunning) {
                moveShapeTimer.Stop();
                moveShapeTicks.Add(moveShapeTimer.Elapsed.TotalMilliseconds);
            }
        }


        /// <summary>
        /// Starts the internal collisionTimer
        /// </summary>
        public static void startCollisionTimer() {
            collisionTimer.Restart();
        }

        /// <summary>
        /// Stops the internal collisionTimer and adds captured time to internal list
        /// </summary>
        public static void stopCollisionTimer() {
            if(collisionTimer.IsRunning) {
                collisionTimer.Stop();
                collisionTicks.Add(collisionTimer.Elapsed.TotalMilliseconds);
            }
        }

        /// <summary>
        /// Starts the internal frameTimer
        /// </summary>
        public static void startFrameTimer() {
            frameTimer.Restart();
        }

        /// <summary>
        /// Stops the internal frameTimer and adds captured time to internal list
        /// </summary>
        public static void stopFrameTimer() {
            if(frameTimer.IsRunning) {
                frameTimer.Stop();
                frameTicks.Add(frameTimer.Elapsed.TotalMilliseconds);
            }
        }

        /// <summary>
        /// Starts the internal fpsLimiterTimer
        /// </summary>
        public static void startFpsLimitTimer() {
            fpsLimitTimer.Restart();
        }

        /// <summary>
        /// Stops the internal fpsLimiterTimer and adds captured time to internal list
        /// </summary>
        public static void stopFpsLimitTimer() {
            if(fpsLimitTimer.IsRunning) {
                fpsLimitTimer.Stop();
                fpsLimitTicks.Add(fpsLimitTimer.Elapsed.TotalMilliseconds);
            }
        }


        /// <summary>
        /// Outputs Data from time readings.
        /// If we are using high or low resolution
        /// Total runtime, number of frames, avg FPS rate,
        /// ms pr frame, how many % that is of 16.667 ms ( 60 FPS )
        /// and the number of resources on the Arena
        /// </summary>
        public static void outpuFrameTimerResults() {

            double totalTickTime = 0;
            foreach(double tickTime in frameTicks) {
                totalTickTime += tickTime;
            }
            double averageTickTime = totalTickTime / frameTicks.Count;
            double totalRunTimeInMs = totalTickTime;
            double averageFPS = frameTicks.Count / (totalRunTimeInMs / 1000);
            double maxFPSms = 1000d / 60d;

            Debug.WriteLine("\n      Frame Rendering Timing Results - Accuracy: " + (Stopwatch.IsHighResolution ? "High" : "Low"));
            Debug.WriteLine("        RunTime    : " + (totalRunTimeInMs / 1000).ToString("N4") + " s");
            Debug.WriteLine("        n Frames   : " + frameTicks.Count.ToString("N0") + " frames");
            Debug.WriteLine("        AVG FPS    : " + averageFPS.ToString("N2") + " FPS");
            Debug.WriteLine("        AVG Frame  : " + averageTickTime.ToString("N3") + " ms");
            Debug.WriteLine("        Using " + (averageTickTime / maxFPSms * 100d).ToString("N2") + "% of " + maxFPSms.ToString("N2") + "ms");
            Debug.WriteLine("        With " + GameController.Resources.Count + " resource(s)"); // How manny % from using the full 16.6 ms 
            Debug.WriteLine("");
        }

        /// <summary>
        /// Outputs Data from time readings.
        /// If we are using high or low resolution
        /// Total runtime, number of frames, avg FPS rate,
        /// ms pr frame, how many % that is of 16.667 ms ( 60 FPS )
        /// and the number of resources on the Arena
        /// </summary>
        public static void outputCollisionTimerResults() {
            double totalTickTime = 0;
            foreach(double tickTime in collisionTicks) {
                totalTickTime += tickTime;
            }
            double averageTickTime = totalTickTime / collisionTicks.Count;
            double totalRunTimeInMs = totalTickTime;
            double maxFPSms = 1000d / 60d;

            Debug.WriteLine("        RunTime      : " + (totalRunTimeInMs / 1000).ToString("N2") + " s");
            Debug.WriteLine("        n collisions : " + collisionTicks.Count.ToString("N0") + " frames");
            Debug.WriteLine("        AVG collision: " + averageTickTime.ToString("N3") + " ms");
            Debug.WriteLine("        Using " + (averageTickTime / maxFPSms * 100d).ToString("N2") + "% of " + maxFPSms.ToString("N2") + "ms");
            Debug.WriteLine("        Ticks pr second " + Stopwatch.Frequency.ToString("N0") + "\n");
        }

        /// <summary>
        /// Outputs Data from time readings.
        /// If we are using high or low resolution
        /// Total runtime, number of frames, avg FPS rate,
        /// ms pr frame, how many % that is of 16.667 ms ( 60 FPS )
        /// and the number of resources on the Arena
        /// </summary>
        public static void outputMoveShapeTimerResults() {
            double totalTickTime = 0;
            foreach(double tickTime in moveShapeTicks) {
                totalTickTime += tickTime;
            }
            double averageTickTime = totalTickTime / moveShapeTicks.Count;
            double totalRunTimeInMs = totalTickTime;
            double maxFPSms = 1000d / 60d;

            Debug.WriteLine("        RunTime     : " + (totalRunTimeInMs / 1000).ToString("N2") + " s");
            Debug.WriteLine("        n moveShape : " + moveShapeTicks.Count.ToString("N0") + " frames");
            Debug.WriteLine("        AVG Move    : " + averageTickTime.ToString("N3") + " ms");
            Debug.WriteLine("        Using " + (averageTickTime / maxFPSms * 100d).ToString("N2") + "% of " + maxFPSms.ToString("N2") + "ms");
            Debug.WriteLine("");
        }
        public static void outputFpsLimitTimerResults() {
            double totalTickTime = 0;
            foreach(double tickTime in fpsLimitTicks) {
                totalTickTime += tickTime;
            }
            double averageTickTime = totalTickTime / fpsLimitTicks.Count;
            double totalRunTimeInMs = totalTickTime;
            double maxFPSms = 1000d / 60d;

            Debug.WriteLine("        RunTime       : " + (totalRunTimeInMs / 1000).ToString("N2") + " s");
            Debug.WriteLine("        n waits       : " + fpsLimitTicks.Count.ToString("N0") + " waits");
            Debug.WriteLine("        AVG wait time : " + averageTickTime.ToString("N3") + " ms");
            Debug.WriteLine("        Using " + (averageTickTime / maxFPSms * 100d).ToString("N2") + "% of " + maxFPSms.ToString("N2") + "ms");
            Debug.WriteLine("");
        }
    }
}
