using FullDB.Data.Entity;

namespace GuitArtistsWeb.Models
{
    public class ArticleModel
    {
        public Guid Id { get; }
        public string UserLogin { get; }
        public string UserAvatarUrl { get; }
        public string Name { get; }
        public string Appendix { get; }
        public string Body { get; }
        public int Likes { get; }
        public string? Image { get; }
        public string CreatedAt { get; }
        public bool Liked { get; set; }
        //public List<Comment> Comments { get; }

        readonly public bool CurrentUserPost;
      
        public ArticleModel(Post post, bool liked, bool currentUserPost = false)
        {
            var createdAtDate = post.CreatedAt;
            Id = post.Id;
            UserLogin = post.User.Login;
            UserAvatarUrl = post.User.AvatarUrl;
            Name = post.Name;
            Appendix = post.Appendix;
            Body = post.Body;
            Likes = post.Likes;
            Image = post.Image;
            CreatedAt = createdAtDate.Day.ToString() + "." + createdAtDate.Month.ToString() + "." + createdAtDate.Year.ToString();
            Liked = liked;
            //Comments = post.Comments;
            CurrentUserPost = currentUserPost;
        }

    }
}
