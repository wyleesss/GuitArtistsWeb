using Microsoft.AspNetCore.Mvc;

namespace GuitArtistsWeb.Controllers
{
    public class PageNotFound : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
