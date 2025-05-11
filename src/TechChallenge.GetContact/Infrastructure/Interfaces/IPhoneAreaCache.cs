using TechChallenge.GetContact.Domain.Entities;

namespace TechChallenge.GetContact.Infrastructure.Interfaces;

public interface IPhoneAreaCache
{
    PhoneArea GetByCode(int code);
    void Add(PhoneArea phoneArea);
    bool Exists(int code);
}
