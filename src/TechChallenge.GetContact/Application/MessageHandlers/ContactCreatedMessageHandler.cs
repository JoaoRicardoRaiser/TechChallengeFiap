using Raisersoft.EasyRabbit.Interfaces;
using TechChallenge.GetContact.Application.Dtos.Events;
using TechChallenge.GetContact.Application.Interfaces;


namespace TechChallenge.GetContact.Application.MessageHandlers
{
    public class ContactCreatedMessageHandler(IContactService contactService) : IMessageHandler<ContactCreatedEventDto>
    {       
        public async Task HandleAsync(ContactCreatedEventDto message)
        => await contactService.CreateAsync(message);
    }
}
