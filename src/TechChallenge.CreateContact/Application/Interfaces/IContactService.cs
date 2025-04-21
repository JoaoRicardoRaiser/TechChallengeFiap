using TechChallenge.CreateContact.Application.Dtos;
using TechChallenge.CreateContact.Application.Dtos.Events;

namespace TechChallenge.CreateContact.Application.Interfaces;

public interface IContactService
{
    Task CreateAsync(CreateContactDto dto);
    Task DeleteAsync(ContactDeletedEventDto dto);
}
