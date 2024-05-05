using FullDB.Data;
using FullDB.Data.Entity;
using GuitArtistsWeb.Helpers;
using GuitArtistsWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GuitArtistsWeb.Controllers
{
    public class CreateArticleController : Controller
    {
        private readonly AppDbContext _context;

        public CreateArticleController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (User.Claims.FirstOrDefault(c => c.Type == "isEmailConfirmed").Value != "True")
                return RedirectToAction("Index", "EmailConfirmation");

            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "PageNotFound");

            return View();
        }

        [HttpPost]
        public IActionResult Index(CreateArticleModel model)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "PageNotFound");

            if (User.Claims.FirstOrDefault(c => c.Type == "isEmailConfirmed").Value != "True")
                return RedirectToAction("Index", "EmailConfirmation");

            if (ModelState.IsValid)
            {
                ImageService instr = new("Data/Images");
                User user = _context.GetUserByID(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
                Post post = new(model, user);

                try
                {
                    if (model.Image != null)
                    {
                        post.Image = instr.SaveImage(model.Image, post.Id);
                    }
                    _context.Posts.Add(post);
                    user.Posts.Add(post);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    model.State = ex.Message;
                    return View("~/Views/CreateArticle/Index.cshtml", model);
                }
                return Redirect($"~/articles/{user.Login}/{post.Slug}");
            }

            return View("~/Views/CreateArticle/Index.cshtml", model);
        }
    }
}
