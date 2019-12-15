using PolyWars.API.Model.Interfaces;
using PolyWars.Client.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace PolyWars.Client.Model {
    class RoughCollition {
        /// <summary>
        /// Checks area around a Shape for collisions within a collection
        /// and a selection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="shape">The Shape to check around</param>
        /// <param name="shapeCollection">The collection of Shapes to check against</param>
        /// <param name="selector">The selector to filter collection from</param>
        /// <returns></returns>
        public IEnumerable<T> checkCollision<T>(IShape shape, IEnumerable<T> shapeCollection, Func<T, IShape> selector) {
            // get rough collision zone
            IRenderable renderable = shape.Renderable;
            double minX = 0;
            double maxX = 0;
            double minY = 0;
            double maxY = 0;
            for(int i = 0; i < renderable.Vertices; i++) {
                Point p = UIDispatcher.Invoke(() => shape.Polygon.Points[i]);
                minX = Math.Min(minX, p.X);
                maxX = Math.Max(maxX, p.X);
                minY = Math.Min(minY, p.Y);
                maxY = Math.Max(maxY, p.Y);
            }

            Func<IShape, string, string, double> sb = (s, d, o) => { //(s)hape (b)ound helper. (s = shape, d = dimension, o = operator
                return (d == "x" ? s.Ray.CenterPoint.X : s.Ray.CenterPoint.Y) + (o == "+" ? s.Renderable.Width : -1 * s.Renderable.Width) / 2;
            };
            IEnumerable<T> roughCollisions = shapeCollection.Where(s =>
                sb(selector(s), "x", "+") >= minX && sb(selector(s), "x", "+") <= maxX || // right-most side of moveable within x-bounds of shape
                sb(selector(s), "x", "-") <= maxX && sb(selector(s), "x", "-") >= minX);  // left-most side of moveable within x-bounds of shape

            if(roughCollisions.Count() > 0) {
                roughCollisions = roughCollisions.Where(s =>
                    sb(selector(s), "y", "+") >= minY && sb(selector(s), "x", "+") <= maxY || // top-most side of moveable within y-bounds of shape
                    sb(selector(s), "y", "-") <= maxY && sb(selector(s), "x", "-") >= minY);  // bottom-most side of moveable within y-bounds of shape
            }

            return roughCollisions;
        }
    }
}

