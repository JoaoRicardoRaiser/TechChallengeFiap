using TechChallenge.UpdateContact.Application.Dtos;
using TechChallenge.UpdateContact.Application.Dtos.Events;

namespace TechChallenge.UpdateContact.Application.Interfaces;

public interface IContactService
{
    Task DeleteAsync(ContactDeletedEventDto dto);
    Task UpdateAsync(UpdateContactDto dto);
    Task CreateAsync(ContactCreatedEventDto dto);    
}
