using PolyWars.API;
using PolyWars.API.Model.Interfaces;
using PolyWars.API.Strategies;
using PolyWars.Logic.Utility;
using System;
using System.Windows;
using System.Windows.Media;

namespace PolyWars.ServerClasses {
    class MoveStrategy : IMoveStrategy {
        public void Move(IMoveable moveable, decimal deltaTime) {
            FrameDebugTimer.startMoveShapeTimer();
            IShape shape = moveable.Shape;
            shape.Ray.Angle += (double) ((decimal) (moveable.RPM * 360 / (60 * 60)) * deltaTime);


            double offsetX = (double) ((decimal) (moveable.Velocity * Math.Sin(shape.Ray.Angle * Math.PI / 180)) * deltaTime);
            double offsetY = (double) ((decimal) (moveable.Velocity * Math.Cos(shape.Ray.Angle * Math.PI / 180)) * deltaTime);

            Point cp = shape.Ray.CenterPoint;
            cp.Offset(offsetX, offsetY);
            shape.Ray.CenterPoint = cp;

            shape.Polygon.RenderTransform = new RotateTransform(-1 * shape.Ray.Angle, shape.Ray.CenterPoint.X, shape.Ray.CenterPoint.Y); //TODO only if it actually rotates?

            PointCollection pc = shape.Polygon.Points;
            for(int i = 0; i < pc.Count; i++) {
                Point p = pc[i];
                p.Offset(offsetX, offsetY);
                pc[i] = p;
            }
            FrameDebugTimer.stopMoveShapeTimer();
            CollisionDetection.resourceCollisionDetection();
        }
    }
}
