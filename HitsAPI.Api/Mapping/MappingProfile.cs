using AutoMapper;
using HitsAPI.api.Resource;
using HitsAPI.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HitsAPI.api.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Domain to resource
            CreateMap<Music, MusicResource>();
            CreateMap<Artist, ArtistResource>();

            //Resource to Domain
            CreateMap<MusicResource, Music>();
            CreateMap<SaveMusicResource, Music>();
            CreateMap<ArtistResource, Artist>();
            CreateMap<SaveArtistResource, Artist>();
        }
    }
}
