using PolyWars.API.Model.Interfaces;
using PolyWars.API.Strategies;
using PolyWars.Client.Logic;
using PolyWars.Client.Logic.Utility;
using System;
using System.Windows;
using System.Windows.Media;

namespace PolyWars.Client.Model {
    public class MoveStrategy : IMoveStrategy {
        /// <summary>
        /// Moves a entitys polygon, angle and centerpoint to 
        /// a new location based on velocity and rpm
        /// </summary>
        /// <param name="moveable">The Moveable to move</param>
        /// <param name="deltaTime">The timefactor to scale move to</param>
        public void Move(IMoveable moveable, double deltaTime) {
            if(GameController.DebugFrameTimings) {
                FrameDebugTimer.startMoveShapeTimer();
            }

            IShape shape = moveable.Shape;
            double xCalculation = moveable.Velocity * Math.Sin(shape.Ray.Angle * Math.PI / 180) * deltaTime;
            double yCalculation = moveable.Velocity * Math.Cos(shape.Ray.Angle * Math.PI / 180) * deltaTime;
            shape.Ray.Angle += moveable.RPM / 10 * deltaTime; // RPM * 360 / (60 * 60) = Rounds per Frame => RPM * 6 / 60 = RPM / 10

            bool checkX = true;
            bool checkY = true;
            PointCollection pc = null;
            UIDispatcher.Invoke(() => {
                pc = shape.Polygon.Points;
                for(int i = 0; pc != null && i < pc.Count - 3 && (checkX || checkY); i++) { // last 3 points of the polygon is the header
                    if(!(xCalculation + pc[i].X >= 1 && xCalculation + pc[i].X <= ArenaController.ArenaBoundWidth - 1)) { checkX = false; }
                    if(!(yCalculation + pc[i].Y >= 1 && yCalculation + pc[i].Y <= ArenaController.ArenaBoundHeight - 1)) { checkY = false; }
                }
            });

            Point cp = shape.Ray.CenterPoint;
            double offsetX = checkX ? xCalculation : 0;
            double offsetY = checkY ? yCalculation : 0;
            cp.Offset(offsetX, offsetY);
            shape.Ray.CenterPoint = cp;

            UIDispatcher.Invoke(() => {
                for(int i = 0; i < pc.Count; i++) {
                    Point p = pc[i];
                    p.Offset(offsetX, offsetY);
                    pc[i] = p;
                }
                shape.Polygon.RenderTransform = new RotateTransform(-1 * shape.Ray.Angle, shape.Ray.CenterPoint.X, shape.Ray.CenterPoint.Y);
            });
            if(GameController.DebugFrameTimings) {
                FrameDebugTimer.stopMoveShapeTimer();
            }
            CollisionDetection.resourceCollisionDetection();
        }
    }


    public class MoveOpponentStrategy : MoveStrategy, IMoveStrategy {
        /// <summary>
        /// Moves an opponents complete polygon to new location 
        /// </summary>
        /// <param name="opponent">The opponent entity to move</param>
        /// <param name="deltaTime">The timefactor to scale move to</param>
        public new void Move(IMoveable opponent, double deltaTime) {
            UIDispatcher.Invoke(() => {
                PointCollection pc = opponent.Shape.Polygon.Points;
                Point currentCenter = pc[pc.Count - 2];
                Point newCenter = opponent.Shape.Ray.CenterPoint;
                if(currentCenter.X != newCenter.X || currentCenter.Y != newCenter.Y) {
                    // Get distance from current center to Ray Center
                    double deltaX = newCenter.X - currentCenter.X;
                    double deltaY = newCenter.Y - currentCenter.Y;

                    //Move the shape to the new ray location
                    for(int i = 0; i < opponent.Shape.Polygon.Points.Count; i++) {
                        Point p = pc[i];
                        p.Offset(deltaX, deltaY);
                        pc[i] = p;
                    }
                }
            });
            base.Move(opponent, deltaTime);
        }
    }
}
