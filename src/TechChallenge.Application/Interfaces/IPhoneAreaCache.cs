using TechChallenge.Domain.Entities;

namespace TechChallenge.Application.Interfaces;

public interface IPhoneAreaCache
{
    PhoneArea GetByCode(int code);
    void Add(PhoneArea phoneArea);
    bool Exists(int code);
}
