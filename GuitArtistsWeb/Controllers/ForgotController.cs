using FullDB.Data;
using FullDB.Data.Entity;
using GuitArtistsWeb.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static GuitArtistsWeb.Helpers.HashingHelper;

namespace GuitArtistsWeb.Controllers
{
    public class ForgotController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly AppDbContext _context;

        public ForgotController(IEmailSender emailSender, AppDbContext context)
        {
            _emailSender = emailSender;
            _context = context;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "PageNotFound");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(ForgotModel model)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "PageNotFound");

            if (ModelState.IsValid)
            {
                User? user;

                if ((user = _context.GetUserByEmail(model.EmailLogin)) == null)
                    user = _context.GetUserByLogin(model.EmailLogin);

                string token = Guid.NewGuid().ToString();
                user.ForfotToken = Hash(token);
                _context.SaveChanges();

                Response.Cookies.Append("ForgotToken", Hash(token), new CookieOptions
                {
                    Expires = DateTime.Now.AddMinutes(5),
                    Secure = true
                });

                var recoveryLink = Url.Action(
                   "RecoveryPage",
                   "Forgot",
                   new { userId = user.Id, token = token },
                   protocol: Request.Scheme);

                string htmlBody = "<!DOCTYPE html>\r\n<html>\r\n<head>\r\n" +
                    "<title>Відновлення паролю</title>\r\n</head>\r\n<body>\r\n" +
                    "<h4>це повідомлення дійсне тільки на протязі 5 хвилин</h4>\r\n" +
                    "<p>Для відновлення доступу до аккаунту натисніть кнопку нижче</p>\r\n" +
                    $"<a href='{recoveryLink}'" +
                    "style=\"display: inline-block; padding: 10px 20px; background-color: #007bff; color: #fff; text-decoration: none;\">Відновити</a>\r\n</body>\r\n</html>";

                await _emailSender.SendEmailAsync(user.Email, "Відновлення паролю", htmlBody);
                TempData["EmailForgotSended"] = true;

                return RedirectToAction("Index", "Home");
            }

            return View("~/Views/Forgot/index.cshtml", model);
        }

        public IActionResult RecoveryPage(string userId, string token)
        {
            if (Request.Cookies["ForgotToken"] == null)
                return RedirectToAction("Index", "PageNotFound");

            if (userId == null || token == null)
                return RedirectToAction("Index", "PageNotFound");

            var user = _context.GetUserByID(userId);

            if (user == null)
                return RedirectToAction("Index", "PageNotFound");

            var currentUserId = user.Id.ToString();

            if (currentUserId == null || currentUserId != userId)
                return RedirectToAction("Index", "PageNotFound");

            if (user.ForfotToken != Hash(token) || user.ForfotToken == null)
                return RedirectToAction("Index", "PageNotFound");

            return RedirectToAction("Index", "Recovery", new { id = userId });
        }
    }
}
