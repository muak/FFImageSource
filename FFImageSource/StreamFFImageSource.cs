using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FFImageLoading.Forms
{
    public class StreamFFImageSource:FFImageSource,IKeyImageSource
    {
        public string Key { get; set; }

        public static BindableProperty StreamProperty =
            BindableProperty.Create(
                nameof(Stream),
                typeof(Func<CancellationToken, Task<Stream>>),
                typeof(FFImageSource),
                default(Func<CancellationToken, Task<Stream>>),
                defaultBindingMode: BindingMode.OneWay
            );

        public Func<CancellationToken, Task<Stream>> Stream
        {
            get { return (Func<CancellationToken, Task<Stream>>)GetValue(StreamProperty); }
            set { SetValue(StreamProperty, value); }
        }

        public static StreamFFImageSource Create(Func<Stream> stream, string key,FFImageSourceOptions options)
        {
            if(stream == null){
                return null;
            }

            key = key ?? stream.GetHashCode().ToString();

            return new StreamFFImageSource
            {
                Stream = token => Task.Run(()=>stream(),token),
                Key = key,
                Options = options,
            };
        }

        public override Work.TaskParameter GetTaskParameter()
        {
            var task = ImageService.Instance.LoadStream(Stream).CacheKey(Key).WithCache(Cache.CacheType.Memory);

            return AddTaskOption(task);
        }
    }
}
