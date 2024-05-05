using Microsoft.AspNetCore.Mvc;

namespace GuitArtistsWeb.Controllers
{
    public class LessonController : Controller
    {
        public IActionResult Create()
        {
            return View();
        }
    }
}
