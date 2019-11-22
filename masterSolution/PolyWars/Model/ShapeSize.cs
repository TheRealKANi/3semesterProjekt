using PolyWars.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolyWars.Model {
    class ShapeSize : IShapeSize {
        /// <summary>
        /// You set how big the shape is.
        /// </summary>
        public ShapeSize(int width, int height) {
            Width = width;
            Height = height;
        }

        public int Width { get; set; }
        public int Height { get; set; }
    }
}
