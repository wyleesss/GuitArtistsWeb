using FullDB.Data;
using GuitArtistsWeb.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static GuitArtistsWeb.Helpers.HashingHelper;

namespace GuitArtistsWeb.Controllers
{
    public class RecoveryController : Controller
    {
        private readonly AppDbContext _context;

        public RecoveryController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index([FromQuery] string? id)
        {
            if (User.Identity.IsAuthenticated || id == null || _context.GetUserByID(id).ForfotToken == null)
                return RedirectToAction("Index", "PageNotFound");

            Response.Cookies.Append("UserId", id, new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(15)
            });

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(RecoveryModel model)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "PageNotFound");

            if (Request.Cookies["UserId"] == null)
                return RedirectToAction("Index", "PageNotFound");

            if (ModelState.IsValid)
            {
                var user = _context.GetUserByID(Request.Cookies["UserId"]);
                var salt = GenerateSalt();

                user.PasswordHash = HashPassword(model.Password, salt);
                user.PasswordSalt = salt;
                user.ForfotToken = null;
                _context.SaveChanges();

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

                TempData["PasswordRecoveredSuccesfully"] = true;
                Response.Cookies.Delete("UserId");

                return RedirectToAction("Index", "Home");
            }

            return View("~/Views/Recovery/index.cshtml", model);
        }
    }
}
