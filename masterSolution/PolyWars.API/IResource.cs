﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace PolyWars.API {
    public interface IResource {
        IShape Shape { get; }
        double Value { get; }
    }
}
