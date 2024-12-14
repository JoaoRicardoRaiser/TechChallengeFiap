using Microsoft.Extensions.Caching.Memory;
using TechChallenge.Application.Interfaces;
using TechChallenge.Domain.Entities;

namespace TechChallenge.Infrastructure.Cache;

public class PhoneAreaCache(IMemoryCache memoryCache) : IPhoneAreaCache
{
    private readonly string _keyPrefix = $"{nameof(PhoneArea)}_";

    public PhoneArea GetByCode(int code)
        => memoryCache.Get<PhoneArea>(GetFormattedKey(code))!;

    public void Add(PhoneArea phoneArea)
        => memoryCache.Set(GetFormattedKey(phoneArea.Code), phoneArea);

    public bool Exists(int code)
        => memoryCache.TryGetValue(GetFormattedKey(code), out _);

    private string GetFormattedKey(int phoneAreaCode)
        => _keyPrefix + phoneAreaCode;
}
