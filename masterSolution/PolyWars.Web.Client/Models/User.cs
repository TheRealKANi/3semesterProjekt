using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PolyWars.Web.Client.Models {
    public class User {
        public string Name { get; set; }
        public string ID { get; private set; }
        public double Wallet { get; set; }
    }
}