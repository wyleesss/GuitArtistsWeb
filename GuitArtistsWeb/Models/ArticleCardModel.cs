using FullDB.Data.Entity;

namespace GuitArtists.Models
{
    public class ArticleCardModel
    {
        public string UserLogin { get; }
        public string UserAvatarUrl { get; }
        public string Name { get; }
        public int Likes { get; }
        public string? Image { get; }
        public string CreatedAt { get; }

        public ArticleCardModel(Post post)
        {
            var createdAtDate = post.CreatedAt;

            UserLogin = post.User.Login;
            UserAvatarUrl = post.User.AvatarUrl;
            Name = post.Name;
            Likes = post.Likes;
            Image = post.Image;
            CreatedAt = createdAtDate.Day.ToString() + "." + createdAtDate.Month.ToString() + "." + createdAtDate.Year.ToString();
        }
    }
}
