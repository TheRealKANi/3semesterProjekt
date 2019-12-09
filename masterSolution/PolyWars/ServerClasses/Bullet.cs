using PolyWars.API.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolyWars.ServerClasses {
    class Bullet : IBullet {
        public Bullet(string id, IMoveable bulletShip, int damage, string playerID) {
            ID = id;
            BulletShip = bulletShip;
            Damage = damage;
            PlayerID = playerID;
        }
        public string ID { get; private set; }
        public IMoveable BulletShip { get; set; }
        public int Damage { get; private set; }
        public string PlayerID{ get; private set; }
    }
}
