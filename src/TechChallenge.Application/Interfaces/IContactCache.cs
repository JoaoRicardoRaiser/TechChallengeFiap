using System.Linq.Expressions;
using TechChallenge.Domain.Entities;

namespace TechChallenge.Application.Interfaces;

public interface IContactCache
{
    bool ContactsOnCache();
    void Add(Contact contact);
    void AddRange(IEnumerable<Contact> contacts);
    IEnumerable<Contact> GetAll();
    void Update(Contact contact);
    void Delete(Contact contactUpdated);
}
