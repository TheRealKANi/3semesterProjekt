using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PolyWars.Web.Client.Controllers {
    public class AccountController : Controller {

        //URL: /Account/Register
        public ActionResult Register() {
            return View();
        }

        //URL: /Account/Login
        public ActionResult Login() {
            return View();
        }
    }
}