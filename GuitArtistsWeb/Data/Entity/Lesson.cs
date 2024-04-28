using GuitArtists.Helpers;
using GuitArtists.Models;
using GuitArtists.Helpers;
using GuitArtists.Models;
using System.ComponentModel.DataAnnotations;

namespace FullDB.Data.Entity
{
    public class Lesson
    {
        public Guid Id { get; set; }

        public Section Section { get; set; }
        public Guid SectionId { get; set; }

        public int? Number { get; set; }

        [Required]
        [StringLength(255)]
        public string Slug { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public string Appendix { get; set; }

        public string Body { get; set; }

        public string? Video { get; set; }

        public int Likes { get; set; }

        public string? Image { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public Lesson()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public Lesson(LessonCreateModel model)
        {
            Id = Guid.NewGuid();
            SectionId = model.SectionID;
            Slug = SlugGenerator.Generate(model.Name);
            Name = model.Name;
            Appendix = model.Appendix;
            Body = model.Body;
            Video = model.Video;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
