using PolyWars.Web.Client.PolyWarsWebClientService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PolyWars.Web.Client.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Leaderboard()
        {
            WebClientServiceClient client = new WebClientServiceClient();

            ViewBag.Message = "Track your highscore, and compete with others!";
            ViewBag.leaderboard = client.GetLeaderBoard();

            return View();
        }

        public ActionResult TheTeam()
        {
            ViewBag.Message = "Project made by";

            return View();
        }
    }
}