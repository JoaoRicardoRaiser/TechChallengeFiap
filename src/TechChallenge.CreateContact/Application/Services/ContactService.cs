using AutoMapper;
using Raisersoft.EasyRabbit.Interfaces;
using TechChallenge.CreateContact.Application.Dtos;
using TechChallenge.CreateContact.Application.Dtos.Events;
using TechChallenge.CreateContact.Application.Interfaces;
using TechChallenge.CreateContact.Domain.Entities;
using TechChallenge.CreateContact.Domain.Exceptions;
using TechChallenge.CreateContact.Domain.Interfaces;
using TechChallenge.CreateContact.Infrastructure.Interfaces;

namespace TechChallenge.CreateContact.Application.Services;

public class ContactService(
    IRepository<Contact> contactRepository,
    IPhoneAreaCache phoneAreaCache,
    IMapper mapper,
    IMessagePublisherService<ContactCreatedEventDto> messagePublisher) : IContactService
{
    public async Task CreateAsync(CreateContactDto dto)
    {
        ValidatePhoneAreaCodeExists(dto.Phone);
        await ValidateContactAlredySavedAsync(dto);

        var contact = mapper.Map<Contact>(dto);

        await contactRepository.AddAsync(contact);

        await contactRepository.SaveChangesAsync();
        
        var @event = mapper.Map<ContactCreatedEventDto>(contact);

        await messagePublisher.SendMessageAsync(@event);
    }

    public async Task UpdateAsync(ContactUpdatedEventDto dto)
    {
        var contactSaved = await GetContactSavedByIdAsync(dto.ContactId);

        mapper.Map(dto, contactSaved);

        await contactRepository.SaveChangesAsync();
    }

    public async Task DeleteAsync(ContactDeletedEventDto dto)
    {
        var contactSaved = await GetContactSavedByIdAsync(dto.ContactId);

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

    private async Task<Contact> GetContactSavedByIdAsync(Guid contactId)
        => await contactRepository.SingleOrDefaultAsync(c => c.Id == contactId)
        ?? throw new BusinessException($"Contact not exists. Id: {contactId}");
}
