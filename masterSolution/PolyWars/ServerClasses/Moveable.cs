using PolyWars.API;
using PolyWars.API.Model.Interfaces;
using PolyWars.API.Strategies;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PolyWars.ServerClasses {
    class Moveable : IMoveable {
        public double Velocity { get; set; }
        public double MaxVelocity { get; set; }
        public double RPM { get; set; }
        public double MaxRPM { get; set; }
        public IShape Shape { get; set; }
        public IMoveStrategy Mover { get; set; }

        public Moveable() {

        }

        public Moveable(double velocity, double maxVelocity, double rPM, double maxRPM, IShape shape, IMoveStrategy mover) {
            Velocity = velocity;
            MaxVelocity = maxVelocity;
            RPM = rPM;
            MaxRPM = maxRPM;
            Shape = shape;
            Mover = mover;
        }
        public void Move(double deltaTime) {
            Mover.Move(this, deltaTime);
        }
        //public void MoveToNewRay() {
        //    // Get the center of current triangle
        //    PointCollection pc = Shape.Polygon.Points;
        //    double sumX = 0;
        //    double sumY = 0;
            
        //    foreach(Point point in pc) {
        //        sumX += point.X;
        //        sumY += point.Y;
        //    }

        //    double centerX = sumX / pc.Count;
        //    double centerY = sumY / pc.Count;
            
        //    // Get distance from current center to Ray Center

        //    Point c = Shape.Ray.CenterPoint;

        //    double deltaX = c.X - centerX;
        //    double deltaY = c.Y - centerY;

        //    //Move the triangle to the new ray

        //    for(int i = 0; i < Shape.Polygon.Points.Count; i++) {
        //        Point p = pc[i];
        //        p.Offset(deltaX, -1 * deltaY);
        //        pc[i] = p;
        //    }
        //    //Shape.Polygon.RenderTransform = new RotateTransform(-1 * Shape.Ray.Angle, c.X, c.Y);
        //}
    }
}
