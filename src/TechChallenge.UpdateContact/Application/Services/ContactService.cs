using AutoMapper;
using TechChallenge.UpdateContact.Application.Dtos;
using TechChallenge.UpdateContact.Application.Dtos.Events;
using TechChallenge.UpdateContact.Application.Interfaces;
using TechChallenge.UpdateContact.Domain.Entities;
using TechChallenge.UpdateContact.Domain.Exceptions;
using TechChallenge.UpdateContact.Domain.Interfaces;
using TechChallenge.UpdateContact.Infrastructure.Interfaces;

namespace TechChallenge.UpdateContact.Application.Services;

public class ContactService(
    IRepository<Contact> contactRepository,
    IPhoneAreaCache phoneAreaCache,
    IMapper mapper,
    IMessagePublisherService<Contact> messagePublisher) : IContactService
{
    public async Task CreateAsync(ContactCreatedEventDto dto)
    {
        var contact = mapper.Map<Contact>(dto);

        await contactRepository.AddAsync(contact);

        await contactRepository.SaveChangesAsync();
    }

    public async Task UpdateAsync(UpdateContactDto dto)
    {
        ValidatePhoneAreaCodeExists(dto.Phone);

        var contact = await GetContactSavedByIdAsync(dto.ContactId);

        contact.Email = dto.Email;
        contact.Phone = dto.Phone.Number;
        contact.PhoneAreaCode = int.Parse(dto.Phone.Number[..2]);

        await contactRepository.SaveChangesAsync();
    }    

    public async Task DeleteAsync(ContactDeletedEventDto dto)
    {
        var contactSaved = await GetContactSavedByIdAsync(dto.Id);

        contactRepository.Delete(contactSaved);

        await contactRepository.SaveChangesAsync();

        await messagePublisher.SendMessageAsync(contactSaved);
    }

    private void ValidatePhoneAreaCodeExists(PhoneDto phoneDto)
    {
        if (!phoneAreaCache.Exists(phoneDto.AreaCode))
            throw new BusinessException($"Phone area code not exists. Code: {phoneDto.AreaCode}");
    }

    private async Task<Contact> GetContactSavedByIdAsync(Guid contactId)
        => await contactRepository.SingleOrDefaultAsync(c => c.Id == contactId)
        ?? throw new BusinessException($"Contact not exists. Id: {contactId}");

    
}
