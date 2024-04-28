using GuitArtists.Attributes;
using System.ComponentModel.DataAnnotations;

namespace GuitArtists.Models
{
    public class RegistrationModel
    {
        [Required(ErrorMessage = "*поле є обов'язковим")]
        [EmailAddress(ErrorMessage = "*недійсний формат електронної пошти")]
        [UniqueEmailValidation]
        public string Email { get; set; }

        [Required(ErrorMessage = "*поле є обов'язковим")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "*логін повинен містити принаймні 3 символи")]
        [UniqueLoginValidation]
        public string Login { get; set; }

        [Required(ErrorMessage = "*поле є обов'язковим")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "*пароль повинен містити принаймні 6 символів")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-zA-Z]).+$",
            ErrorMessage = "*пароль повинен містити як мінімум одну цифру і латинську літеру")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "*поле є обов'язковим")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "*паролі не сходяться")]
        public string ConfirmPassword { get; set; }

        [FullNameValidation]
        public string? FirstName { get; set; }

        [FullNameValidation]
        public string? LastName { get; set; }
    }
}
