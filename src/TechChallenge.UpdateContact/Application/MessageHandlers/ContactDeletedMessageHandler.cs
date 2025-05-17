using Raisersoft.EasyRabbit.Interfaces;
using TechChallenge.UpdateContact.Application.Dtos.Events;
using TechChallenge.UpdateContact.Application.Interfaces;

namespace TechChallenge.UpdateContact.Application.MessageHandlers;

public class ContactDeletedMessageHandler(IContactService contactService) : IMessageHandler<ContactDeletedEventDto>
{
    public async Task HandleAsync(ContactDeletedEventDto message)
        => await contactService.DeleteAsync(message);
}
