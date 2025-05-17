using Raisersoft.EasyRabbit.Interfaces;
using TechChallenge.GetContact.Application.Dtos.Events;
using TechChallenge.GetContact.Application.Interfaces;

namespace TechChallenge.GetContact.Application.MessageHandlers;

public class ContactUpdatedMessageHandler(IContactService contactService) : IMessageHandler<ContactUpdatedEventDto>
{
    public async Task HandleAsync(ContactUpdatedEventDto message)
        => await contactService.UpdateAsync(message);
}
