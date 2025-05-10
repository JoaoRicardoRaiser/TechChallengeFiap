using Raisersoft.EasyRabbit.Interfaces;
using TechChallenge.UpdateContact.Application.Dtos.Events;
using TechChallenge.UpdateContact.Application.Interfaces;


namespace TechChallenge.UpdateContact.Application.MessageHandlers
{
    public class ContactCreatedMessageHandler(IContactService contactService) : IMessageHandler<ContactCreatedEventDto>
    {
        public async Task Handle(ContactCreatedEventDto message)
            => await contactService.CreateAsync(message);

        public Task HandleAsync(ContactCreatedEventDto message)
        {
            throw new NotImplementedException();
        }
    }
}
