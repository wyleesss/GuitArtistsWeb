using GuitArtistsWeb.Attributes;
using System.ComponentModel.DataAnnotations;

namespace GuitArtistsWeb.Models
{
    public class EmailConfirmationModel
    {
        [Required(ErrorMessage = "*поле є обов'язковим")]
        [EmailAddress(ErrorMessage = "*недійсний формат електронної пошти")]
        [UniqueConfirmEmailValidation]
        public string Email { get; set; }
    }
}
