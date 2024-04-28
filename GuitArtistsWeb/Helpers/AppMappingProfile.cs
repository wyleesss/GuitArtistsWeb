using AutoMapper;
using GuitArtists.Models;
using FullDB.Data.Entity;
using GuitArtists.Models;

namespace GuitArtists.Helpers
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<Chord, SongViewModel>().ReverseMap();
            CreateMap<Section, SectionNameViewModel>().ReverseMap();
        }
    }
}
