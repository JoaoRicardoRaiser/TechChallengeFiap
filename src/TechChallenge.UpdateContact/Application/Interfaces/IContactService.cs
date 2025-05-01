using TechChallenge.UpdateContact.Application.Dtos;
using TechChallenge.UpdateContact.Application.Dtos.Events;
using TechChallenge.UpdateContact.Application.Dtos;

namespace TechChallenge.UpdateContact.Application.Interfaces;

public interface IContactService
{
    
    Task DeleteAsync(ContactDeletedEventDto dto);
    Task UpdateAsync(UpdateContactDto dto);

}
