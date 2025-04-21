using AutoMapper;
using TechChallenge.DeleteContact.Application.Dtos.Events;
using TechChallenge.DeleteContact.Domain.Entities;

namespace TechChallenge.DeleteContact.Application.Profiles;

public class ContactProfile : Profile
{
    public ContactProfile()
    {
        CreateMap<ContactCreatedEventDto, Contact>();
    }
}
