using GuitArtists.Attributes;
using System.ComponentModel.DataAnnotations;

namespace GuitArtists.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "*поле є обов'язковим")]
        [EmailAddress(ErrorMessage = "*недійсний формат електронної пошти")]
        public string Email { get; set; }

        [Required(ErrorMessage = "*поле є обов'язковим")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [ExistingUserValidation]
        public bool ExistingUser { get; }
    }
}
