using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolyWars.Model
{
    class InputChangedEventArgs : EventArgs
    {

        public int Movement { get; private set; }

        public InputChangedEventArgs(int movement) {
            this.Movement = movement;
        }
    }
}
