using FullDB.Data.Entity;

namespace GuitArtistsWeb.Models
{
    public class SectionEditViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Guid? ParentId { get; set; }
        public List<Section> sections { get; set; }
        public string state { get; set; }
    }
}
