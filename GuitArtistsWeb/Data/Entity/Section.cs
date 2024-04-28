using GuitArtists.Helpers;
using GuitArtists.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FullDB.Data.Entity
{
    public class Section
    {
        public Guid Id { get; set; }

        public int? Number { get; set; }

        [Required]
        [StringLength(255)]
        public string Slug { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public string Description { get; set; }
        public Guid? ParentId { get; set; }

        [ForeignKey("ParentId")]
        public virtual Section Parent { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public List<Section> Children { get; set; }

        public List<Lesson> Lessons { get; set; }


        public Section()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public Section(string name, string des, string sectionid, int number)
        {
            Id = Guid.NewGuid();
            if (sectionid != "parent")
            {
                ParentId = Guid.Parse(sectionid);
            }
            Name = name;
            Description = des;
            Number = number;
            Slug = SlugGenerator.Generate(name);
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }

}
