using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace GuitArtistsWeb.Controllers
{
    public class LogoutController : Controller
    {
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
