using System;
using Xamarin.Forms;
using System.IO;
using System.Reflection;

namespace FFImageLoading.Forms
{
    public static class FFSourceFactory
    {
        public static ImageSource FromSvgStream(Func<Stream> stream, string key = null,
                                                int vectorWidth = 0, int vectorHeight = 0,
                                                Color fillColor = default(Color),FFImageSourceOptions options = null)
        {
            return SvgStreamFFImageSource.Create(stream, key, vectorWidth, vectorHeight, fillColor,null, options);
        }

        public static ImageSource FromStream(Func<Stream> stream, string key = null, FFImageSourceOptions options = null)
        {
            return StreamFFImageSource.Create(stream, key, options);
        }

        public static ImageSource FromSvgPath(string path, 
                                              int vectorWidth = 0, int vectorHeight = 0,
                                              Color fillColor = default(Color), FFImageSourceOptions options = null)
        {
            FFImageSource.AssemblyCache =  FFImageSource.AssemblyCache ?? Assembly.GetCallingAssembly();

            return SvgFFImageSource.Create(path, vectorWidth, vectorHeight, fillColor, null, options);
        }

        public static ImageSource FromPath(string path, FFImageSourceOptions options = null)
        {
            FFImageSource.AssemblyCache = FFImageSource.AssemblyCache ?? Assembly.GetCallingAssembly();

            return FFImageSource.Create(path, options);
        }
    }
}
