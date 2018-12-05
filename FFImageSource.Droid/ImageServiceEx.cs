using System;
using System.Reflection;
using System.Threading.Tasks;
using FFImageLoading.Cache;
using FFImageLoading.Drawables;

namespace FFImageLoading
{
public class ImageServiceEx:IImageServiceEx
{
    static ByteBoundStrongLruCache<SelfDisposingBitmapDrawable> _reusePool;
    static ByteBoundStrongLruCache<SelfDisposingBitmapDrawable> ReusePool
    {
        get
        {
            if (_reusePool != null)
                return _reusePool;

            var imageCache = ImageCache.Instance as ImageCache<SelfDisposingBitmapDrawable>;
            var fieldInfo = imageCache.GetType().GetField("_cache", BindingFlags.Instance | BindingFlags.NonPublic);
            var innerCache = (ReuseBitmapDrawableCache<SelfDisposingBitmapDrawable>)fieldInfo.GetValue(imageCache);
            fieldInfo = innerCache.GetType().GetField("reuse_pool", BindingFlags.Instance | BindingFlags.NonPublic);
            _reusePool = (ByteBoundStrongLruCache<SelfDisposingBitmapDrawable>)fieldInfo.GetValue(innerCache);

            return _reusePool;
        }
    }

    public async Task ForceInvalidateCacheEntryAsync(string key, CacheType cacheType, bool removeSimilar = false)
    {
        await ImageService.Instance.InvalidateCacheEntryAsync(key, cacheType, removeSimilar);
        ReusePool.Remove(key);
    }
}
}
