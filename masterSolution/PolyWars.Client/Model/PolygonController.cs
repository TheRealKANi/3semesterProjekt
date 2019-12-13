using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace PolyWars.Model {
    class PolygonController {
        private static PolygonController polygonController;
        private List<Polygon> polygons;

        static PolygonController() {
            polygonController = new PolygonController();
            polygonController.polygons = new List<Polygon>();
        }

        //static Polygon this[int i] {
        //    get {
        //        return polygons[i];
        //    }
        //}
    }
}
