using PolyWars.API.Model;
using PolyWars.API.Model.Interfaces;
using System.Collections.Generic;

namespace PolyWars.API.Strategies {
    public interface ICollisionStrategy {
        Collision checkCollision(IShape shape, IEnumerable<IMoveable> moveable);
    }
}
