using System.ComponentModel.DataAnnotations;

public class FullNameValidationAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var firstName = validationContext.ObjectType.GetProperty("FirstName")?.GetValue(validationContext.ObjectInstance, null);
        var lastName = validationContext.ObjectType.GetProperty("LastName")?.GetValue(validationContext.ObjectInstance, null);

        if (!string.IsNullOrEmpty(firstName?.ToString()) || !string.IsNullOrEmpty(lastName?.ToString()))
        {
            if (string.IsNullOrEmpty(firstName?.ToString()) || string.IsNullOrEmpty(lastName?.ToString()))
            {
                return new ValidationResult("*якщо ви вказуєте ім'я або прізвище, вкажіть обидва");
            }
        }

        return ValidationResult.Success;
    }
}