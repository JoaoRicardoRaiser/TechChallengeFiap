using AutoMapper;
using TechChallenge.UpdateContact.Api.Dtos;
using TechChallenge.UpdateContact.Application.Dtos;

namespace TechChallenge.UpdateContact.Api.Profiles;

public class ContactControllerProfile : Profile
{
    public ContactControllerProfile()
    {
        ContactMapping();
    }

    private void ContactMapping()
    {
        CreateMap<PutContactDto, UpdateContactDto>()
            .ForMember(dest => dest.Phone, options => options.MapFrom(src => new PhoneDto { Number = src.PhoneNumber! }));
    }

}