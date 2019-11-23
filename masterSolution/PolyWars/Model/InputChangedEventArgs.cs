using PolyWars.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolyWars.Model
{
    class InputChangedEventArgs : EventArgs
    {

        public ButtonDown Movement { get; private set; }

        public InputChangedEventArgs(ButtonDown movement) {
            this.Movement = movement;
        }
    }
}
