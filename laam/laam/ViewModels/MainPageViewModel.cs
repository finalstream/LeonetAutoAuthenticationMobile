using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using laam.Models;
using Plugin.Toasts;
using Prism.AppModel;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;

namespace laam.ViewModels
{
    public class MainPageViewModel : BindableBase, INavigationAware, IApplicationLifecycle
    {
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private string _leonetId;
        public string LeonetId
        {
            get { return _leonetId; }
            set
            {
                Application.Current.Properties["LeonetId"] = value;
                CanLogon = !string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(LeonetPassword);
                SetProperty(ref _leonetId, value);
            }
        }

        private string _leonetPassword;
        public string LeonetPassword
        {
            get { return _leonetPassword; }
            set
            {
                Application.Current.Properties["LeonetPassword"] = value;
                CanLogon = !string.IsNullOrEmpty(LeonetId) && !string.IsNullOrEmpty(value);
                SetProperty(ref _leonetPassword, value);
            }
        }

        private bool _canLogon;
        public bool CanLogon
        {
            get { return _canLogon; }
            set
            {
                SetProperty(ref _canLogon, value);
            }
        }

        public ICommand LogonCommand { get; }


        public LeonetConnecter LeonetConnecter { get; private set; }

        public MainPageViewModel(IWifiService wifiService)
        {
            LeonetConnecter = new LeonetConnecter(wifiService);
            LeonetConnecter.Connected += async (sender, args) =>
            {
                var notificator = DependencyService.Get<IToastNotificator>();
                var androidOptions = new AndroidOptions();
                androidOptions.DismissText = "";

                var options = new NotificationOptions()
                {
                    Title = "Success",
                    AndroidOptions = androidOptions
                };

                await notificator.Notify(options);
            };

            LogonCommand = new DelegateCommand(async () =>
                {
                    await LeonetConnecter.ConnectAsync(LeonetId, LeonetPassword);

                })
                .ObservesCanExecute(()=> CanLogon);
        }

        private bool CanLogonCommand()
        {
            return !string.IsNullOrWhiteSpace(LeonetId)
                   && !string.IsNullOrWhiteSpace(LeonetPassword);
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            // 起動時
            if (parameters.ContainsKey("title")) Title = (string)parameters["title"];
            LeonetId = Application.Current.Properties["LeonetId"] as string;
            LeonetPassword = Application.Current.Properties["LeonetPassword"] as string;
        }

        public void OnResume()
        {
            // スリープから復帰
            LeonetId = Application.Current.Properties["LeonetId"] as string;
            LeonetPassword = Application.Current.Properties["LeonetPassword"] as string;
        }

        public void OnSleep()
        {
            
        }

    }
}
