using Raisersoft.EasyRabbit.Interfaces;
using TechChallenge.CreateContact.Application.Dtos.Events;
using TechChallenge.CreateContact.Application.Interfaces;

namespace TechChallenge.CreateContact.Application.MessageHandlers;

public class ContactDeletedMessageHandler(IContactService contactService) : IMessageHandler<ContactDeletedEventDto>
{
    public async Task HandleAsync(ContactDeletedEventDto message)
        => await contactService.DeleteAsync(message);
}
