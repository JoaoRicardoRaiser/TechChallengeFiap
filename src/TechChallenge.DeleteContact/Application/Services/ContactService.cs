using AutoMapper;
using TechChallenge.DeleteContact.Application.Dtos.Events;
using TechChallenge.DeleteContact.Application.Interfaces;
using TechChallenge.DeleteContact.Domain.Entities;
using TechChallenge.DeleteContact.Domain.Exceptions;
using TechChallenge.DeleteContact.Domain.Interfaces;
using TechChallenge.DeleteContact.Infrastructure.Interfaces;

namespace TechChallenge.DeleteContact.Application.Services;

public class ContactService(
    IRepository<Contact> contactRepository,
    IMapper mapper,
    IPublisherService<Contact> messagePublisher) : IContactService
{
    public async Task CreateAsync(ContactCreatedEventDto dto)
    {
        var contact = mapper.Map<Contact>(dto);

        await contactRepository.AddAsync(contact);

        await contactRepository.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid contactId)
    {
        var contactSaved = await GetContactSavedByIdAsync(contactId);

        contactSaved.Delete();

        await contactRepository.SaveChangesAsync();

        await messagePublisher.SendMessageAsync(contactSaved);
    }


    private async Task<Contact> GetContactSavedByIdAsync(Guid contactId)
        => await contactRepository.SingleOrDefaultAsync(c => c.Id == contactId)
        ?? throw new BusinessException($"Contact not exists. Id: {contactId}");
}