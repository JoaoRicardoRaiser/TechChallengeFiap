using TechChallenge.GetContact.Application.Dtos.Events;
using TechChallenge.UpdateContact.Application.Dtos;
using TechChallenge.UpdateContact.Application.Dtos.Events;
using TechChallenge.UpdateContact.Domain.Entities;

namespace TechChallenge.UpdateContact.Application.Interfaces;

public interface IContactService
{
    
    Task DeleteAsync(ContactDeletedEventDto dto);
    Task<IEnumerable<Contact>> GetAsync(int? phoneAreaCode);
    Task UpdateAsync(ContactUpdatedEventDto dto);
    Task CreateAsync(ContactCreatedEventDto dto);    
}
