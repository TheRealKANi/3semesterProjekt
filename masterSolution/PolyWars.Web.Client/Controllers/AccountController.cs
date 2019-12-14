using PolyWars.Web.Client.Models;
using PolyWars.Web.Client.PolyWarsWebClientService;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PolyWars.Web.Client.Controllers {
    public class AccountController : Controller {

        public static string UserName { get; set; }
        //URL: /Account/Register
        public ActionResult Register() {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model) {
            if(ModelState.IsValid) {
                WebClientServiceClient client = new WebClientServiceClient();
                UserData userData = new UserData() {
                    userName = model.UserName,
                    password = model.Password
                };
                if(client.register(userData)) {
                    if(client.login(userData)) {
                        UserName = userData.userName;
                        return RedirectToAction("Index", "Home");
                    } else {
                        UserName = null;
                    }
                } else {
                    UserName = null;
                }
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }


        //URL: /Account/Login
        public ActionResult Login() {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl) {
            if(!ModelState.IsValid) {
                return View(model);
            }
            WebClientServiceClient client = new WebClientServiceClient();
            UserData userData = new UserData() {
                userName = model.UserName,
                password = model.Password
            };

            if(client.login(userData)) {
                UserName = userData.userName;
                return RedirectToLocal(returnUrl);
            } else {
                UserName = null;
                return View(model);
            }
        }

        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff() {
            UserName = null;
            return RedirectToAction("Index", "Home");
        }

        private ActionResult RedirectToLocal(string returnUrl) {
            if(Url.IsLocalUrl(returnUrl)) {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}