using TechChallenge.UpdateContact.Application.Dtos;
using TechChallenge.UpdateContact.Application.Interfaces;
using TechChallenge.UpdateContact.Domain.Entities;
using TechChallenge.UpdateContact.Infrastructure.Interfaces;

namespace TechChallenge.UpdateContact.Application.MessageHandlers;

    public class ContactMessageHandler(IContactService contactService) : IMessageHandler<UpdateContactDto>
    {
        public async Task Handle(UpdateContactDto message)
            => await contactService.UpdateAsync(message);
    }

