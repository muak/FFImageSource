using System;
using System.Threading.Tasks;
using FFImageLoading.Cache;

namespace FFImageLoading
{
    public interface IImageServiceEx
    {
        Task ForceInvalidateCacheEntryAsync(string key, CacheType cacheType, bool removeSimilar = false);
    }
}
