using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using SkiaSharp;
using Xamarin.Forms;

namespace FFImageLoading.Forms
{
    public class FFImageSource : ImageSource
    {
        public static BindableProperty PathProperty =
            BindableProperty.Create(
                nameof(Path),
                typeof(string),
                typeof(FFImageSource),
                default(string),
                defaultBindingMode: BindingMode.OneWay
            );

        public string Path
        {
            get { return (string)GetValue(PathProperty); }
            set { SetValue(PathProperty, value); }
        }

        public static BindableProperty OptionsProperty =
            BindableProperty.Create(
                nameof(Options),
                typeof(FFImageSourceOptions),
                typeof(FFImageSource),
                default(FFImageSourceOptions),
                defaultBindingMode: BindingMode.OneWay
            );

        public FFImageSourceOptions Options
        {
            get { return (FFImageSourceOptions)GetValue(OptionsProperty); }
            set { SetValue(OptionsProperty, value); }
        }

        Work.TaskParameter _taskParameter;
        public Work.TaskParameter TaskParameter { 
            get {
                _taskParameter = _taskParameter ?? GetTaskParameter();
                return _taskParameter;
            }
        }

        public void ClearTaskParameter()
        {
            _taskParameter?.TryDispose();
            _taskParameter = null;
        }

        public static Assembly AssemblyCache;

        /// <summary>
        /// Registers the assembly.
        /// </summary>
        /// <param name="typeHavingResource">Type having resource.</param>
        public static void RegisterAssembly(Type typeHavingResource = null)
        {
            if (typeHavingResource == null)
            {
#if NETSTANDARD2_0
                AssemblyCache = Assembly.GetCallingAssembly();
#else
                MethodInfo callingAssemblyMethod = typeof(Assembly).GetTypeInfo().GetDeclaredMethod("GetCallingAssembly");
                if (callingAssemblyMethod != null)
                {
                    AssemblyCache = (Assembly)callingAssemblyMethod.Invoke(null, new object[0]);
                }
#endif
            }
            else
            {
                AssemblyCache = typeHavingResource.GetTypeInfo().Assembly;
            }
        }

        public static string GetRealResourcePath(string path)
        {
            return AssemblyCache.GetManifestResourceNames()
                              .FirstOrDefault(x => x.EndsWith(path, StringComparison.CurrentCultureIgnoreCase));

        }

        public static FFImageSource Create(string path, FFImageSourceOptions options)
        {
            if(string.IsNullOrEmpty(path))
            {
                return null;
            }

            return new FFImageSource
            {
                Path = path,
                Options = options,
            };
        }

        Work.TaskParameter ParsePath(string path, TimeSpan? cacheDuration = null)
        {
            if (Uri.TryCreate(path, UriKind.Absolute, out var uri))
            {
                if (uri.Scheme.Equals("data", StringComparison.OrdinalIgnoreCase))
                {
                    var encode = uri.LocalPath.Contains(";base64,") ? Work.DataEncodingType.Base64Encoded : Work.DataEncodingType.RAW;

                    if (path.IsSvgDataUrl())
                    {
                        return ImageService.Instance.LoadString(path, encode);
                    }

                    return ImageService.Instance.LoadString(path, encode);
                }
                if (uri.IsFile)
                {
                    return ImageService.Instance.LoadFileFromApplicationBundle(uri.OriginalString.Replace(uri.GetLeftPart(UriPartial.Scheme), ""));
                }
                if (uri.Scheme.Equals("resource", StringComparison.OrdinalIgnoreCase))
                {
                    var realPath = GetRealResourcePath(uri.OriginalString.Replace(uri.GetLeftPart(UriPartial.Scheme), ""));

                    if (path.IsSvgFileUrl())
                    {
                        return ImageService.Instance.LoadEmbeddedResource(realPath, AssemblyCache);
                    }

                    return ImageService.Instance.LoadEmbeddedResource(realPath, AssemblyCache);
                }

                if (path.IsSvgFileUrl())
                {
                    return ImageService.Instance.LoadUrl(path, cacheDuration);
                }

                return ImageService.Instance.LoadUrl(path, cacheDuration);
            }

            if (path.IsSvgDataUrl())
            {
                return ImageService.Instance.LoadString(path);
            }

            if (path.IsSvgFileUrl())
            {
                return ImageService.Instance.LoadEmbeddedResource(GetRealResourcePath(path), AssemblyCache);
            }

            if (!string.IsNullOrWhiteSpace(path))
            {
                // return EmbeddedReource if no scheme path.
                return ImageService.Instance.LoadEmbeddedResource(GetRealResourcePath(path), AssemblyCache);
            }

            return null;
        }

