using FullDB.Data;
using System.ComponentModel.DataAnnotations;

namespace GuitArtistsWeb.Attributes
{
    public class UniqueEmailValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var serviceProvider = validationContext.GetRequiredService<IServiceProvider>();
            var _dbContext = serviceProvider.GetRequiredService<AppDbContext>();

            var email = value?.ToString();

            if (_dbContext.GetUserByEmail(email) != null)
            {
                return new ValidationResult("*даний email вже існує в системі");
            }

            return ValidationResult.Success;
        }
    }
}
