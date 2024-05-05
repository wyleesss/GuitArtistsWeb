using FullDB.Data;
using FullDB.Data.Entity;
using GuitArtistsWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GuitArtistsWeb.Controllers
{
    public class ProfileController : Controller
    {
        private readonly AppDbContext _context;

        public ProfileController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index([FromRoute] string? login)
        {
            User? user;
            ProfileModel model;
            List<ArticleCardModel> articleCard = new();
            if (login == "me")
            {
                if (!User.Identity.IsAuthenticated)
                    return RedirectToAction("Index", "PageNotFound");

                user = _context.GetUserByID(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

                if (user == null)
                    return RedirectToAction("Index", "PageNotFound");
                var posts = _context.Posts.Where(a => a.UserId == user.Id).ToList();
                foreach (var buff in posts)
                {
                    articleCard.Add(new(buff));
                }
                model = new(user, true);
                model.Posts = articleCard;
            }
            else
            {
                if (login == null || _context.GetUserByLogin(login) == null)
                    return RedirectToAction("Index", "PageNotFound");

                user = _context.GetUserByLogin(login);

                if (User.Identity.IsAuthenticated)
                {
                    if (user.Id.ToString() == User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value)
                    {
                        return Index("me");
                    }
                }
                var posts = _context.Posts.Where(a => a.UserId == user.Id).ToList();
                foreach (var buff in posts)
                {
                    articleCard.Add(new(buff));
                }
                model = new(user);
                model.Posts = articleCard;
            }

            return View("~/Views/profile/index.cshtml", model);
        }
    }
}
