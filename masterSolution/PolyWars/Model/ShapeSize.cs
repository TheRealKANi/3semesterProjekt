using PolyWars.API;

namespace PolyWars.Model {
    public class ShapeSize : IShapeSize {
        /// <summary>
        /// You set how big the shape is.
        /// </summary>
        public ShapeSize( int width, int height ) {
            Width = width;
            Height = height;
        }

        public int Width { get; set; }
        public int Height { get; set; }
    }
}
