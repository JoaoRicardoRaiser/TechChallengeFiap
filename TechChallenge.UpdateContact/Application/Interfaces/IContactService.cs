using TechChallenge.CreateContact.Application.Dtos;
using TechChallenge.CreateContact.Application.Dtos.Events;
using TechChallenge.UpdateContact.Application.Dtos;

namespace TechChallenge.CreateContact.Application.Interfaces;

public interface IContactService
{
    
    Task DeleteAsync(ContactDeletedEventDto dto);
    Task UpdateAsync(UpdateContactDto dto);

}
