using System;
using System.Threading;
using System.Threading.Tasks;
using Android.Content;
using Android.Graphics;
using FFImageLoading.Drawables;
using FFImageLoading.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace FFImageLoading.Forms.Droid
{
    public class FFImageSourceHandler : IImageSourceHandler
    {
        public async Task<Bitmap> LoadImageAsync(ImageSource imagesource, Context context, CancellationToken cancelationToken = default(CancellationToken))
        {
            var ffImageSource = imagesource as FFImageSource;

            ffImageSource.LoadingStarted();
            ffImageSource.RegisterCancelToken(cancelationToken);

            SelfDisposingBitmapDrawable bitmapDrawable = null;
            try
            {
                bitmapDrawable = await Task.Run(() => ffImageSource.TaskParameter.AsBitmapDrawableAsync(), cancelationToken).ConfigureAwait(false);

                ffImageSource.LoadingCompleted(false);
            }
            catch (OperationCanceledException)
            {
                ffImageSource.LoadingCompleted(true);
                throw;
            }

            return bitmapDrawable.Bitmap;
        }
    }
}
