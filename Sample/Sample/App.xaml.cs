﻿using Prism;
using Prism.Ioc;
using Prism.Unity;
using Sample.Views;
using Xamarin.Forms;
using FFImageLoading.Forms;

namespace Sample
{
    public partial class App : PrismApplication
	{
		public App(IPlatformInitializer initializer = null) : base(initializer) { }


        protected override void OnInitialized()
		{
			InitializeComponent();

            FFImageSource.RegisterAssembly();

			NavigationService.NavigateAsync("/NavigationPage/MainPage");
		}

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MainPage>();
            containerRegistry.RegisterForNavigation<NavigationPage>();
        }
    }
}

