using Microsoft.Extensions.Caching.Memory;
using NLog;
using System;
using System.Collections.Concurrent;
using System.Runtime.Caching;
using System.Threading;
using System.Threading.Tasks;

public class WaitToFinishMemoryCache<TItem>
{
    private static Logger logger = LogManager.GetCurrentClassLogger();
    private Microsoft.Extensions.Caching.Memory.MemoryCache _cache = new Microsoft.Extensions.Caching.Memory.MemoryCache(new MemoryCacheOptions());
    private ConcurrentDictionary<object, SemaphoreSlim> _locks = new ConcurrentDictionary<object, SemaphoreSlim>();

    public async Task<TItem> GetOrCreate(object key, Func<Task<TItem>> createItem)
    {
        TItem cacheEntry;
        bool isCache = true;
        if (!_cache.TryGetValue(key, out cacheEntry))// Look for cache key.
        {
            SemaphoreSlim mylock = _locks.GetOrAdd(key, k => new SemaphoreSlim(1, 1));

            await mylock.WaitAsync();
            try
            {
                if (!_cache.TryGetValue(key, out cacheEntry))
                {
                    // Key not in cache, so get data.
                    isCache = false;
                    cacheEntry = await createItem();
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSize(1)//Size amount
                        //Priority on removing when reaching size limit (memory pressure)
                        .SetPriority(Microsoft.Extensions.Caching.Memory.CacheItemPriority.Low)
                        // Keep in cache for this time, reset time if accessed.
                        .SetSlidingExpiration(TimeSpan.FromSeconds(2))
                        // Remove from cache after this time, regardless of sliding expiration
                        .SetAbsoluteExpiration(TimeSpan.FromSeconds(10));

                    _cache.Set(key, cacheEntry, cacheEntryOptions);
                }
            }
            finally
            {
                mylock.Release();
            }
        }
        if (isCache)
        {
            logger.Info($"Cache : {cacheEntry.ToString()}");
        }
        return cacheEntry;
    }
}