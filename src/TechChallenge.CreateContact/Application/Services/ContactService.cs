using AutoMapper;
using TechChallenge.CreateContact.Application.Dtos;
using TechChallenge.CreateContact.Application.Interfaces;
using TechChallenge.CreateContact.Domain.Entities;
using TechChallenge.CreateContact.Domain.Exceptions;
using TechChallenge.CreateContact.Domain.Interfaces;

namespace TechChallenge.CreateContact.Application.Services;

public class ContactService(
    IRepository<Contact> contactRepository,
    IPhoneAreaCache phoneAreaCache,
    IMapper mapper,
    IMessagePublisher<Contact> messagePublisher) : IContactService
{
    public async Task CreateAsync(CreateContactDto dto)
    {
        ValidatePhoneAreaCodeExists(dto.Phone);
        await ValidateContactAlredySavedAsync(dto);

        var contact = mapper.Map<Contact>(dto);

        await contactRepository.AddAsync(contact);

        await contactRepository.SaveChangesAsync();

        await messagePublisher.SendMessageAsync(contact);
    }

    private async Task ValidateContactAlredySavedAsync(CreateContactDto dto)
    {
        var contactSaved = await contactRepository.SingleOrDefaultAsync(c => c.Name == dto.Name);
        if (contactSaved is not null)
            throw new BusinessException($"Contact with this name alredy exists. Name: {dto.Name}");
    }

    private void ValidatePhoneAreaCodeExists(PhoneDto phoneDto)
    {
        if (!phoneAreaCache.Exists(phoneDto.AreaCode))
            throw new BusinessException($"Phone area code not exists. Code: {phoneDto.AreaCode}");
    }
}
