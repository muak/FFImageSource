using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using static FFImageLoading.Forms.SvgUtility;
using FFImageLoading.Forms;
using System.Linq;
using System.Reflection;

namespace FFImageLoading.Forms
{
    public static class CachedSourceFactory
    {
        public static KeyStreamImageSource FromStream(Func<Stream> stream, string key)
        {
            return new KeyStreamImageSource
            {
                Stream = token => Task.Run(() => stream(), token),
                Key = key
            };
        }

        public static SvgExImageSource FromSvgStream(Func<Stream> stream, string key, int vectorWidth = 0, int vectorHeight = 0, Color fillColor = default(Color))
        {
            return new SvgExImageSource(key, fillColor, ImageSource.FromStream(stream),
                                        vectorWidth, vectorHeight, true, GetReplaceStringMapForFillColor(fillColor));
        }

        public static SvgExImageSource FromSvgUri(Uri uri, int vectorWidth = 0, int vectorHeight = 0, Color fillColor = default(Color))
        {
            return new SvgExImageSource(null, fillColor, ImageSource.FromUri(uri),
                                        vectorWidth, vectorHeight, true, GetReplaceStringMapForFillColor(fillColor));
        }

        public static SvgExImageSource FromSvgResource(string resource, int vectorWidth = 0, int vectorHeight = 0, Color fillColor = default(Color))
        {
            FFImageSource.AssemblyCache = FFImageSource.AssemblyCache ?? Assembly.GetCallingAssembly();
            return new SvgExImageSource(null, fillColor, new EmbeddedResourceImageSource(FFImageSource.GetRealResourcePath(resource),FFImageSource.AssemblyCache),
                                        vectorWidth, vectorHeight, true, GetReplaceStringMapForFillColor(fillColor));
        }

        public static SvgExImageSource FromSvgString(string svg, int vectorWidth = 0, int vectorHeight = 0, Color fillColor = default(Color))
        {
            return new SvgExImageSource(null,fillColor,new DataUrlImageSource(svg), 
                                        vectorWidth, vectorHeight, true,GetReplaceStringMapForFillColor(fillColor));
        }

    }
}
