namespace GuitArtists.Models
{
    public class LessonCreateModel
    {
        public Guid SectionID { get; set; }
        public string Name { get; set; }
        public string Appendix { get; set; }
        public string Body { get; set; }
        public string? Video { get; set; }
        public IFormFile? Image { get; set; } // Це властивість для отримання файлу зображення
    }

}
