using FullDB.Data.Entity;

namespace GuitArtistsWeb.Models
{
    public class ArticleCardModel
    {
        public string Slug { get; set; }
        public string UserLogin { get; }
        public string UserAvatarUrl { get; }
        public string Name { get; }
        public int Likes { get; }
        public string? Image { get; }
        public string CreatedAt { get; }

        public ArticleCardModel(Post post)
        {
            var createdAtDate = post.CreatedAt;
            Slug = post.Slug;
            UserLogin = post.User.Login;
            UserAvatarUrl = post.User.AvatarUrl;
            Name = post.Name;
            Likes = post.Likes;
            Image = post.Image;
            CreatedAt = createdAtDate.Day.ToString() + "." + createdAtDate.Month.ToString() + "." + createdAtDate.Year.ToString();
        }
    }
}
