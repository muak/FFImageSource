using System;
using System.Collections.Generic;
using FFImageLoading.Svg.Platform;
using FFImageLoading.Work;
using Xamarin.Forms;

namespace FFImageLoading.Forms
{
    public class SvgFFImageSource:FFImageSource,IVectorImageSourceEx
    {
        public Xamarin.Forms.ImageSource ImageSource => throw new NotImplementedException();

        public int VectorWidth { get; set; }
        public int VectorHeight { get; set; }
        public bool UseDipUnits { get; set; } = true;
        public Dictionary<string, string> ReplaceStringMap { get; set; }
        public Color FillColor { get; set; }

        public IVectorDataResolver GetVectorDataResolver()
        {
            if (ReplaceStringMap == null)
            {
                ReplaceStringMap = SvgUtility.GetReplaceStringMapForFillColor(FillColor);
            }
            return new SvgDataResolver(VectorWidth, VectorHeight, true, ReplaceStringMap);
        }

        public static SvgFFImageSource Create(string path, int vectorWidth, int vectorHeight,
            Color fillColor, Dictionary<string, string> replaceStringMap, FFImageSourceOptions options)
        {
            if(string.IsNullOrEmpty(path))
            {
                return null;
            }

            return new SvgFFImageSource
            {
                Path = path,
                FillColor = fillColor,
                VectorWidth = vectorWidth,
                VectorHeight = vectorHeight,
                ReplaceStringMap = replaceStringMap,
                Options = options,
            };
        }

        public override TaskParameter GetTaskParameter()
        {
            return base.GetTaskParameter().WithCustomDataResolver(GetVectorDataResolver());
        }

    }
}
