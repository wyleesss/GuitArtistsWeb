using FullDB.Data;
using System.ComponentModel.DataAnnotations;
using static GuitArtistsWeb.Helpers.HashingHelper;

namespace GuitArtistsWeb.Attributes
{
    public class ExistingUserValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var serviceProvider = validationContext.GetRequiredService<IServiceProvider>();
            var _dbContext = serviceProvider.GetRequiredService<AppDbContext>();

            var email = validationContext.ObjectType.GetProperty("Email")?.GetValue(validationContext.ObjectInstance, null);
            var password = validationContext.ObjectType.GetProperty("Password")?.GetValue(validationContext.ObjectInstance, null);

            if (email == null || password == null)
                return new ValidationResult("*неправильна пошта або пароль");

            var user = _dbContext.GetUserByEmail(email?.ToString());

            if (user != null && user.GoogleId != null)
                return new ValidationResult("*неправильна пошта або пароль");

            if (user == null || user.PasswordHash != HashPassword(password.ToString(), user.PasswordSalt))
                return new ValidationResult("*неправильна пошта або пароль");

            return ValidationResult.Success;
        }
    }
}
