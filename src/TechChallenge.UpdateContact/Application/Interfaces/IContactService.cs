using TechChallenge.UpdateContact.Application.Dtos;
using TechChallenge.UpdateContact.Application.Dtos.Events;
using TechChallenge.UpdateContact.Domain.Entities;

namespace TechChallenge.UpdateContact.Application.Interfaces;

public interface IContactService
{
    
    Task DeleteAsync(ContactDeletedEventDto dto);
    Task UpdateAsync(Contact dto);
    Task CreateAsync(ContactCreatedEventDto dto);    
}
