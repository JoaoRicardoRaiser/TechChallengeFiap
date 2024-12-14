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

    public async Task Create(CreateContactDto dto)
    {
        await ValidateContactAlredySaved(dto);

        var contact = mapper.Map<Contact>(dto);

        await contactRepository.AddAsync(contact);

        await contactRepository.SaveChangesAsync();

        contact.PhoneArea = phoneAreaCache.GetByCode(dto.Phone.AreaCode);
        contactCache.Add(contact);
    }

    public async Task Update(UpdateContactDto dto)
    {
        var contactSaved = await GetContactSavedById(dto.ContactId);

        mapper.Map(dto, contactSaved!);

        await contactRepository.SaveChangesAsync();

        contactSaved.PhoneArea = phoneAreaCache.GetByCode(contactSaved.PhoneAreaCode);
        contactCache.Update(contactSaved);
    }

    public async Task Delete(Guid contactId)
    {
        var contactSaved = await GetContactSavedById(contactId);

        contactRepository.Delete(contactSaved);

        await contactRepository.SaveChangesAsync();

        contactCache.Delete(contactSaved);
    }

    private async Task ValidateContactAlredySaved(CreateContactDto dto)
    {
        var contactSaved = await contactRepository.SingleOrDefaultAsync(c => c.Name == dto.Name);
        if (contactSaved is not null)
            throw new BusinessException($"Contact with this name alredy exists. Name: {dto.Name}");
    }

    private async Task<Contact> GetContactSavedById(Guid contactId)
        => await contactRepository.SingleOrDefaultAsync(c => c.Id == contactId) ?? throw new BusinessException($"Contact not exists. Id: {contactId}");
}
