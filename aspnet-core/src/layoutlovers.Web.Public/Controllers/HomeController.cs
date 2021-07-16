using Microsoft.AspNetCore.Mvc;
using layoutlovers.Web.Controllers;

namespace layoutlovers.Web.Public.Controllers
{
    public class HomeController : layoutloversControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}