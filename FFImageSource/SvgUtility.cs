using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SkiaSharp;
using Xamarin.Forms;

namespace FFImageLoading.Forms
{
    /// <summary>
    /// Svg utility.
    /// </summary>
    public static class SvgUtility
    {

        public static Dictionary<string, string> GetReplaceStringMapForFillColor(Color fillColor)
        {
            if (fillColor.IsDefault)
            {
                return null;
            }

            var hex = $"#{(int)(fillColor.R * 255):X2}{(int)(fillColor.G * 255):X2}{(int)(fillColor.B * 255):X2}";
            var replaceStringMap = new Dictionary<string, string>
                {
                    {"regex:fill=\"#.+?\"",""},
                    {"<svg",$"<svg fill=\"{hex}\" "}
                };

            return replaceStringMap;
        }
    }
}
