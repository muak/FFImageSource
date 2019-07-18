using System;
using Xamarin.Forms.Xaml;
using Xamarin.Forms;
namespace FFImageLoading.Forms.Extensions
{
    [ContentProperty("Path")]
    public class SvgResourceExtension:IMarkupExtension<ImageSource>
    {
        public string Path { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Color FillColor { get; set; }

        public ImageSource ProvideValue(IServiceProvider serviceProvider)
        {
            if (Path != null)
            {
                return CachedSourceFactory.FromSvgResource(Path, Width, Height, FillColor);
            }

            return null;
        }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            return (this as IMarkupExtension<ImageSource>).ProvideValue(serviceProvider);
        }
    }
}
