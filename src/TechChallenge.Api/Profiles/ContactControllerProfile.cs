using AutoMapper;
using TechChallenge.Api.Dtos;
using TechChallenge.Application.Dtos;

namespace TechChallenge.Api.Profiles;

public class ContactControllerProfile: Profile
{
    public ContactControllerProfile()
    {
        CreateContactDtoMapping();
        UpdateContactDtoMapping();
    }

    private void CreateContactDtoMapping()
    {
        CreateMap<PostContactDto, CreateContactDto>()
                    .ForMember(dest => dest.Phone, options => options.MapFrom(src => new PhoneDto { Number = src.PhoneNumber! }));
    }

    private void UpdateContactDtoMapping()
    {
        CreateMap<PutContactDto, UpdateContactDto>()
                    .ForMember(dest => dest.Phone, options => options.MapFrom(src => new PhoneDto { Number = src.PhoneNumber! }));
    }
}
