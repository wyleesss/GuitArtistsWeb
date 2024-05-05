using System.ComponentModel.DataAnnotations;

namespace GuitArtistsWeb.Models
{
    public class RecoveryModel
    {
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
    }
}
