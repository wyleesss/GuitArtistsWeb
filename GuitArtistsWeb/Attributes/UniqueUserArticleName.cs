using FullDB.Data;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace GuitArtists.Attributes
{
    public class UniqueUserArticleName : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var serviceProvider = validationContext.GetRequiredService<IServiceProvider>();
            var _dbContext = serviceProvider.GetRequiredService<AppDbContext>();
            var httpContext = validationContext.GetRequiredService<IHttpContextAccessor>()?.HttpContext;

            var name = value?.ToString();
            var userId = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            if (_dbContext.Posts.Any(p => p.Name == name && p.UserId.ToString() == userId))
            {
                return new ValidationResult("*у вас вже є стаття з такою назвою");
            }

            return ValidationResult.Success;
        }
    }
}
