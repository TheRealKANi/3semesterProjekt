using PolyWars.API.Model;
using PolyWars.API.Model.Interfaces;
using PolyWars.API.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PolyWars.ServerClasses {
    class CollisionStrategy : ICollisionStrategy{
        public Collision checkCollision(IShape shape, IEnumerable<IMoveable> moveables) {
            // get rough collision zone
            IRenderable renderable = shape.Renderable;
            double minX = 0;
            double maxX = 0;
            double minY = 0;
            double maxY = 0;
            for(int i = 0; i < renderable.Vertices; i++) {
                Point p = shape.Polygon.Points[i];
                minX = Math.Min(minX, p.X);
                maxX = Math.Max(maxX, p.X);
                minY = Math.Min(minY, p.Y);
                maxY = Math.Max(maxY, p.Y);
            }
            
            Func<IShape, string, string, double> sb = (s, d, o) => { //(s)hape (b)ound helper. (s = shape, d = dimension, o = operator
                return (d == "x" ? s.Ray.CenterPoint.X : s.Ray.CenterPoint.Y) + (o == "+" ? s.Renderable.Width : -1 * s.Renderable.Width) / 2;
            };
            IEnumerable<IMoveable> roughCollisions = moveables.Where(s => 
                sb(s.Shape, "x", "+") > minX && sb(s.Shape, "x", "+") < maxX || // right-most side of moveable within x-bounds of shape
                sb(s.Shape, "x", "-") < maxX && sb(s.Shape, "x", "-") > minX);  // left-most side of moveable within x-bounds of shape
            
            if(roughCollisions.Count() > 0) {
                roughCollisions = roughCollisions.Where(s =>
                    sb(s.Shape, "y", "+") > minY && sb(s.Shape, "x", "+") < maxY || // top-most side of moveable within y-bounds of shape
                    sb(s.Shape, "y", "-") < maxY && sb(s.Shape, "x", "-") > minY);  // bottom-most side of moveable within y-bounds of shape
            }

            if(roughCollisions.Count() > 0) {
                foreach(IMoveable moveable in roughCollisions) {
                    IRay mRay = moveable.Shape.Ray;
                    
                    // angle to moveable
                    double moveableToShapeAngle = mRay.Angle - Math.Atan((mRay.CenterPoint.X - shape.Ray.CenterPoint.X) / (mRay.CenterPoint.Y - shape.Ray.CenterPoint.Y));

                    // get moveable vertice closest to shape
                    double anglePerVertice = 360d / moveable.Shape.Renderable.Vertices;

                }
            }
            return new Collision();
        }
    }
}
