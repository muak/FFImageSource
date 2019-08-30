using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FFImageLoading.Svg.Platform;
using FFImageLoading.Work;
using Xamarin.Forms;

namespace FFImageLoading.Forms
{
    public class SvgStreamFFImageSource : StreamFFImageSource, IVectorImageSourceEx
    {
        public Xamarin.Forms.ImageSource ImageSource => throw new NotImplementedException();

        public int VectorWidth { get; set; }
        public int VectorHeight { get; set; }
        public bool UseDipUnits { get; set; } = true;
        public Dictionary<string, string> ReplaceStringMap { get; set; }
        public Color FillColor { get; set; }

        public static SvgStreamFFImageSource Create(
            Func<Stream> stream, string key, int vectorWidth, int vectorHeight,
            Color fillColor, Dictionary<string, string> replaceStringMap, FFImageSourceOptions options)
        {
            if (stream == null)
            {
                return null;
            }

            key = key ?? stream.GetHashCode().ToString();


            return new SvgStreamFFImageSource
            {
                Stream = token => Task.Run(() => stream(), token),
                Key = key,
                FillColor = fillColor,
                VectorWidth = vectorWidth,
                VectorHeight = vectorHeight,
                ReplaceStringMap = replaceStringMap,
                Options = options,
            };
        }

        public IVectorDataResolver GetVectorDataResolver()
        {
            if(ReplaceStringMap == null)
            {
                ReplaceStringMap = SvgUtility.GetReplaceStringMapForFillColor(FillColor);
            }
            return new SvgDataResolver(VectorWidth, VectorHeight, true, ReplaceStringMap);
        }

        public override Work.TaskParameter GetTaskParameter()
        {
            var task = ImageService.Instance.LoadStream(Stream)
                                   .CacheKey(Key)
                                   .WithCache(Cache.CacheType.Memory)
                                   .WithCustomDataResolver(GetVectorDataResolver());

            return AddTaskOption(task);
        }

        public IVectorImageSource Clone()
        {
            throw new NotImplementedException();
        }
    }
}
