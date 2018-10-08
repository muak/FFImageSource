using Android.Content;

namespace FFImageLoading.Forms.Droid
{
    public static class FFImageSourceInitializer
    {
        public static void Init(Context context)
        {
            Xamarin.Forms.Internals.Registrar.Registered.Register(typeof(FFImageSource), typeof(FFImageSourceHandler));
            Xamarin.Forms.Internals.Registrar.Registered.Register(typeof(SvgStreamFFImageSource), typeof(FFImageSourceHandler));
            Xamarin.Forms.Internals.Registrar.Registered.Register(typeof(StreamFFImageSource), typeof(FFImageSourceHandler));
            Xamarin.Forms.Internals.Registrar.Registered.Register(typeof(SvgFFImageSource), typeof(FFImageSourceHandler));

            var handlerType = Xamarin.Forms.Internals.Registrar.Registered.GetHandlerType(typeof(CachedImage));
            if (handlerType == typeof(Platform.CachedImageFastRenderer))
            {
                Xamarin.Forms.Internals.Registrar.Registered.Register(typeof(CachedImage), typeof(CachedImageExFastRenderer));
            }
            else
            {
                Xamarin.Forms.Internals.Registrar.Registered.Register(typeof(CachedImage), typeof(CachedImageExRenderer));
            }
        }
    }
}
