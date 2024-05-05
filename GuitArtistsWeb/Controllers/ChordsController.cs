using AutoMapper;
using GuitArtistsWeb.Helpers;
using GuitArtistsWeb.Models;
using FullDB.Data;
using FullDB.Data.Entity;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;

namespace GuitArtistsWeb.Controllers
{
    public class ChordsController : Controller
    {
        private IMapper _mapper;
        private readonly AppDbContext _context;
        public ChordsController(IMapper mapper, AppDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public IActionResult Search()
        {
            return View("~/Views/Chords/Search.cshtml");
        }

        public IActionResult SearchResults(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Пошуковий запит порожній.");
            }

            var songs = _mapper.Map<List<SongViewModel>>(_context.FullTextSearch(query));
            return PartialView("~/Views/Chords/_SearchResultsPartial.cshtml", songs);
        }

        public IActionResult Song(string slug)
        {
            var song = _mapper.Map<SongViewModel>(_context.Chords.FirstOrDefault(a => a.Slug == slug));
            if (song == null)
            {
                return NotFound("Пісня не знайдена.");
            }

            return View("~/Views/Chords/Chords.cshtml", song);
        }
        public IActionResult Parse()
        {
            return View("~/Views/Chords/Parse.cshtml");
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
                return View("~/Views/Chords/Parse.cshtml", (object)"Дані успішно записані");
            }
            catch (Exception ex)
            {
                return View("~/Views/Chords/Parse.cshtml", (object)"Пісня уже є в бд");
            }
        }
    }
}
