using FullDB.Data;
using System.ComponentModel.DataAnnotations;

namespace GuitArtists.Attributes
{
    public class ExistingForgotUserValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var serviceProvider = validationContext.GetRequiredService<IServiceProvider>();
            var _dbContext = serviceProvider.GetRequiredService<AppDbContext>();

            var data = value?.ToString();

            if (data == null)
                return new ValidationResult("*неправильні дані");

            if (_dbContext.GetUserByEmail(data) == null && _dbContext.GetUserByLogin(data) == null)
                return new ValidationResult("*неправильні дані");

            return ValidationResult.Success;
        }
    }
}
