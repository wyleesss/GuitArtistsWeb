using FullDB.Data;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace GuitArtistsWeb.Attributes
{
    public class UniqueConfirmEmailValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var serviceProvider = validationContext.GetRequiredService<IServiceProvider>();
            var _dbContext = serviceProvider.GetRequiredService<AppDbContext>();
            var httpContext = validationContext.GetRequiredService<IHttpContextAccessor>()?.HttpContext;

            var email = value?.ToString();
            var userEmail = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;

            if (userEmail == null)
            {
                return new ValidationResult("*спочатку зареєструйтесь на сайті");
            }

            if (_dbContext.GetUserByEmail(email) != null && userEmail != email)
            {
                return new ValidationResult("*попередній email вже існує в системі");
            }

            return ValidationResult.Success;
        }
    }
}


