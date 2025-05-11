using AutoMapper;
using TechChallenge.GetContact.Application.Dtos;
using TechChallenge.GetContact.Domain.Entities;
using TechChallenge.GetContact.Api.Dtos;

namespace TechChallenge.GetContact.Api.Profiles;

public class ContactControllerProfile : Profile
{
    public ContactControllerProfile()
    {
        ContactMapping();
    }

    private void ContactMapping()
    {
        CreateMap<PutContactDto, Contact>()
                    .ForMember(dest => dest.Phone, options => options.MapFrom(src => new PhoneDto { Number = src.PhoneNumber! }));
    }

}