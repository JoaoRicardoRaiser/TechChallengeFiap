using TechChallenge.CreateContact.Domain.Entities;

namespace TechChallenge.CreateContact.Infrastructure.Interfaces;

public interface IPhoneAreaCache
{
    PhoneArea GetByCode(int code);
    void Add(PhoneArea phoneArea);
    bool Exists(int code);
}
