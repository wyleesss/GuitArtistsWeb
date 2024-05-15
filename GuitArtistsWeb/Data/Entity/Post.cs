using GuitArtistsWeb.Helpers;
using GuitArtistsWeb.Models;
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

        public Post()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public Post(CreateArticleModel model, User user)
        {
            Id = Guid.NewGuid();
            UserId = user.Id;
            User = user;
            Slug = SlugGenerator.Generate(model.Name);
            Name = model.Name;
            Appendix = model.Appendix;
            Body = model.Body;
            Likes = 0;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public Post(Guid id, User user, string slug, string name, string appendix, string body, int likes, string? image, DateTime createdAt, DateTime updatedAt)
        {
            Id = id;
            UserId = user.Id;
            User = user;
            Slug = slug;
            Name = name;
            Appendix = appendix;
            Body = body;
            Likes = likes;
            Image = image;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
