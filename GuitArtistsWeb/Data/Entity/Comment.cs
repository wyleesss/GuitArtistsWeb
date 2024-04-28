using System.ComponentModel.DataAnnotations;

namespace FullDB.Data.Entity
{
    public class Comment
    {
        public Guid Id { get; set; }

        public Post Post { get; set; }

        public User User { get; set; }
        public Guid UserId { get; set; }

        public Guid PostId { get; set; }

        [Required]
        public string Body { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
