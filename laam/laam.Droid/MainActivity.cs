using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Autofac;
using laam.Models;
using Plugin.Toasts;
using Prism.Autofac.Forms;
using Xamarin.Forms;

namespace laam.Droid
{
    [Activity(Label = "laam", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.tabs;
            ToolbarResource = Resource.Layout.toolbar;

            base.OnCreate(bundle);

            DependencyService.Register<ToastNotification>(); // Register your dependency
            // If you are using Android you must pass through the activity
            var platformOptions = new PlatformOptions();
            platformOptions.Style = NotificationStyle.Snackbar;
            ToastNotification.Init(this, platformOptions);


            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App(new AndroidInitializer(this)));
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        private readonly Context _context;


        public AndroidInitializer(Context context)
        {
            _context = context;
        }

        public void RegisterTypes(IContainer container)
        {
            var builder = new ContainerBuilder();
            builder.Register(ctx => { return new WifiService(_context); }).As<IWifiService>().SingleInstance();
            //builder.RegisterType<WifiService>().As<IWifiService>().SingleInstance().WithParameter("context", this);
            builder.Update(container);
        }
    }
}

