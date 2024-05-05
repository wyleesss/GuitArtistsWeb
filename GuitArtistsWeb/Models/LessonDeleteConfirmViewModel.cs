using FullDB.Data.Entity;

namespace GuitArtistsWeb.Models
{
    public class LessonDeleteConfirmViewModel
    {
        public Lesson lesson { get; set; }
        public string state { get; set; }
    }
}
