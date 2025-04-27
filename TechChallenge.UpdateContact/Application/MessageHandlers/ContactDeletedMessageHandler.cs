using TechChallenge.UpdateContact.Application.Dtos.Events;
using TechChallenge.UpdateContact.Application.Interfaces;
using TechChallenge.UpdateContact.Infrastructure.Interfaces;

namespace TechChallenge.UpdateContact.Application.MessageHandlers;

public class ContactDeletedMessageHandler(IContactService contactService) : IMessageHandler<ContactDeletedEventDto>
{
    public async Task Handle(ContactDeletedEventDto message)
        => await contactService.DeleteAsync(message);
}
