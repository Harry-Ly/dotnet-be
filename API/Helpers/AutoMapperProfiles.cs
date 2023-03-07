using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        // The second .ForMember lets us calculate age without asking for the whole entity; more efficient
        CreateMap<AppUser, MemberDto>()
            .ForMember(dest => dest.PhotoUrl,
                opt => opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url))
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
        CreateMap<Photo, PhotoDto>(); // From, To
        CreateMap<MemberUpdateDto, AppUser>(); // Added this to auto map it
        CreateMap<RegisterDto, AppUser>();
        CreateMap<Message, MessageDto>()
            .ForMember(d => d.SenderPhotoUrl,
                o => o.MapFrom(
                    s => s.Sender.Photos.FirstOrDefault(x => x.IsMain).Url))
            .ForMember(d => d.RecipientPhotoUrl,
                o => o.MapFrom(
                    s => s.Recipient.Photos.FirstOrDefault(x => x.IsMain).Url));
        CreateMap<DateTime, DateTime>().ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc));
        CreateMap<DateTime?, DateTime?>().ConvertUsing(d => d.HasValue ? 
            DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : null);
    }
}