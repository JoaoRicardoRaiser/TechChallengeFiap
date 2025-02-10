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

    public async Task Create(CreateContactDto dto)
    {
        await ValidateContactAlredySaved(dto);

        var contact = mapper.Map<Contact>(dto);

        await contactRepository.AddAsync(contact);

        await contactRepository.SaveChangesAsync();
<<<<<<< Updated upstream

        contact.PhoneArea = phoneAreaCache.GetByCode(dto.Phone.AreaCode);
        contactCache.Add(contact);
=======
>>>>>>> Stashed changes
    }

    public async Task Update(UpdateContactDto dto)
    {
        var contactSaved = await GetContactSavedById(dto.ContactId);

        mapper.Map(dto, contactSaved!);

        await contactRepository.SaveChangesAsync();
<<<<<<< Updated upstream

        contactSaved.PhoneArea = phoneAreaCache.GetByCode(contactSaved.PhoneAreaCode);
        contactCache.Update(contactSaved);
=======
>>>>>>> Stashed changes
    }

    public async Task Delete(Guid contactId)
    {
        var contactSaved = await GetContactSavedById(contactId);

        contactRepository.Delete(contactSaved);

        await contactRepository.SaveChangesAsync();
    }

    private async Task ValidateContactAlredySaved(CreateContactDto dto)
    {
        var contactSaved = await contactRepository.SingleOrDefaultAsync(c => c.Name == dto.Name);
        if (contactSaved is not null)
            throw new BusinessException($"Contact with this name alredy exists. Name: {dto.Name}");
    }

<<<<<<< Updated upstream
    private async Task<Contact> GetContactSavedById(Guid contactId)
        => await contactRepository.SingleOrDefaultAsync(c => c.Id == contactId) ?? throw new BusinessException($"Contact not exists. Id: {contactId}");
=======
    private void ValidatePhoneAreaCodeExists(PhoneDto phoneDto)
    {
        if (!phoneAreaCache.Exists(phoneDto.AreaCode))
            throw new BusinessException($"Phone area code not exists. Code: {phoneDto.AreaCode}");
    }

    private async Task<Contact> GetContactSavedByIdAsync(Guid contactId)
        => await contactRepository.SingleOrDefaultAsync(c => c.Id == contactId) 
        ?? throw new BusinessException($"Contact not exists. Id: {contactId}");
>>>>>>> Stashed changes
}
