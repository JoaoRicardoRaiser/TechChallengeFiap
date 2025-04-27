using AutoMapper;
using TechChallenge.CreateContact.Application.Dtos;
using TechChallenge.UpdateContact.Api.Dtos;
using TechChallenge.UpdateContact.Application.Dtos;

namespace TechChallenge.CreateContact.Api.Profiles;

public class ContactControllerProfile : Profile
{
    public ContactControllerProfile()
    {
        UpdateContactDtoMapping();
    }

    private void UpdateContactDtoMapping()
    {
        CreateMap<PutContactDto, UpdateContactDto>()
                    .ForMember(dest => dest.Phone, options => options.MapFrom(src => new PhoneDto { Number = src.PhoneNumber! }));
    }

}