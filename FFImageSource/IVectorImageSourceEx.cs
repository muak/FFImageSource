using System;
using Xamarin.Forms;
namespace FFImageLoading.Forms
{
    public interface IVectorImageSourceEx:IVectorImageSource
    {
        Color FillColor { get; set; }
    }
}
