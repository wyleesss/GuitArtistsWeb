using AutoMapper;
using FullDB.Data;
using FullDB.Data.Entity;
using GuitArtists.Helpers;
using GuitArtists.Models;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            List<FullDB.Data.Entity.Section> buff = _context.Sections.Where(a => a.ParentId == null).Include(s => s.Children).ThenInclude(c => c.Lessons).Include(s => s.Lessons).ToList();
            return View("~/Views/Admin/Lessons.cshtml", buff);
        }
        [Route("Admin/Lessons/{id}")]
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
                ImageService instr = new("Data\\Images");
                Lesson lesson = new(_model);
                try
                {
                    lesson.Image = instr.SaveImage(_model.Image, lesson.Id);
                    var section = _context.Sections.FirstOrDefault(a => a.Id == lesson.SectionId);
                    if (section.Lessons == null)
                    {
                        lesson.Number = 1;
                    }
                    else lesson.Number = section.Lessons.Count + 1;
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
            FullDB.Data.Entity.Section sec = new FullDB.Data.Entity.Section(name, description, sectionId, num);
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

        [Route("Admin/{type}/Edit/{id}")]
        public IActionResult Edit(string type, string id)
        {
            if (type == "Section")
            {
                SectionEditViewModel sectionModel = new();
                var secBuff = _context.Sections.FirstOrDefault(a => a.Id == Guid.Parse(id));
                sectionModel.Id = secBuff.Id;
                sectionModel.Name = secBuff.Name;
                sectionModel.Description = secBuff.Description;
                sectionModel.ParentId = secBuff.ParentId;

                sectionModel.sections = _context.Sections.Where(a => a.ParentId == null && a.Id != Guid.Parse(id)).ToList();
                return View("~/Views/Admin/SectionEdit.cshtml", sectionModel);
            }
            LessonEditViewModel model = new LessonEditViewModel();
            var buff = _context.Lessons.FirstOrDefault(a => a.Id == Guid.Parse(id));
            model.Id = buff.Id;
            model.Name = buff.Name;
            model.Appendix = buff.Appendix;
            model.Body = buff.Body;
            model.SectionID = buff.SectionId;
            model.Video = buff.Video;
            model.Sections = _context.Sections.ToList();

            return View("~/Views/Admin/LessonEdit.cshtml", model);
        }

        [HttpPost]
        [Route("Admin/Section/Edit/Confirm")]
        public IActionResult EditSectionConfirm(SectionEditViewModel model)
        {
            if (model != null)
            {
                var buff = _context.Sections.Find(model.Id);
                buff.Name = model.Name;
                buff.Slug = SlugGenerator.Generate(model.Name);
                buff.Description = model.Description;
                buff.ParentId = model.ParentId;
                buff.UpdatedAt = DateTime.UtcNow;
                _context.SaveChanges();
                model.sections = _context.Sections.Where(a => a.Id != model.Id && a.ParentId == null).ToList();
                model.state = "Дані успішно замінені та збережені!";
                return View("~/Views/Admin/SectionEdit.cshtml", model);
            }
            model.state = "Помилка вхідних даних";
            return View("~/Views/Admin/SectionEdit.cshtml", model);
        }

        [HttpPost]
        [Route("Admin/Lesson/Edit/Confirm")]
        public IActionResult EditLessonConfirm(LessonEditViewModel model)
        {
            if (model != null)
            {
                var buff = _context.Lessons.Find(model.Id);
                buff.SectionId = model.SectionID;
                buff.Slug = SlugGenerator.Generate(model.Name);
                buff.Name = model.Name;
                buff.Appendix = model.Appendix;
                buff.Body = model.Body;
                buff.Video = model.Video;
                if (model.Image.Length > 0)
                {
                    ImageService instr = new("Data\\Images");
                    buff.Image = instr.SaveImage(model.Image, model.Id);
                }
                var section = _context.Sections.FirstOrDefault(a => a.Id == buff.SectionId);
                if (section.Lessons != null)
                {
                    buff.Number = section.Lessons.Count + 1;
                }
                else buff.Number = 1;
                buff.UpdatedAt = DateTime.UtcNow;
                _context.SaveChanges();
                model.Sections = _context.Sections.ToList();
                model.state = "Дані успішно замінені та збережені!";
                return View("~/Views/Admin/LessonEdit.cshtml", model);
            }
            model.state = "Помилка вхідних даних";
            return View("~/Views/Admin/LessonEdit.cshtml", model);
        }

        [Route("Admin/{type}/Delete/{id}")]
        public IActionResult Delete(string type, string id)
        {
            if (type == "Section")
            {
                SectionDeleteConfirmViewModel model = new();
                var buff = _context.Sections.FirstOrDefault(a => a.Id == Guid.Parse(id));
                model.section = _mapper.Map<SectionNameViewModel>(buff);
                return View("~/Views/Admin/SectionDeleteConfirm.cshtml", model);
            }
            LessonDeleteConfirmViewModel Lmodel = new();
            var lesson = _context.Lessons.FirstOrDefault(a => a.Id == Guid.Parse(id));
            Lmodel.lesson = lesson;

            return View("~/Views/Admin/LessonDeleteConfirm.cshtml", Lmodel);
        }

        [Route("Admin/{type}/Delete/Confirm")]
        [HttpPost]
        public IActionResult DeleteConfirm(string type, string sectionId)
        {
            if (type == "Section")
            {
                SectionDeleteConfirmViewModel model = new();
                try
                {
                    var buff = _context.Sections.FirstOrDefault(a => a.Id == Guid.Parse(sectionId));
                    var listSubSection = _context.Sections.Where(a => a.ParentId == Guid.Parse(sectionId));
                    var listLesson = _context.Lessons.Where(a => a.SectionId == Guid.Parse(sectionId));
                    _context.Sections.RemoveRange(listSubSection);
                    _context.Sections.Remove(buff);
                    _context.Lessons.RemoveRange(listLesson);
                    _context.SaveChanges();
                    model.section = _mapper.Map<SectionNameViewModel>(buff);
                    model.state = "Секція успішно вилучена";
                    return View("~/Views/Admin/SectionDeleteConfirm.cshtml", model);
                }
                catch (Exception ex)
                {
                    model.state = ex.Message;
                    return View("~/Views/Admin/SectionDeleteConfirm.cshtml", model);
                }
            }
            LessonDeleteConfirmViewModel Lmodel = new();
            try
            {
                var lesson = _context.Lessons.FirstOrDefault(a => a.Id == Guid.Parse(sectionId));
                _context.Remove(lesson);
                _context.SaveChanges();
                Lmodel.lesson = lesson;
                Lmodel.state = "Урок успішно вилучений";
                return View("~/Views/Admin/LessonDeleteConfirm.cshtml", Lmodel);
            }
            catch (Exception ex)
            {
                Lmodel.state = ex.Message;
                return View("~/Views/Admin/LessonDeleteConfirm.cshtml", Lmodel);
            }
        }
    }
}
