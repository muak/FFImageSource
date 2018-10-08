using System;
using FFImageLoading.Forms.Platform;
using Xamarin.Forms.Platform.iOS;

namespace FFImageLoading.Forms.iOS
{
    /// <summary>
    /// CachedImageRenderer which has DefaultCacheFactory as the default value of CacheKeyFactory.
    /// This renderer is the same as CachedImageRenderer except for it.
    /// </summary>
    public class CachedImageExRenderer : CachedImageRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<CachedImage> e)
        {
            if (e.NewElement != null)
            {
                if (e.NewElement.CacheKeyFactory == null)
                {
                    e.NewElement.CacheKeyFactory = new DefaultCacheFactory();
                }
            }
            base.OnElementChanged(e);
        }
    }
}
