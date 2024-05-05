using FullDB.Data.Entity;

namespace GuitArtistsWeb.Models
{
    public class LessonEditViewModel
    {
        public Guid Id { get; set; }
        public Guid SectionID { get; set; }
        public string Name { get; set; }
        public string Appendix { get; set; }
        public string Body { get; set; }
        public string Video { get; set; }
        public IFormFile? Image { get; set; }
        public List<Section> Sections { get; set; }
        public string state { get; set; }
    }
}
