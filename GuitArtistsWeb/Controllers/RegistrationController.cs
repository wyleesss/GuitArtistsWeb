using FullDB.Data;
using FullDB.Data.Entity;
using GuitArtists.Models;
using GuitArtistsWeb.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static GuitArtists.Helpers.HashingHelper;

namespace GuitArtists.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly AppDbContext _context;

        public RegistrationController(AppDbContext context)
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
        public async Task<IActionResult> Index(RegistrationModel? model)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "PageNotFound");

            if (ModelState.IsValid)
            {
                string hashSalt = GenerateSalt();

                User user = new(Guid.NewGuid(), model.Email, HashPassword(model.Password, hashSalt), hashSalt, LoginChecker.Change(model.Login), model.FirstName, model.LastName, "/default-user-logo.png");
                _context.AddUser(user);
                _context.SaveChanges();

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, LoginChecker.Change(model.Login)),
                    new Claim(ClaimTypes.Email, model.Email),
                    new Claim(ClaimTypes.GivenName, model.FirstName == null ? "" : model.FirstName + " " + model.LastName),
                    new Claim("isEmailConfirmed", user.IsEmailConfirmed.ToString()),
                    new Claim("photo", user.AvatarUrl == null ? "" : user.AvatarUrl)
                };

                var userIdentity = new ClaimsIdentity(claims, "login");
                var userPrincipal = new ClaimsPrincipal(userIdentity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal);

                TempData["ShowEmailConfirmationModal"] = true;

                return RedirectToAction("Index", "Home");
            }

            return View("~/Views/registration/index.cshtml", model);
        }
    }
}
