using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PolyWars.API {
    public interface IRay {
        int ID { get; set; }
        Point CenterPoint { get; set; }
        double Angle { get; set; }
    }
}
