using AutoMapper;
using TechChallenge.GetContact.Application.Dtos;
using TechChallenge.GetContact.Application.Dtos.Events;
using TechChallenge.GetContact.Domain.Entities;

namespace TechChallenge.GetContact.Application.Profiles;

public class ContactProfile : Profile
{
    public ContactProfile()
    {
        UpdateContactMapping();
        CreateContactEventMapping();
    }

    private void UpdateContactMapping()
    {
        CreateMap<UpdateContactDto, Contact>()
            .ForMember(dest => dest.PhoneAreaCode, options => options.MapFrom(src => src.Phone.AreaCode))
            .ForMember(dest => dest.Phone, options => options.MapFrom(src => src.Phone.Number));
    }

    private void CreateContactEventMapping()
    {
        CreateMap<ContactCreatedEventDto, Contact>();            
    }
}
