using AutoMapper;
using Microsoft.Extensions.Logging;
using TechChallenge.Application.Dtos;
using TechChallenge.Application.Interfaces;
using TechChallenge.Domain.Entities;
using TechChallenge.Domain.Exceptions;
using TechChallenge.Domain.Interfaces.Repositories;

namespace TechChallenge.Application.Services;

public class ContactService(
    ILogger<ContactService> logger,
    IRepository<Contact> contactRepository,
    IContactCache contactCache,
    IPhoneAreaCache phoneAreaCache,
    IMapper mapper) : IContactService
{
    public async Task<IEnumerable<Contact>> GetAsync(int? phoneAreaCode)
    {
        if (contactCache.ContactsOnCache())
        {
            logger.LogInformation("obtained contacts from cache");
            return contactCache.GetAll();
        }
        else
        {
            var contactsSaved = await contactRepository.GetAsync(
                c => phoneAreaCode == null || c.PhoneAreaCode == phoneAreaCode,
                [nameof(Contact.PhoneArea)]
            );

            contactCache.AddRange(contactsSaved);
            return contactsSaved;
        }
    }

    public async Task CreateAsync(CreateContactDto dto)
    {
        ValidatePhoneAreaCodeExists(dto.Phone);
        await ValidateContactAlredySavedAsync(dto);

        var contact = mapper.Map<Contact>(dto);

        await contactRepository.AddAsync(contact);

        await contactRepository.SaveChangesAsync();

        contact.PhoneArea = GetPhoneArea(dto.Phone);
        contactCache.Add(contact);
    }

    public async Task UpdateAsync(UpdateContactDto dto)
    {
        ValidatePhoneAreaCodeExists(dto.Phone);
        var contactSaved = await GetContactSavedByIdAsync(dto.ContactId);

        mapper.Map(dto, contactSaved!);

        await contactRepository.SaveChangesAsync();

        contactSaved.PhoneArea = GetPhoneArea(dto.Phone);
        contactCache.Update(contactSaved);
    }

    public async Task DeleteAsync(Guid contactId)
    {
        var contactSaved = await GetContactSavedByIdAsync(contactId);

        contactRepository.Delete(contactSaved);

        await contactRepository.SaveChangesAsync();

        contactCache.Delete(contactSaved);
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

    private PhoneArea GetPhoneArea(PhoneDto phoneDto)
        => phoneAreaCache.GetByCode(phoneDto.AreaCode);


    private async Task<Contact> GetContactSavedByIdAsync(Guid contactId)
        => await contactRepository.SingleOrDefaultAsync(c => c.Id == contactId) 
        ?? throw new BusinessException($"Contact not exists. Id: {contactId}");
}
