using System;
using System.Web.Mvc;
using RestaurantsExample.Managers;

namespace RestaurantsExample.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Restaurants Example Home Page";
            RestaurantManager rm = new RestaurantManager();
            rm.GetOpenRestaurants(DateTime.Now);

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
