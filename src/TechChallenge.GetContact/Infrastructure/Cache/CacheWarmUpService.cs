using System.Diagnostics.CodeAnalysis;
using TechChallenge.GetContact.Domain.Entities;
using TechChallenge.GetContact.Domain.Interfaces;
using TechChallenge.GetContact.Infrastructure.Interfaces;

namespace TechChallenge.GetContact.Infrastructure.Cache;

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

