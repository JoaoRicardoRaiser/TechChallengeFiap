using TechChallenge.DeleteContact.Application.Dtos.Events;

namespace TechChallenge.DeleteContact.Application.Interfaces;

public interface IContactService
{
    Task CreateAsync(ContactCreatedEventDto dto);
    Task UpdateAsync(ContactUpdatedEventDto dto);
    Task DeleteAsync(Guid contactId);
}
