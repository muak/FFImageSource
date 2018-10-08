using System;
using System.Collections.Generic;
using System.IO;
using FFImageLoading.Svg.Forms;
using FFImageLoading.Svg.Platform;
using Xamarin.Forms;

namespace FFImageLoading.Forms
{
    public class SvgExImageSource : SvgImageSource, IKeyImageSource
    {
        /// <summary>
        /// Casually apply a fill color for an SVG file that has only one fill color or no color.
        /// If multi colors, this setting is ignored.
        /// </summary>
        /// <value>The color of the fill.</value>
        public Color FillColor { get; set; }

        public string Key { get; set; }

        public SvgExImageSource(ImageSource imageSource, int vectorWidth, int vectorHeight, bool useDipUnits, Dictionary<string, string> replaceStringMap = null)
            : base(imageSource, vectorWidth, vectorHeight, useDipUnits, replaceStringMap) { }

        public SvgExImageSource(string key, Color fillColor, ImageSource imageSource, int vectorWidth, int vectorHeight, bool useDipUnits, Dictionary<string, string> replaceStringMap = null)
            : base(imageSource, vectorWidth, vectorHeight, useDipUnits, replaceStringMap)
        {
            Key = key;
            FillColor = fillColor;
        }
    }
}
