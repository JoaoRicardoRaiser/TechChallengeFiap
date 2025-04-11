using TechChallenge.CreateContact.Application.Dtos;

namespace TechChallenge.CreateContact.Application.Interfaces;

public interface IContactService
{
    Task CreateAsync(CreateContactDto dto);
}
