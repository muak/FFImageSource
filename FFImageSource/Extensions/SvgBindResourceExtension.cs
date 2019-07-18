using System;
using Xamarin.Forms.Xaml;
using Xamarin.Forms;
using System.Runtime.CompilerServices;

namespace FFImageLoading.Forms.Extensions
{
    [ContentProperty("Path")]
    public class SvgBindResourceExtension:BindableObject, IMarkupExtension<BindingBase>
    {
        public string Path { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Color FillColor { get; set; }

        public static BindableProperty InnerPathProperty =
            BindableProperty.Create(
                nameof(InnerPath),
                typeof(string),
                typeof(SvgBindResourceExtension),
                default(string),
                defaultBindingMode: BindingMode.OneWay
            );

        public string InnerPath
        {
            get { return (string)GetValue(InnerPathProperty); }
            set { SetValue(InnerPathProperty, value); }
        }

        public static BindableProperty SourceProperty =
            BindableProperty.Create(
                nameof(Source),
                typeof(ImageSource),
                typeof(SvgBindResourceExtension),
                default(ImageSource),
                defaultBindingMode: BindingMode.OneWay
            );

        public ImageSource Source
        {
            get { return (ImageSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return (this as IMarkupExtension<BindingBase>).ProvideValue(serviceProvider);
        }

        BindingBase IMarkupExtension<BindingBase>.ProvideValue(IServiceProvider serviceProvider)
        {
            var target = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            var targetObject = (BindableObject)target.TargetObject;

            EventHandler bindingChanged = null;
            bindingChanged = (sender, e) => {
                BindingContext = targetObject.BindingContext;
                targetObject.BindingContextChanged -= bindingChanged;
            };

            targetObject.BindingContextChanged += bindingChanged;

            if (!string.IsNullOrEmpty(Path))
            {
                this.SetBinding(InnerPathProperty, Path);
            }

            UpdateSource();

            return new Binding(SourceProperty.PropertyName, BindingMode.Default, null, null, null, this);
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if(propertyName == InnerPathProperty.PropertyName)
            {
                UpdateSource();
            }
            base.OnPropertyChanged(propertyName);
        }

        void UpdateSource()
        {
            if(InnerPath != null)
            {
                Source = CachedSourceFactory.FromSvgResource(InnerPath, Width, Height, FillColor);
            }
        }
    }
}
