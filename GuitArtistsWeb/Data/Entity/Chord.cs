using System.ComponentModel.DataAnnotations;

namespace FullDB.Data.Entity
{
    public class Chord
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Slug { get; set; }

        [Required]
        [StringLength(255)]
        public string NameSong { get; set; }

        [Required]
        [StringLength(255)]
        public string NameAvtor { get; set; }


        public string Body { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        // Конструктор за замовчуванням (потрібний для Entity Framework)
        public Chord()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        // Конструктор для зручності створення об'єктів з параметрами
        public Chord(string slug, string nameSong, string nameAvtor, string body)
        {
            Id = Guid.NewGuid();
            Slug = slug;
            NameSong = nameSong;
            NameAvtor = nameAvtor;
            Body = body;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
