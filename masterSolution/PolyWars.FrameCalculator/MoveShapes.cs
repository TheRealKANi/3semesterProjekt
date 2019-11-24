﻿using PolyWars.API;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PolyWars.FrameCalculator {
    public static class MoveShapes {

        public static void move( IShape shape, double timeFactor ) {
            shape.Angle += shape.RPS * 6 * timeFactor;
            shape.Polygon.RenderTransform = new RotateTransform( -1 * shape.Angle, shape.CenterPoint.X, shape.CenterPoint.Y );

            double offsetX = shape.Velocity * Math.Sin( shape.Angle * Math.PI / 180 ) * timeFactor;
            double offsetY = shape.Velocity * Math.Cos( shape.Angle * Math.PI / 180 ) * timeFactor;

            Point cp = shape.CenterPoint;
            cp.Offset( offsetX, offsetY );
            shape.CenterPoint = cp;

            PointCollection pc = shape.Polygon.Points;
            for( int i = 0; i < pc.Count; i++ ) {
                Point p = pc[i];
                p.Offset( offsetX, offsetY );
                pc[i] = p;
            }
        }

        public static void collisionDetection( Canvas canvas, IShape player ) {
            // Start iteration from second child in canvas, player is the first child.

            foreach( Polygon canvasChild in canvas.Children ) {
                // If child is not the player and is not hidden on the canvas
                if( !canvasChild.Points.Equals( player.Polygon.Points ) ) {
                    if( !canvasChild.IsVisible.Equals( Visibility.Hidden ) ) {
                        if( canvasChild.RenderedGeometry.Bounds.IntersectsWith( player.Polygon.RenderedGeometry.Bounds ) ) {
                            canvasChild.Visibility = Visibility.Hidden;
                        }
                    }
                }
            }
        }
    }
}
