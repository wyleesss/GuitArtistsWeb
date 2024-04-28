using System.ComponentModel.DataAnnotations;

namespace FullDB.Data.Entity
{
    public class Post
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; }

        [Required]
        [StringLength(255)]
        public string Slug { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public string Appendix { get; set; }

        public string Body { get; set; }

        public int Likes { get; set; }

        public string? Image { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
