using FullDB.Data;
using System.ComponentModel.DataAnnotations;
using GuitArtistsWeb.Helpers;

namespace GuitArtists.Attributes
{
    public class UniqueLoginValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var serviceProvider = validationContext.GetRequiredService<IServiceProvider>();
            var _dbContext = serviceProvider.GetRequiredService<AppDbContext>();

            var login = value?.ToString();

            if (_dbContext.GetUserByLogin(LoginChecker.Change(login)) != null)
            {
                return new ValidationResult("*даний login вже існує в системі");
            }

            return ValidationResult.Success;
        }
    }
}
