using FullDB.Data;
using FullDB.Data.Entity;
using GuitArtists.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualBasic;
using System.Security.Claims;

namespace GuitArtists.Controllers
{
    public class ProfileController : Controller
    {
        private readonly AppDbContext _context;

        public ProfileController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index([FromRoute]string? login)
        {
            User? user;
            ProfileModel model;

            if (login == "me")
            {
                if (!User.Identity.IsAuthenticated)
                    return RedirectToAction("Index", "PageNotFound");

                user = _context.GetUserByID(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

                if (user == null)
                    return RedirectToAction("Index", "PageNotFound");

                model = new(user, true);
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

                model = new(user);
            }

            return View("~/Views/profile/index.cshtml", model);
        }
    }
}
