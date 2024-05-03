using FullDB.Data;
using FullDB.Data.Entity;
using GuitArtists.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace GuitArtistsWeb.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly AppDbContext _context;

        public ArticlesController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Search()
        {
            var posts = _context.Posts.Include(u => u.User).OrderBy(p => p.Likes).ToList();
            List<ArticleCardModel> models = new();

            foreach(var post in posts)
            {
                models.Add(new(post));
            }

            return View(models);
        }

        public IActionResult Index([FromRoute] string? userLogin, [FromRoute] string? slug)
        {
            if (userLogin == null || slug == null)
                return RedirectToAction("Index", "PageNotFound");

            Post? post;
            User? user;
            ArticleModel model;

            if ((user = _context.GetUserByLogin(userLogin)) == null)
                return RedirectToAction("Index", "PageNotFound");

            if (_context.GetUserByLogin(userLogin) == null || (post =_context.GetPost(user.Id.ToString(), slug)) == null)
                return RedirectToAction("Index", "PageNotFound");

            if (!User.Identity.IsAuthenticated)
            {
                model = new(post, false);
            }
            else
            {
                model = new(post,
                    _context.GetUserByID(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value)
                    .LikedPostsID.Any(p => p == post.Id),
                    _context.GetUserByID(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value).Login == user.Login);
            }

            return View("~/Views/articles/index.cshtml", model);
        }

        [HttpPost]
        [Route("like-article/{userLogin}/{id}")]
        public IActionResult LikeArticle(string userLogin, string id)
        {
            if (userLogin == null || id == null)
                return RedirectToAction("Index", "PageNotFound");

            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Login");

            User user = _context.GetUserByID(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            User postAuthor = _context.GetUserByLogin(userLogin);
            bool liked = false;

            if (user.LikedPostsID.Any(g => g.ToString() == id))
            {
                user.LikedPostsID.Remove(Guid.Parse(id));
                postAuthor.Posts.FirstOrDefault(p => p.Id.ToString() == id).Likes--;
                _context.SaveChanges();
            }
            else
            {
                user.LikedPostsID.Add(Guid.Parse(id));
                postAuthor.Posts.FirstOrDefault(p => p.Id.ToString() == id).Likes++;
                _context.SaveChanges();
                liked = true;
            }

            ArticleModel model = new(postAuthor.Posts.FirstOrDefault(p => p.Id.ToString() == id), liked,
                _context.GetUserByID(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value).Login == userLogin);

            return View("~/Views/articles/index.cshtml", model);
        }

        [HttpPost]
        [Route("delete-article/{userLogin}/{id}")]
        public IActionResult DeleteArticle(string userLogin, string id)
        {
            if (userLogin == null || id == null)
                return RedirectToAction("Index", "PageNotFound");

            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "PageNotFound");

            User user = _context.GetUserByID(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

            if (user.Login != userLogin)
                return RedirectToAction("Index", "PageNotFound");

            _context.Posts.Remove(_context.Posts.FirstOrDefault(p => p.Id.ToString() == id));
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}
