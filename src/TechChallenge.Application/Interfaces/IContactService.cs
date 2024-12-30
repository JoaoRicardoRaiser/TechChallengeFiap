using TechChallenge.Application.Dtos;
using TechChallenge.Domain.Entities;

namespace TechChallenge.Application.Interfaces;

public interface IContactService
{
    Task<IEnumerable<Contact>> GetAsync(int? phoneAreaCode);
    Task CreateAsync(CreateContactDto dto);
    Task UpdateAsync(UpdateContactDto dto);
    Task DeleteAsync(Guid contactId);
}
