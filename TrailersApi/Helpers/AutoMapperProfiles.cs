using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrailersApi.DTOs;
using TrailersApi.Models;

namespace TrailersApi.Helpers {
    public class AutoMapperProfiles: Profile {
        public AutoMapperProfiles() {
            CreateMap<UserTrailer, UserInfo>().ReverseMap();


            CreateMap<Trailer, TrailersDTO>().ReverseMap();
            CreateMap<Trailer, TrailersCreacionDTO>().ReverseMap();
            CreateMap<TrailersCreacionDTO, Trailer>();

        }
    }
}
