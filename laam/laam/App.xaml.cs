using Autofac;
using Prism.Autofac;
using Prism.Autofac.Forms;
using laam.Views;
using Prism.AppModel;
using Xamarin.Forms;

namespace laam
{
    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer initializer = null) : base(initializer) { }

        protected override void OnInitialized()
        {
            InitializeComponent();

            NavigationService.NavigateAsync("NavigationPage/MainPage?title=Leonet%20Auto%20Authentication");
        }

        protected override void RegisterTypes()
        {
            Container.RegisterTypeForNavigation<NavigationPage>();
            Container.RegisterTypeForNavigation<MainPage>();
        }

        protected override void OnSleep()
        {
            base.OnSleep();
            (MainPage.BindingContext as IApplicationLifecycle)?.OnSleep();
        }

        protected override void OnResume()
        {
            base.OnResume();
            (MainPage.BindingContext as IApplicationLifecycle)?.OnResume();
        }



    }
}
