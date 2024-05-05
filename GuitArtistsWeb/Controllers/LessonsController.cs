using FullDB.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace GuitArtistsWeb.Controllers
{

    public class LessonsController : Controller
    {
        private readonly AppDbContext _context;

        public LessonsController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Sections.Where(a => a.ParentId == null).Include(s => s.Children).ThenInclude(c => c.Lessons).Include(s => s.Lessons).ToList());
        }
    }
}
