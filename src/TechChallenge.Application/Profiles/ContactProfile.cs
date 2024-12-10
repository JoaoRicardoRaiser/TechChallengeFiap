using AutoMapper;
using TechChallenge.Application.Dtos;
using TechChallenge.Domain.Entities;

namespace TechChallenge.Application.Profiles;

public class ContactProfile: Profile
{
    public ContactProfile()
    {
        CreateMap<CreateContactDto, Contact>()
            .ForMember(dest => dest.PhoneAreaCode, options => options.MapFrom(src => src.PhoneNumber.PhoneAreaCode))
            .ForMember(dest => dest.Phone, options => options.MapFrom(src => src.PhoneNumber.Number));
    }
}
