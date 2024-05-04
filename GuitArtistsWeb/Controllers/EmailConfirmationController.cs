using FullDB.Data;
using GuitArtists.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static GuitArtists.Helpers.ClaimUpdateHelper;
using static GuitArtists.Helpers.HashingHelper;

namespace GuitArtists.Controllers
{
    public class EmailConfirmationController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly AppDbContext _context;

        public EmailConfirmationController(IEmailSender emailSender, AppDbContext context)
        {
            _emailSender = emailSender;
            _context = context;
        }

        public IActionResult Index()
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == "isEmailConfirmed");

            if (claim == null || claim.Value == "True")
                return RedirectToAction("Index", "PageNotFound");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(EmailConfirmationModel model)
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == "isEmailConfirmed");

            if (claim == null || claim.Value == "True")
                return RedirectToAction("Index", "PageNotFound");

            if (ModelState.IsValid)
            {
                string token = Guid.NewGuid().ToString();
                string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

                Response.Cookies.Append("ConfirmToken", Hash(token), new CookieOptions
                {
                    Expires = DateTime.Now.AddMinutes(5),
                    Secure = true
                });

                var user = _context.GetUserByID(userId);
                user.Email = model.Email;
                user.EmailConfirmToken = Hash(token);

                var identity = (ClaimsIdentity)User.Identity;
                ClaimsPrincipal newPrincipal = new ClaimsPrincipal(GetNewIdentity(identity, ClaimTypes.Email, model.Email));
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, newPrincipal);

                _context.SaveChanges();

                var confirmationLink = Url.Action(
                    "EmailConfirm",
                    "EmailConfirmation",
                    new { userId = userId, token = token },
                    protocol: Request.Scheme);

                string htmlBody = "<!DOCTYPE html>\r\n<html>\r\n<head>\r\n" +
                    "<title>Підтвердження пошти</title>\r\n</head>\r\n<body>\r\n" +
                    "<h4>це повідомлення дійсне тільки на протязі 5 хвилин</h4>\r\n" +
                    "<p>Для завершення реєстрації натисніть кнопку нижче</p>\r\n" +
                    $"<a href='{confirmationLink}'" +
                    "style=\"display: inline-block; padding: 10px 20px; background-color: #007bff; color: #fff; text-decoration: none;\">Підтвердити пошту</a>\r\n</body>\r\n</html>";

                await _emailSender.SendEmailAsync(model.Email, "Підтвердження пошти", htmlBody);
                TempData["EmailConfirmationSended"] = true;

                return RedirectToAction("Index", "Home");
            }

            return View("~/Views/EmailConfirmation/index.cshtml", model);
        }

        public async Task<IActionResult> EmailConfirm(string userId, string token)
        {
            if (Request.Cookies["ConfirmToken"] == null)
                return RedirectToAction("Index", "PageNotFound");

            if (userId == null || token == null)
                return RedirectToAction("Index", "PageNotFound");

            var currentUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            if (currentUserId == null || currentUserId != userId)
                return RedirectToAction("Index", "PageNotFound");

            var user = _context.GetUserByID(userId);
            if (user.EmailConfirmToken != Hash(token) || user.EmailConfirmToken == null)
                return RedirectToAction("Index", "PageNotFound");

            user.EmailConfirmToken = null;
            user.IsEmailConfirmed = true;

            var identity = (ClaimsIdentity)User.Identity;
            ClaimsPrincipal newPrincipal = new ClaimsPrincipal(GetNewIdentity(identity, "isEmailConfirmed", "True"));
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, newPrincipal);

            _context.SaveChanges();

            TempData["EmailConfirmedSuccesfully"] = true;
            return RedirectToAction("Index", "Home");
        }
    }
}
