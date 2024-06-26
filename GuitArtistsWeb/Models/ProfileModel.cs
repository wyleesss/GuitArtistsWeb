﻿using FullDB.Data.Entity;

namespace GuitArtistsWeb.Models
{
    public class ProfileModel
    {
        public string Login { get; }
        public List<ArticleCardModel> Posts { get; set; }
        public string? FirstName { get; }
        public string? LastName { get; }
        public string? AvatarUrl { get; }
        public string? CreatedAt { get; }

        readonly public bool CurrentUser;

        public ProfileModel(User user, bool currentUser = false)
        {
            var createdAtDate = user.CreatedAt.Date;
            Login = user.Login;
            FirstName = user.FirstName;
            LastName = user.LastName;
            AvatarUrl = user.AvatarUrl;
            CreatedAt = createdAtDate.Day.ToString() + " / " + createdAtDate.Month.ToString() + " / " + createdAtDate.Year.ToString();
            CurrentUser = currentUser;
        }
    }
}
