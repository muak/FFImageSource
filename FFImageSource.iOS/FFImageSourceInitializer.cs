namespace FFImageLoading.Forms.iOS
{
    public static class FFImageSourceInitializer
    {
        public static void Init()
        {
            Xamarin.Forms.Internals.Registrar.Registered.Register(typeof(FFImageSource), typeof(FFImageSourceHandler));
            Xamarin.Forms.Internals.Registrar.Registered.Register(typeof(SvgStreamFFImageSource), typeof(FFImageSourceHandler));
            Xamarin.Forms.Internals.Registrar.Registered.Register(typeof(StreamFFImageSource), typeof(FFImageSourceHandler));
            Xamarin.Forms.Internals.Registrar.Registered.Register(typeof(SvgFFImageSource), typeof(FFImageSourceHandler));
            Xamarin.Forms.Internals.Registrar.Registered.Register(typeof(CachedImage), typeof(CachedImageExRenderer));
        }
    }
}
