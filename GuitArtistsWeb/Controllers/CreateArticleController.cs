﻿using FullDB.Data;
using FullDB.Data.Entity;
using GuitArtists.Helpers;
using GuitArtists.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GuitArtists.Controllers
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
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "PageNotFound");

            return View();
        }

        [HttpPost]
        public IActionResult Index(CreateArticleModel model)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "PageNotFound");

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
