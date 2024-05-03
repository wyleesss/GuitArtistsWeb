namespace GuitArtists.Models
{
    public class LessonViewModel
    {
        public string SectionName { get; set; }
        public string Name { get; set; }
        public string Appendix { get; set; }
        public string Body { get; set; }
        public string? Video { get; set; }
        public string? ImagePath { get; set; }
        public int Likes { get; set; }
    }
}
