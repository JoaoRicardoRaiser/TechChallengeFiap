using AutoMapper;
using TechChallenge.CreateContact.Application.Dtos;
using TechChallenge.CreateContact.Application.Dtos.Events;
using TechChallenge.CreateContact.Domain.Entities;

namespace TechChallenge.CreateContact.Application.Profiles;

public class ContactProfile : Profile
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

        CreateMap<Contact, ContactCreatedEventDto>();
    }
    
    private void UpdateContactMapping()
        => CreateMap<ContactUpdatedEventDto, Contact>();
}
