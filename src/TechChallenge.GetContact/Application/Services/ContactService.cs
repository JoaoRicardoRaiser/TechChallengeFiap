using AutoMapper;
using Raisersoft.EasyRabbit.Interfaces;
using TechChallenge.GetContact.Application.Dtos;
using TechChallenge.GetContact.Application.Dtos.Events;
using TechChallenge.GetContact.Application.Interfaces;
using TechChallenge.GetContact.Domain.Entities;
using TechChallenge.GetContact.Domain.Exceptions;
using TechChallenge.GetContact.Domain.Interfaces;
using TechChallenge.GetContact.Infrastructure.Interfaces;

namespace TechChallenge.GetContact.Application.Services;

public class ContactService(
    IRepository<Contact> contactRepository,
    IPhoneAreaCache phoneAreaCache,
    IMapper mapper,
    IMessagePublisherService<Contact> messagePublisher) : IContactService
{
    public async Task<IEnumerable<Contact>> GetAsync(int? phoneAreaCode)
        => await contactRepository.GetAsync(
            c => phoneAreaCode == null || c.PhoneAreaCode == phoneAreaCode,
            [nameof(Contact.PhoneArea)]
        );
    
    private async Task<Contact> GetContactSavedByIdAsync(Guid contactId)
        => await contactRepository.SingleOrDefaultAsync(c => c.Id == contactId)
        ?? throw new BusinessException($"Contact not exists. Id: {contactId}");

    public async Task CreateAsync(ContactCreatedEventDto dto)
    {
        var contact = mapper.Map<Contact>(dto);

        await contactRepository.AddAsync(contact);

        await contactRepository.SaveChangesAsync();
    }

    public async Task UpdateAsync(ContactUpdatedEventDto dto)
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
    
}
