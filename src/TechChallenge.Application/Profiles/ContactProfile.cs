using AutoMapper;
using TechChallenge.Application.Dtos;
using TechChallenge.Domain.Entities;

namespace TechChallenge.Application.Profiles;

public class ContactProfile: Profile
{
    public ContactProfile()
    {
        CreateContactMapping();
        UpdateContactMapping();
    }

    private void CreateContactMapping()
    {
        CreateMap<CreateContactDto, Contact>()
            .ForMember(dest => dest.PhoneAreaCode, options => options.MapFrom(src => src.Phone.AreaCode))
            .ForMember(dest => dest.Phone, options => options.MapFrom(src => src.Phone.Number));
    }

    private void UpdateContactMapping()
    {
        CreateMap<UpdateContactDto, Contact>()
            .ForMember(dest => dest.Phone, options => options.MapFrom(src => src.Phone.Number))
            .ForMember(dest => dest.PhoneAreaCode, options => options.MapFrom(src => src.Phone.AreaCode));
    }
}
