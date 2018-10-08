using System;
using Xamarin.Forms;

namespace FFImageLoading.Forms
{
    public class DefaultCacheFactory : ICacheKeyFactory
    {
        public DefaultCacheFactory(){}

        public string GetKey(ImageSource imageSource, object bindingContext)
        {
            var keyImageSource = imageSource as IKeyImageSource;
           
            return keyImageSource?.Key;
        }
    }
}
