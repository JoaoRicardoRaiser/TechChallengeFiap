using TechChallenge.UpdateContact.Domain.Entities;

namespace TechChallenge.UpdateContact.Infrastructure.Interfaces;

public interface IPhoneAreaCache
{
    PhoneArea GetByCode(int code);
    void Add(PhoneArea phoneArea);
    bool Exists(int code);
}