        public void RegisterCancelToken(CancellationToken token)
        {
            token.Register(CancellationTokenSource.Cancel);
        }

        public void LoadingStarted()
        {
            OnLoadingStarted();
        }

        public void LoadingCompleted(bool cancelled)
        {
            OnLoadingCompleted(cancelled);
        }

        public virtual Work.TaskParameter GetTaskParameter()
        {
            var task =  ParsePath(Path, Options?.CacheDuration);
            return AddTaskOption(task);
        }

        protected virtual Work.TaskParameter AddTaskOption(Work.TaskParameter taskParameter)
        {
            if(Options == null)
            {
                return taskParameter;
            }

            if(Options.DownSampleWidth > 0 || Options.DownSampleHeight > 0)
            {
                taskParameter.DownSample(Options.DownSampleWidth, Options.DownSampleHeight);
            }

            return taskParameter;
        }

        // ListViewの場合に難ありな気がする 様々な問題を含むのでPlaceholderとErrorPlaceholderはやめる
        //protected void SetImageSourceOptions(Work.TaskParameter contentTask, FFImageSourceOptions options)
        //{
        //    CancellationTokenSource?.Cancel();
        //    if (!string.IsNullOrEmpty(options.ErrorPlaceholderPath))
        //    {
        //        contentTask.Error(ex =>
        //        {
        //            //TaskParameter?.TryDispose();
        //            TaskParameter = ParsePath(options.ErrorPlaceholderPath, options.ErrorPlaceholderSvgOptions, options.CacheDuration);
        //            Device.BeginInvokeOnMainThread(() => OnSourceChanged());
        //        });
        //    }

        //    if (!string.IsNullOrEmpty(options.PlaceholderPath))
        //    {
        //        //TaskParameter?.TryDispose();
        //        TaskParameter = ParsePath(options.PlaceholderPath, options.PlaceholderSvgOptions, options.CacheDuration).Finish(_ =>
        //        {
        //            TaskParameter?.TryDispose();
        //            TaskParameter = contentTask;
        //            Device.BeginInvokeOnMainThread(() => OnSourceChanged());
        //        });
        //    }
        //    else
        //    {
        //        //TaskParameter?.TryDispose();
        //        TaskParameter = contentTask;
        //    }
        //}

        //protected virtual string GenerateKey()
        //{
        //    var colorSeg = Color.IsDefault ? "" : FormatColorSegment(Color);

        //    return $"{Path}&width={Size.Width}&height={Size.Height}&{colorSeg}";
        //}

        //protected string FormatColorSegment(Color color)
        //{
        //    return $"color=A{color.A}R{color.R}G{color.G}B{color.B}";
        //}
    }

    public class FFImageSourceOptions
    {
        //public string PlaceholderPath { get; set; }
        //public SvgOptions PlaceholderSvgOptions { get; set; }
        //public string ErrorPlaceholderPath { get; set; }
        //public SvgOptions ErrorPlaceholderSvgOptions { get; set; }
        public TimeSpan? CacheDuration { get; set; }
        public int DownSampleWidth { get; set; }
        public int DownSampleHeight { get; set; }
    }
}
