using Raisersoft.EasyRabbit.Interfaces;
using TechChallenge.DeleteContact.Application.Dtos.Events;
using TechChallenge.DeleteContact.Application.Interfaces;

namespace TechChallenge.DeleteContact.Application.MessageHandlers;

public class ContactUpdatedMessageHandler(IContactService contactService) : IMessageHandler<ContactUpdatedEventDto>
{
    public Task HandleAsync(ContactUpdatedEventDto message)
        => contactService.UpdateAsync(message);
}
