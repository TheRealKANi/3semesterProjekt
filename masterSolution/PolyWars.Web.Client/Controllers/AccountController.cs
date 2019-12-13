using PolyWars.Web.Client.Models;
using PolyWars.Web.Client.PolyWarsWebClientService;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PolyWars.Web.Client.Controllers {
    public class AccountController : Controller {

        //URL: /Account/Register
        public ActionResult Register() {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model) {
            if(ModelState.IsValid) {
                WebClientServiceClient client = new WebClientServiceClient();
                UserData userData = new UserData() {
                    userName = model.UserName,
                    password = model.Password
                };
                if(client.register(userData)) {
                    if(client.login(userData)) {
                        return RedirectToAction("Index", "Home");
                    }
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
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl) {
            if(!ModelState.IsValid) {
                return View(model);
            }
            WebClientServiceClient client = new WebClientServiceClient();
            UserData userData = new UserData() {
                userName = model.UserName,
                password = model.Password
            };

            if(client.login(userData)) {

                return RedirectToLocal(returnUrl);
            } else {

                return View(model);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl) {
            if(Url.IsLocalUrl(returnUrl)) {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}