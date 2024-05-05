using GuitArtistsWeb.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using FullDB.Data;
namespace GuitArtistsWeb.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;

        public LoginController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "PageNotFound");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginModel? model)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "PageNotFound");

            if (ModelState.IsValid)
            {
                var user = _context.GetUserByEmail(model.Email);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Login),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.GivenName, user.FirstName == null ? "" : user.FirstName + " " + user.LastName),
                    new Claim("isEmailConfirmed", user.IsEmailConfirmed.ToString()),
                    new Claim("photo", user.AvatarUrl == null ? "" : user.AvatarUrl)
                };

                var userIdentity = new ClaimsIdentity(claims, "login");
                var userPrincipal = new ClaimsPrincipal(userIdentity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal);

                if (user.IsEmailConfirmed == false)
                    TempData["ShowEmailConfirmationModal"] = true;

                return RedirectToAction("Index", "Home");
            }

            return View("~/Views/login/index.cshtml", model);
        }
    }
}
