using AutoMapper;
using TechChallenge.CreateContact.Api.Dtos;
using TechChallenge.CreateContact.Application.Dtos;

namespace TechChallenge.CreateContact.Api.Profiles;

public class ContactControllerProfile : Profile
{
    public ContactControllerProfile()
    {
        CreateContactDtoMapping();
    }

    private void CreateContactDtoMapping()
    {
        CreateMap<PostContactDto, CreateContactDto>()
                    .ForMember(dest => dest.Phone, options => options.MapFrom(src => new PhoneDto { Number = src.PhoneNumber! }));
    }
}