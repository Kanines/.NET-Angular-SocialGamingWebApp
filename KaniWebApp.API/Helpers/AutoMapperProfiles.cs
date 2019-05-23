using System.Linq;
using AutoMapper;
using KaniWebApp.API.Dtos;
using KaniWebApp.API.Models;

namespace KaniWebApp.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForListDto>()
                .ForMember(dest => dest.ImageUrl, opt =>
                {
                    opt.MapFrom(src => src.Images.FirstOrDefault(p => p.IsMain).Url);
                })
                .ForMember(dest => dest.Age, opt =>
                {
                    opt.ResolveUsing(d => d.DateOfBirth.CalculateAge());
                });
                
            CreateMap<User, UserForDetailedDto>()
                .ForMember(dest => dest.ImageUrl, opt =>
                {
                    opt.MapFrom(src => src.Images.FirstOrDefault(p => p.IsMain).Url);
                })
                .ForMember(dest => dest.Age, opt =>
                {
                    opt.ResolveUsing(d => d.DateOfBirth.CalculateAge());
                }); ;

            CreateMap<Image, ImageForDetailedDto>();
        }
    }
}