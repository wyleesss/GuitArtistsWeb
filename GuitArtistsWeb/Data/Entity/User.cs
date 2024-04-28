using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FullDB.Data.Entity
{
    public class User
    {
        public Guid Id { get; set; }

        public string? GoogleId { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }

        [Required]
        public bool IsEmailConfirmed { get; set; }

        public string? PasswordHash { get; set; }

        public string? PasswordSalt { get; set; }

        [StringLength(255)]
        public string Login { get; set; }

        [StringLength(255)]
        public string? FirstName { get; set; }

        [StringLength(255)]
        public string? LastName { get; set; }

        public string? AvatarUrl { get; set; }

        public string? EmailConfirmToken { get; set; }
        public string? ForfotToken { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public List<Post> Posts { get; set; }

        public List<Comment> Comments { get; set; }

        public User(Guid id, string email, string? passwordHash, string? passwordSalt, string login, string? firstName, string? lastName, string? avatarUrl,
            string? googleId = null, string? emailConfirmToken = null, string? forfotToken = null)
        {
            Id = id;
            Email = email;
            IsEmailConfirmed = false;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
            Login = login;
            FirstName = firstName;
            LastName = lastName;
            AvatarUrl = avatarUrl;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            Posts = new List<Post>();
            Comments = new List<Comment>();
            GoogleId = googleId;
            EmailConfirmToken = emailConfirmToken;
            ForfotToken = forfotToken;
        }
    }

}
