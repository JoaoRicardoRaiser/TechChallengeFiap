using TechChallenge.Domain.Entities;

namespace TechChallenge.Application.Interfaces;

public interface IPhoneAreaCache
{
    void Add(PhoneArea phoneArea);
    bool ExistsAsync(int phoneAreaCode);
}
