using TechChallenge.GetContact.Application.Dtos.Events;
using TechChallenge.GetContact.Domain.Entities;

namespace TechChallenge.GetContact.Application.Interfaces;

public interface IContactService
{
    
    Task DeleteAsync(ContactDeletedEventDto dto);
    Task<IEnumerable<Contact>> GetAsync(int? phoneAreaCode);
    Task UpdateAsync(ContactUpdatedEventDto dto);
    Task CreateAsync(ContactCreatedEventDto dto);    
}
