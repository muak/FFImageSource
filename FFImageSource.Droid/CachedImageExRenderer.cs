using System;
using FFImageLoading.Forms.Platform;
using Xamarin.Forms.Platform.Android;
using Android.Content;
using FFImageLoading.Cache;
using FFImageLoading.Drawables;
using System.Reflection;

namespace FFImageLoading.Forms.Droid
{
    /// <summary>
    /// CachedImageRenderer which has DefaultCacheFactory as the default value of CacheKeyFactory.
    /// This renderer is the same as CachedImageRenderer except for it.
    /// </summary>
    public class CachedImageExRenderer : Platform.CachedImageRenderer
    {
        public CachedImageExRenderer(Context context) : base(context) { }

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

    /// <summary>
    /// CachedImageFastRenderer which has DefaultCacheFactory as the default value of CacheKeyFactory.
    /// This renderer is the same as CachedImageFastRenderer except for it.
    /// </summary>
    public class CachedImageExFastRenderer : Platform.CachedImageFastRenderer
    {

        public CachedImageExFastRenderer(Context context) : base(context)
        {
            ElementChanged += CachedImageExFastRenderer_ElementChanged;
        }

        void CachedImageExFastRenderer_ElementChanged(object sender, VisualElementChangedEventArgs e)
        {
            if (e.NewElement != null)
            {
                var control = e.NewElement as CachedImage;
                if (control.CacheKeyFactory == null)
                {
                    control.CacheKeyFactory = new DefaultCacheFactory();
                }
                ElementChanged -= CachedImageExFastRenderer_ElementChanged;
            }
            else
            {
                ElementChanged -= CachedImageExFastRenderer_ElementChanged;
            }
        }

        protected override void Dispose(bool disposing)
        {
            ElementChanged -= CachedImageExFastRenderer_ElementChanged;
            base.Dispose(disposing);
        }
    }
}
