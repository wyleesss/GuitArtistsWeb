using FullDB.Data;
using FullDB.Data.Entity;
using GuitArtistsWeb.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GuitArtistsWeb.Controllers
{
    public class GoogleOAuthController : Controller
    {
        private readonly AppDbContext _context;

        public GoogleOAuthController(AppDbContext context)
        {
            _context = context;
        }

        public async Task Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                RedirectToAction("Index", "PageNotFound");
            }
            else
            {
                await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme,
                    new AuthenticationProperties
                    {
                        RedirectUri = Url.Action("GoogleResponse")
                    });
            }
        }

        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(claim => new
            {
                claim.Type,
                claim.Value
            });

            var id = result.Principal.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            var name = result.Principal.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname")?.Value;
            var surname = result.Principal.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname")?.Value;
            var photo = result.Principal.FindFirst("photo")?.Value;
            var email = result.Principal.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;
            string login = LoginChecker.Change(name);

            await HttpContext.SignOutAsync();

            if (name == null || surname == null)
            {
                name = null;
                surname = null;
            }

            User? user;

            if ((user = _context.GetUserByEmail(email)) != null)
            {
                if (user.GoogleId == null)
                {
                    TempData["ShowGoogleOAuthError"] = true;
                    return RedirectToAction("Index", "Home");
                }
            }

            if (_context.GetUserByLogin(login) != null)
            {
                login += surname;

                var oldLogin = login;

                while (_context.GetUserByLogin(login) != null)
                {
                    login = oldLogin + "-" + Guid.NewGuid().ToString().Substring(0, 3);
                }
            }

            if ((user = _context.GetUserByGoogleID(id)) == null)
            {
                user = new User(Guid.NewGuid(), email, null, null, login, name, surname, photo, id);
                user.IsEmailConfirmed = true;
                _context.AddUser(user);
                _context.SaveChanges();
            }

            var existingClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.FirstName == null ? "" : user.FirstName + " " + user.LastName),
                new Claim("isEmailConfirmed", "True"),
                new Claim("photo", user.AvatarUrl == null ? "" : user.AvatarUrl)
            };

            var userIdentity = new ClaimsIdentity(existingClaims, "login");
            var userPrincipal = new ClaimsPrincipal(userIdentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal);

            return RedirectToAction("Index", "Home");
        }
    }
}
