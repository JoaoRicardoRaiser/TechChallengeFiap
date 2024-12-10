using Microsoft.Extensions.Caching.Memory;
using TechChallenge.Application.Interfaces;
using TechChallenge.Domain.Entities;

namespace TechChallenge.Infrastructure.Cache;

public class PhoneAreaCache(IMemoryCache memoryCache) : IPhoneAreaCache
{
    protected readonly string _keyPrefix = $"{nameof(PhoneArea)}_";

    public void Add(PhoneArea phoneArea)
        => memoryCache.Set(GetFormattedKey(phoneArea.Code), phoneArea.Code);

    public bool ExistsAsync(int phoneAreaCode)
        => memoryCache.TryGetValue(GetFormattedKey(phoneAreaCode), out _);

    private string GetFormattedKey(int phoneAreaCode)
        => _keyPrefix + phoneAreaCode;
}
