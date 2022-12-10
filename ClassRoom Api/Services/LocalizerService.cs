using ClassRoom_Api.Context;
using Microsoft.Extensions.Caching.Memory;
using System.Globalization;

namespace ClassRoom_Api.Services;
public class LocalizerService
{
    private readonly ApplicationDbContext _context;
    private readonly IMemoryCache _memoryCache;

    public LocalizerService(ApplicationDbContext context, IMemoryCache memoryCache)
    {
        _context = context;
        _memoryCache = memoryCache;
    }

    public string this[string key] => GetLocalizedString(key);

    public string GetLocalizedString(string key)
    {
        var cacheKey = $"localized_{CultureInfo.CurrentCulture.Name}_{key}";

        return _memoryCache.GetOrCreate(cacheKey, entry =>
        {
            var localizedString = _context.LocalizedStrings.FirstOrDefault(local => local.Key == key);
            if (localizedString is null)
                return key;

            var currentCulture = CultureInfo.CurrentCulture.Name;

            var localized = currentCulture.ToLower() switch
            {
                "uz" => localizedString.Uz,
                "en" => localizedString.En,
                "ru" => localizedString.Ru,
                _ => key
            };

            entry.SlidingExpiration = TimeSpan.FromHours(1);

            return localized ?? key;
        });

    }
}
