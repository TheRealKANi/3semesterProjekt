﻿using PolyWars.API;
using PolyWars.API.Model.Interfaces;
using PolyWars.API.Strategies;
using PolyWars.Logic;
using PolyWars.Logic.Utility;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

namespace PolyWars.ServerClasses {
    public class MoveStrategy : IMoveStrategy {
        public void Move(IMoveable moveable, decimal deltaTime) {
            FrameDebugTimer.startMoveShapeTimer();
            IShape shape = moveable.Shape;
            shape.Ray.Angle += (double) ((decimal) (moveable.RPM * 360 / (60 * 60)) * deltaTime);

            double offsetX = (double) ((decimal) (moveable.Velocity * Math.Sin(shape.Ray.Angle * Math.PI / 180)) * deltaTime);
            double offsetY = (double) ((decimal) (moveable.Velocity * Math.Cos(shape.Ray.Angle * Math.PI / 180)) * deltaTime);

            Point cp = shape.Ray.CenterPoint;
            cp.Offset(offsetX, offsetY);
            shape.Ray.CenterPoint = cp;

            PointCollection pc = shape.Polygon.Points;
            for(int i = 0; i < pc.Count; i++) {
                Point p = pc[i];
                p.Offset(offsetX, offsetY);
                pc[i] = p;
            }
            UIDispatcher.Invoke(() => {
                shape.Polygon.RenderTransform = new RotateTransform(-1 * shape.Ray.Angle, shape.Ray.CenterPoint.X, shape.Ray.CenterPoint.Y); //TODO only if it actually rotates?
            });
            FrameDebugTimer.stopMoveShapeTimer();
            CollisionDetection.resourceCollisionDetection();
        }
    }
    public class MoveOpponentStrategy : MoveStrategy, IMoveStrategy {
        public new void Move(IMoveable moveable, decimal deltaTime) {
            UIDispatcher.Invoke(() => {
                PointCollection pc = moveable.Shape.Polygon.Points;
                Point currentCenter = pc[pc.Count - 2];
                Point newCenter = moveable.Shape.Ray.CenterPoint;
                if(currentCenter.X != newCenter.X || currentCenter.Y != newCenter.Y) {
                    // Get distance from current center to Ray Center
                    double deltaX = newCenter.X - currentCenter.X;
                    double deltaY = newCenter.Y - currentCenter.Y;

                    //Move the triangle to the new ray
                    for(int i = 0; i < moveable.Shape.Polygon.Points.Count; i++) {
                        Point p = pc[i];
                        p.Offset(deltaX, deltaY);
                        pc[i] = p;
                    } 
                }
            });
            base.Move(moveable, deltaTime);
        }
    }
}
