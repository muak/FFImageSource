using System;
using System.Threading;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace FFImageLoading.Forms.iOS
{
    public class FFImageSourceHandler : IImageSourceHandler
    {
        public async Task<UIImage> LoadImageAsync(ImageSource imagesource, CancellationToken cancelationToken = default(CancellationToken), float scale = 1)
        {
            var ffImageSource = imagesource as FFImageSource;

            ffImageSource.LoadingStarted();
            ffImageSource.RegisterCancelToken(cancelationToken);

            UIImage image = null;
            var taskParameter = ffImageSource.GetTaskParameter();
            try
            {
                image = await Task.Run(() => taskParameter.AsUIImageAsync(), cancelationToken);
                ffImageSource.LoadingCompleted(false);
            }
            catch (OperationCanceledException)
            {
                ffImageSource.LoadingCompleted(true);
                throw;
            }
            finally
            {
                taskParameter.Dispose();
            }

            return image;
        }
    }
}
