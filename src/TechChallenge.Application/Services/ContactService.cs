using AutoMapper;
using TechChallenge.Application.Dtos;
using TechChallenge.Application.Interfaces;
using TechChallenge.Domain.Entities;
using TechChallenge.Domain.Exceptions;
using TechChallenge.Domain.Interfaces.Repositories;

namespace TechChallenge.Application.Services;

public class ContactService(
    IRepository<Contact> contactRepository,
    IPhoneAreaCache phoneAreaCache,
    IMapper mapper) : IContactService
{
    public async Task<IEnumerable<Contact>> GetAsync(int? phoneAreaCode)
        => await contactRepository.GetAsync(
            c => phoneAreaCode == null || c.PhoneAreaCode == phoneAreaCode,
            [nameof(Contact.PhoneArea)]
        );

    public async Task CreateAsync(CreateContactDto dto)
    {
        ValidatePhoneAreaCodeExists(dto.Phone);
        await ValidateContactAlredySavedAsync(dto);

        var contact = mapper.Map<Contact>(dto);

        await contactRepository.AddAsync(contact);

        await contactRepository.SaveChangesAsync();
    }

    public async Task UpdateAsync(UpdateContactDto dto)
    {
        ValidatePhoneAreaCodeExists(dto.Phone);
        var contactSaved = await GetContactSavedByIdAsync(dto.ContactId);

        mapper.Map(dto, contactSaved!);

        await contactRepository.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid contactId)
    {
        var contactSaved = await GetContactSavedByIdAsync(contactId);

        contactRepository.Delete(contactSaved);

        await contactRepository.SaveChangesAsync();
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

    private async Task<Contact> GetContactSavedById(Guid contactId)
        => await contactRepository.SingleOrDefaultAsync(c => c.Id == contactId) ?? throw new BusinessException($"Contact not exists. Id: {contactId}");

    private async Task<Contact> GetContactSavedByIdAsync(Guid contactId)
        => await contactRepository.SingleOrDefaultAsync(c => c.Id == contactId) 
        ?? throw new BusinessException($"Contact not exists. Id: {contactId}");
}
