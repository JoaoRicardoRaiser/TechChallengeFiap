using System.Diagnostics.CodeAnalysis;
using TechChallenge.CreateContact.Domain.Entities;
using TechChallenge.CreateContact.Domain.Interfaces;
using TechChallenge.CreateContact.Infrastructure.Interfaces;

namespace TechChallenge.CreateContact.Infrastructure.Cache;

[ExcludeFromCodeCoverage]
public class CacheWarmUpService(
    ILogger<CacheWarmUpService> logger,
    IPhoneAreaCache phoneAreaCache,
    IRepository<PhoneArea> phoneAreaRepository) : ICacheWarmUpService
{
    public async Task WarmUp()
        => await WarmUpPhoneArea();

    private async Task WarmUpPhoneArea()
    {
        var allPhoneArea = await phoneAreaRepository.GetAllAsync();

        foreach (var phoneArea in allPhoneArea)
            phoneAreaCache.Add(phoneArea);

        logger.LogInformation($"Cached {allPhoneArea.Count()} {nameof(PhoneArea)} entities!");
    }
}

