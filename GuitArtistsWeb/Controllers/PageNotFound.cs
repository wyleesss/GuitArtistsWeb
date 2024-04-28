using Microsoft.AspNetCore.Mvc;

namespace GuitArtists.Controllers
{
    public class PageNotFound : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
