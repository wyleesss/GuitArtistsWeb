using GuitArtistsWeb.Attributes;
using System.ComponentModel.DataAnnotations;

namespace GuitArtistsWeb.Models
{
    public class ForgotModel
    {
        [Required(ErrorMessage = "*поле є обов'язковим")]
        [ExistingForgotUserValidation]
        public string EmailLogin { get; set; }
    }
}
