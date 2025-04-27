using AutoMapper;
using TechChallenge.UpdateContact.Domain.Entities;
using TechChallenge.UpdateContact.Application.Dtos;

namespace TechChallenge.UpdateContact.Application.Profiles;

public class ContactProfile : Profile
{
    public ContactProfile()
    {
        UpdateContactMapping();
    }

    private void UpdateContactMapping()
    {
        CreateMap<UpdateContactDto, Contact>()
            .ForMember(dest => dest.PhoneAreaCode, options => options.MapFrom(src => src.Phone.AreaCode))
            .ForMember(dest => dest.Phone, options => options.MapFrom(src => src.Phone.Number));
    }
}
