using GuitArtists.Attributes;
using System.ComponentModel.DataAnnotations;

namespace GuitArtists.Models
{
    public class CreateArticleModel
    {
        [Required(ErrorMessage = "*поле є обов'язковим")]
        [UniqueUserArticleName]
        public string Name { get; set; }

        [Required(ErrorMessage = "*поле є обов'язковим")]
        public string Appendix { get; set; }

        [Required(ErrorMessage = "*поле є обов'язковим")]
        public string Body { get; set; }

        public IFormFile? Image { get; set; }

        public string? State { get; set; } = null;
    }
}
