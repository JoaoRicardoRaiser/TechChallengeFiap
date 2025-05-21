using AutoMapper;
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
        => CreateMap<ContactUpdatedEventDto, Contact>();

    private void CreateContactEventMapping()
        => CreateMap<ContactCreatedEventDto, Contact>();
}
