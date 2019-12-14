using PolyWars.Web.Client.PolyWarsWebClientService;
using System.Web.Mvc;

namespace PolyWars.Web.Client.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            return View();
        }

        public ActionResult Leaderboard() {
            WebClientServiceClient client = new WebClientServiceClient();

            if(AccountController.UserName != null) {
                ViewBag.Message = "Track your highscore, and compete with others!";
                ViewBag.leaderboard = client.GetLeaderBoard();
            } else {
                ViewBag.Message = "Your Are Not Allowed To See The LeaderBoard, Please Login First";
            }

            return View();
        }

        public ActionResult TheTeam() {
            ViewBag.Message = "Project made by";

            return View();
        }
    }
}