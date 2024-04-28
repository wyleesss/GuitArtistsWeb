using AutoMapper;
using GuitArtists.Helpers;
using GuitArtists.Models;
using FullDB.Data;
using FullDB.Data.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HtmlAgilityPack;

namespace GuitArtists.Controllers
{
    [AdminAccessFilter]
    public class AdminController : Controller
    {
        private IMapper _mapper;
        private readonly AppDbContext _context;
        public AdminController(IMapper mapper, AppDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        public IActionResult Main()
        {
            return View("~/Views/Admin/Main.cshtml");
        }

        public IActionResult Lessons()
        {
            List<Section> buff = _context.Sections.Where(a => a.ParentId == null).Include(s => s.Children).ThenInclude(c => c.Lessons).Include(s => s.Lessons).ToList();
            return View("~/Views/Admin/Lessons.cshtml", buff);
        }
        public IActionResult LessonCreate()
        {
            LessonCreateViewModel model = new();
            model.Sections = _mapper.Map<List<SectionNameViewModel>>(_context.Sections);
            return View("~/Views/Admin/LessonCreate.cshtml", model);
        }

        // Дія, яка обробляє POST-запит на створення нового елемента
        [HttpPost]
        public IActionResult LessonCreate(LessonCreateModel _model)
        {
            LessonCreateViewModel model = new();
            model.Sections = _mapper.Map<List<SectionNameViewModel>>(_context.Sections);
            if (ModelState.IsValid)
            {
                ImageService instr = new("Data/Images");
                Lesson lesson = new(_model);
                try
                {
                    lesson.Image = instr.SaveImage(_model.Image, lesson.Id);
                    _context.Lessons.Add(lesson);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    model.State = ex.Message;
                    return View("~/Views/Admin/LessonCreate.cshtml", model);
                }
                model.State = "Урок успішно створений";
                return View("~/Views/Admin/LessonCreate.cshtml", model);
            }
            model.State = "Помилка введення!";
            return View("~/Views/Admin/LessonCreate.cshtml", model);
        }

        public IActionResult SectionCreate()
        {
            SectionCreateViewModel model = new();
            model.sections = _mapper.Map<List<SectionNameViewModel>>(_context.Sections.Where(a => a.ParentId == null).ToList());
            return View("~/Views/Admin/SectionCreate.cshtml", model);
        }
        [HttpPost]
        public IActionResult SectionCreate(string name, string description, string sectionId)
        {
            int num;
            if (sectionId == "parent")
            {
                num = _context.Sections.ToList().Count + 1;
            }
            else
            {
                var buff = _context.Sections.FirstOrDefault(a => a.Id == Guid.Parse(sectionId));
                if (buff.Children != null)
                    num = buff.Children.Count + 1;
                else num = 1;
            }
            SectionCreateViewModel model = new();
            Section sec = new Section(name, description, sectionId, num);
            try
            {
                _context.Sections.Add(sec);
                _context.SaveChanges();
                model.sections = _mapper.Map<List<SectionNameViewModel>>(_context.Sections);
                model.state = "Дані успішно додані";
                return View("~/Views/Admin/SectionCreate.cshtml", model);
            }
            catch (Exception ex)
            {
                model.sections = _mapper.Map<List<SectionNameViewModel>>(_context.Sections);
                model.state = ex.Message;
                return View("~/Views/Admin/SectionCreate.cshtml", model);
            }
        }

        public IActionResult Parse()
        {
            return View("~/Views/Admin/Parse.cshtml");
        }

        [HttpPost]
        public IActionResult Parse(string url)
        {
            try
            {
                HtmlWeb web = new HtmlWeb();

                HtmlDocument document = web.Load(url);
                var songname = document.DocumentNode.SelectNodes("//h2[@class='text-2xl font-semibold tracking-tight']");
                var avtorname = document.DocumentNode.SelectNodes("//p[@class='text-sm']");
                var body = document.DocumentNode.SelectNodes("//div[@class='flex flex-col lg:flex-row']");
                string SongName = "", AvtorName = "";
                foreach (var name in songname)
                {
                    SongName = name.InnerText.Trim();
                }
                foreach (var name in avtorname)
                {
                    AvtorName = name.InnerText.Trim();
                }
                string slug = SlugGenerator.Generate(SongName + " " + AvtorName);
                Chord acord = new(slug, songname[0].InnerText.Trim(), avtorname[0].InnerText.Trim(), body[0].InnerHtml);

                _context.Chords.Add(acord);
                _context.SaveChanges();
                return View("~/Views/Admin/Parse.cshtml", (object)"Дані успішно записані");
            }
            catch (Exception ex)
            {
                return View("~/Views/Admin/Parse.cshtml", (object)"Пісня уже є в бд");
            }
        }
    }
}
