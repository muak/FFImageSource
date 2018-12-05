using System;
using System.Threading.Tasks;
using FFImageLoading.Cache;

namespace FFImageLoading
{
    public class ImageServiceEx:IImageServiceEx
    {
        public async Task ForceInvalidateCacheEntryAsync(string key, CacheType cacheType, bool removeSimilar = false)
        {
            await FFImageLoading.ImageService.Instance.InvalidateCacheEntryAsync(key, cacheType, removeSimilar);
        }
    }
}
