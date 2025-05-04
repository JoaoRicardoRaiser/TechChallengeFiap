using Raisersoft.EasyRabbit.Interfaces;
using TechChallenge.DeleteContact.Application.Dtos.Events;
using TechChallenge.DeleteContact.Application.Interfaces;

namespace TechChallenge.DeleteContact.Application.MessageHandlers;

public class ContactCreatedMessageHandler(IContactService contactService) : IMessageHandler<ContactCreatedEventDto>
{
    public async Task HandleAsync(ContactCreatedEventDto message)
        => await contactService.CreateAsync(message);
}
