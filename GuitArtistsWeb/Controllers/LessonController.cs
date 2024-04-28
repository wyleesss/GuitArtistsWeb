using Microsoft.AspNetCore.Mvc;

namespace GuitArtists.Controllers
{
    public class LessonController : Controller
    {
        public IActionResult Create()
        {
            return View();
        }
    }
}
