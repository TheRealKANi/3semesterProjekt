namespace PolyWars.Logic.Utility {
    static class ShapeCalculations {
        //public static void moveShape( IShape shape, decimal deltaTime ) {
        //    // TODO DEBUG - Starts moveShape Timer
        //    FrameDebugTimer.startMoveShapeTimer();
        //    shape.Angle += (double)((decimal)(shape.RPM * 360 / ( 60 * 60 )) * deltaTime);

        //    double offsetX = (double)((decimal)(shape.Velocity * Math.Sin( shape.Angle * Math.PI / 180 )) * deltaTime);
        //    double offsetY = (double)((decimal)(shape.Velocity * Math.Cos( shape.Angle * Math.PI / 180 )) * deltaTime);

        //    Point cp = shape.CenterPoint;
        //    cp.Offset( offsetX, offsetY );
        //    shape.CenterPoint = cp;

        //    shape.Polygon.RenderTransform = new RotateTransform( -1 * shape.Angle, shape.CenterPoint.X, shape.CenterPoint.Y ); //TODO only if it actually rotates?

        //    PointCollection pc = shape.Polygon.Points;
        //    for( int i = 0; i < pc.Count; i++ ) {
        //        Point p = pc[i];
        //        p.Offset( offsetX, offsetY );
        //        pc[i] = p;
        //    }
        //    // TODO DEBUG - Stops moveShape Timer
        //    FrameDebugTimer.stopMoveShapeTimer();
        //    //CollisionDetection.resourceCollisionDetection();
        //}
    }
}
