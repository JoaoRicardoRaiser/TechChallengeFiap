using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics.CodeAnalysis;
using TechChallenge.Application.Interfaces;
using TechChallenge.Domain.Entities;

namespace TechChallenge.Infrastructure.Cache;

[ExcludeFromCodeCoverage]
public class ContactCache(IMemoryCache memoryCache) : IContactCache
{
    private const string _contactsKey = "contacts";

    public bool ContactsOnCache()
    => memoryCache.TryGetValue(_contactsKey, out _);

    public void Add(Contact contact)
    {
        if (!ContactsCached.Contains(contact))
            ContactsCached.Add(contact);
    }

    public void AddRange(IEnumerable<Contact> contacts)
    {
        foreach (var contact in contacts)
            if(!ContactsCached.Contains(contact))
                ContactsCached.Add(contact);
    }

    public IEnumerable<Contact> GetAll()
        =>  ContactsCached!;

    public void Update(Contact contactUpdated)
    {

        var contactOnCache = ContactsCached.FirstOrDefault(x => x.Id == contactUpdated.Id);
        ContactsCached.Remove(contactOnCache!);
        ContactsCached.Add(contactUpdated);
    }

    public void Delete(Contact contact)
    {

        var contactOnCache = ContactsCached.FirstOrDefault(x => x.Id == contact.Id);
        ContactsCached.Remove(contactOnCache!);
    }

    private IList<Contact> ContactsCached
        => memoryCache.GetOrCreate(_contactsKey, entry =>
        {
            return new List<Contact>();
        });
}
