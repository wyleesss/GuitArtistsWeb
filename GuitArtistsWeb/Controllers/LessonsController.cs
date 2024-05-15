using FullDB.Data;
using GuitArtistsWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public IActionResult Lesson(string id)
        {
            var buff = _context.Lessons.FirstOrDefault(a => a.Id == Guid.Parse(id));
            LessonViewModel model = new();
            model.SectionName = (_context.Sections.FirstOrDefault(a => a.Id == buff.SectionId)).Name;
            model.Name = buff.Name;
            model.Appendix = buff.Appendix;
            model.Body = buff.Body;
            model.ImagePath = "/" + buff.Image;
            model.Video = buff.Video;
            return View("~/Views/Admin/LessonView.cshtml", model);
        }
    }
}
