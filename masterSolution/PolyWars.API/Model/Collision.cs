using PolyWars.API.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolyWars.API.Model {
    public enum CollisionType {
        RESOURCE,
        PLAYER,
        WALL
    }
    public class Collision {
        public CollisionType Type { get; set; }
        public IEnumerable<IMoveable> Colliders { get; set; }

    }
}
