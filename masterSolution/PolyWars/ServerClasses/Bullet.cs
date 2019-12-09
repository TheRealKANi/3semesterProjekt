using PolyWars.API.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolyWars.ServerClasses {
    class Bullet : IBullet {
        public Bullet(string id, IShape shape, int damage) {
            ID = id;
            Shape = shape;
            Damage = damage;
        }
        public string ID { get; private set; }
        public IShape Shape { get; private set; }
        public int Damage { get; private set; }
    }
}
