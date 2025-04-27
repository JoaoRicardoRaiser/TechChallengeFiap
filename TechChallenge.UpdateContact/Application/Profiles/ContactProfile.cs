using AutoMapper;
using TechChallenge.CreateContact.Domain.Entities;
using TechChallenge.UpdateContact.Application.Dtos;

namespace TechChallenge.CreateContact.Application.Profiles;

public class ContactProfile : Profile
{
    public ContactProfile()
    {
        CreateContactMapping();
    }

    private void CreateContactMapping()
    {
        CreateMap<CreateContactDto, Contact>()
            .ForMember(dest => dest.PhoneAreaCode, options => options.MapFrom(src => src.Phone.AreaCode))
            .ForMember(dest => dest.Phone, options => options.MapFrom(src => src.Phone.Number));
    }
}
