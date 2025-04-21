using TechChallenge.CreateContact.Application.Dtos.Events;
using TechChallenge.CreateContact.Application.Interfaces;
using TechChallenge.CreateContact.Infrastructure.Interfaces;

namespace TechChallenge.CreateContact.Application.MessageHandlers;

public class ContactDeletedMessageHandler(IContactService contactService) : IMessageHandler<ContactDeletedEventDto>
{
    public async Task Handle(ContactDeletedEventDto message)
        => await contactService.DeleteAsync(message);
}
