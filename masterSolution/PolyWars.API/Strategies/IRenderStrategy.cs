using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace PolyWars.API.Strategies {
    public interface IRenderStrategy {
        Polygon Render(IRenderable r, IRay ray);
    }
}
