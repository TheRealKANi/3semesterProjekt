﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolyWars.API.Model.Interfaces {
    public interface IBullet {
        string ID { get; }
        string PlayerID { get; }
        IMoveable BulletShip { get; }
        int Damage { get; }

    }
}
