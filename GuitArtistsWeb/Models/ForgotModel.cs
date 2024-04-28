using GuitArtists.Attributes;
using System.ComponentModel.DataAnnotations;

namespace GuitArtists.Models
{
    public class ForgotModel
    {
        [Required(ErrorMessage = "*поле є обов'язковим")]
        [ExistingForgotUserValidation]
        public string EmailLogin { get; set; }
    }
}
