using Raisersoft.EasyRabbit.Interfaces;
using TechChallenge.UpdateContact.Application.Dtos.Events;
using TechChallenge.UpdateContact.Application.Interfaces;

namespace TechChallenge.UpdateContact.Application.MessageHandlers;

public class ContactDeletedMessageHandler(IContactService contactService) : IMessageHandler<ContactDeletedEventDto>
{
    public async Task Handle(ContactDeletedEventDto message)
        => await contactService.DeleteAsync(message);

    public Task HandleAsync(ContactDeletedEventDto message)
    {
        throw new NotImplementedException();
    }
}
