using TechChallenge.DeleteContact.Application.Dtos.Events;
using TechChallenge.DeleteContact.Application.Interfaces;
using TechChallenge.DeleteContact.Infrastructure.Interfaces;

namespace TechChallenge.DeleteContact.Application.MessageHandlers;

public class ContactCreatedMessageHandler(IContactService contactService) : IMessageHandler<ContactCreatedEventDto>
{
    public async Task Handle(ContactCreatedEventDto message)
        => await contactService.CreateAsync(message);
}
