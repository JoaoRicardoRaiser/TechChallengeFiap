using TechChallenge.Application.Dtos;
using TechChallenge.Domain.Entities;

namespace TechChallenge.Application.Interfaces;

public interface IContactService
{
    Task<IEnumerable<Contact>> GetAsync(int? phoneAreaCode);
    Task Create(CreateContactDto dto);
    Task Update(UpdateContactDto dto);
}
