using FullDB.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace FullDB.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Chord> Chords { get; set; }

        public void AddUser(User user)
        {
            Users.Add(user);
        }

        public User? GetUserByLogin(string? login)
        {
            if (string.IsNullOrEmpty(login))
                return null;
            return Users.Include(a => a.Posts).FirstOrDefault(u => u.Login == login);
        }

        public User? GetUserByEmail(string? email)
        {
            if (string.IsNullOrEmpty(email))
                return null;
            return Users.Include(a => a.Posts).FirstOrDefault(u => u.Email == email);
        }

        public User? GetUserByGoogleID(string? googleId)
        {
            if (string.IsNullOrEmpty(googleId))
                return null;
            return Users.Include(a => a.Posts).FirstOrDefault(u => u.GoogleId == googleId);
        }

        public User? GetUserByID(string? id)
        {
            if (string.IsNullOrEmpty(id))
                return null;
            return Users.Include(a => a.Posts).FirstOrDefault(u => u.Id.ToString() == id);
        }

        public Post? GetPost(string? userId, string? slug)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(slug))
                return null;
            return Posts.Include(a => a.User).FirstOrDefault(p => p.UserId.ToString() == userId && p.Slug == slug);
        }

        public IQueryable<Chord> FullTextSearch(string query)
        {
            query = query.ToLower();
            return Chords
                .Where(a => EF.Functions.Like(a.NameSong.ToLower(), $"%{query}%")
                         || EF.Functions.Like(a.NameAvtor.ToLower(), $"%{query}%")
                         || EF.Functions.Like(a.Body.ToLower(), $"%{query}%"));
        }

        public IQueryable<Post> FullTextSearchArticles(string query)
        {
            query = query.ToLower();
            return Posts
                .Where(a => EF.Functions.Like(a.Name.ToLower(), $"%{query}%")
                         || EF.Functions.Like(a.User.Login.ToLower(), $"%{query}%")
                         || EF.Functions.Like(a.Body.ToLower(), $"%{query}%")).Include(u => u.User);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<Section>().HasIndex(s => s.Slug).IsUnique();
            modelBuilder.Entity<Lesson>().HasIndex(l => l.Slug).IsUnique();
            modelBuilder.Entity<Post>().HasIndex(p => p.Slug).IsUnique();
            modelBuilder.Entity<Chord>().HasIndex(a => a.Slug).IsUnique();

            modelBuilder.Entity<Section>()
                .HasMany(s => s.Children)
                .WithOne(s => s.Parent)
                .HasForeignKey(s => s.ParentId);

            modelBuilder.Entity<Lesson>()
                .HasOne(l => l.Section)
                .WithMany(s => s.Lessons)
                .HasForeignKey(l => l.SectionId);

            modelBuilder.Entity<Post>()
                        .HasOne(p => p.User)
                        .WithMany(u => u.Posts)
                        .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<User>()
            .Property(e => e.Id)
            .HasConversion(
                v => v.ToString().ToLower(),
                v => new Guid(v));

            modelBuilder.Entity<Lesson>()
            .Property(e => e.Id)
            .HasConversion(
                v => v.ToString().ToLower(),
                v => new Guid(v));

            modelBuilder.Entity<Post>()
            .Property(e => e.Id)
            .HasConversion(
                v => v.ToString().ToLower(),
                v => new Guid(v));

            modelBuilder.Entity<Section>()
            .Property(e => e.Id)
            .HasConversion(
                v => v.ToString().ToLower(),
                v => new Guid(v));
        }
    }
}
