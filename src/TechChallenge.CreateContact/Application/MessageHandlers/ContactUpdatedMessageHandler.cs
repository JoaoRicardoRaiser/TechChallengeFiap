using Raisersoft.EasyRabbit.Interfaces;
using TechChallenge.CreateContact.Application.Dtos.Events;
using TechChallenge.CreateContact.Application.Interfaces;

namespace TechChallenge.CreateContact.Application.MessageHandlers;

public class ContactUpdatedMessageHandler(IContactService contactService) : IMessageHandler<ContactUpdatedEventDto>
{
    public Task HandleAsync(ContactUpdatedEventDto message)
        => contactService.UpdateAsync(message);
}
