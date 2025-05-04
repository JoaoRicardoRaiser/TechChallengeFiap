using TechChallenge.UpdateContact.Application.Interfaces;
using TechChallenge.UpdateContact.Domain.Entities;
using TechChallenge.UpdateContact.Infrastructure.Interfaces;

namespace TechChallenge.UpdateContact.Application.MessageHandlers;

    public class ContactMessageHandler(IContactService contactService) : IMessageHandler<Contact>
    {
        public async Task Handle(Contact message)
            => await contactService.UpdateAsync(message);
    }

