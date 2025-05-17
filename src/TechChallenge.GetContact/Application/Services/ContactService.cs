using AutoMapper;
using TechChallenge.GetContact.Application.Dtos.Events;
using TechChallenge.GetContact.Application.Interfaces;
using TechChallenge.GetContact.Domain.Entities;
using TechChallenge.GetContact.Domain.Exceptions;
using TechChallenge.GetContact.Domain.Interfaces;

namespace TechChallenge.GetContact.Application.Services;

public class ContactService(
    IRepository<Contact> contactRepository,
    IMapper mapper) : IContactService
{
    public async Task<IEnumerable<Contact>> GetAsync(int? phoneAreaCode)
        => await contactRepository.GetAsync(
            c => phoneAreaCode == null || c.PhoneAreaCode == phoneAreaCode,
            [nameof(Contact.PhoneArea)]
        );

    public async Task CreateAsync(ContactCreatedEventDto dto)
    {
        var contact = mapper.Map<Contact>(dto);

        await contactRepository.AddAsync(contact);

        await contactRepository.SaveChangesAsync();
    }

    public async Task UpdateAsync(ContactUpdatedEventDto dto)
    {
        var contact = await GetContactSavedByIdAsync(dto.ContactId);

        mapper.Map(dto, contact);

        await contactRepository.SaveChangesAsync();
    }    

    public async Task DeleteAsync(ContactDeletedEventDto dto)
    {
        var contactSaved = await GetContactSavedByIdAsync(dto.ContactId);

        contactRepository.Delete(contactSaved);

        await contactRepository.SaveChangesAsync();
    }

    private async Task<Contact> GetContactSavedByIdAsync(Guid contactId)
        => await contactRepository.SingleOrDefaultAsync(c => c.Id == contactId)
        ?? throw new BusinessException($"Contact not exists. Id: {contactId}");
}
