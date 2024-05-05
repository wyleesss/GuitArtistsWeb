using AutoMapper;
using GuitArtistsWeb.Models;
using FullDB.Data.Entity;
using GuitArtistsWeb.Models;

namespace GuitArtistsWeb.Helpers
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
