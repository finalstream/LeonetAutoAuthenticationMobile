using System;
using System.Net;
using System.Threading.Tasks;
using Android.Content;
using Android.Net;
using Android.Net.Wifi;
using Android.OS;
using Android.Telephony;
using Java.Lang.Reflect;
using Java.Net;
using laam.Models;
using Debug = System.Diagnostics.Debug;

namespace laam.Droid
{
    public class WifiService : IWifiService
    {
        private readonly WifiManager _wifiManager;
        private readonly ConnectivityManager _connectivityManager;
        private readonly TelephonyManager _telephonyManager;
        private readonly ContentResolver _contextResolver;

        public WifiService()
        {
            
        }

        public WifiService(Context context)
        {
            _wifiManager = (WifiManager) context.GetSystemService(Context.WifiService);
            _connectivityManager = (ConnectivityManager) context.GetSystemService(Context.ConnectivityService);
            _telephonyManager = (TelephonyManager) context.GetSystemService(Context.TelephonyService);

            _contextResolver = context.ContentResolver;
        }

        public string GetDefaultGateway()
        {

            var defaultGateway = new IPAddress(_wifiManager.DhcpInfo.Gateway);

            return defaultGateway.ToString();
        }

        public void Connect(string leonetId, string leonetPassword)
        {

            NetworkInfo activeConnection = _connectivityManager.ActiveNetworkInfo;

            bool isOnline = (activeConnection != null) && activeConnection.IsConnected;

            if (isOnline)
            {
                // Networkのタイプを表示
                NetworkInfo.State activeState = activeConnection.GetState();
                System.Diagnostics.Debug.WriteLine(activeConnection.TypeName);

                // 全接続を取得して、それぞれの接続状態を確認
                // GetAllNetworks()は5.0以上でそれ以前はgetAllNetworkInfo()を使用
                if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
                {
                    Network[] allNetworks = _connectivityManager.GetAllNetworks();
                    foreach (var network in allNetworks)
                    {
                        
                        NetworkInfo info = _connectivityManager.GetNetworkInfo(network);
                        var connect = info.IsConnectedOrConnecting ? "cennected" : "disconnected";
                        System.Diagnostics.Debug.WriteLine($"{info.TypeName} is {connect}");

                        if (info.Type == ConnectivityType.Wifi)
                        {
                            Authenticator.SetDefault(new BasicAuthenticator(leonetId, leonetPassword));
                            var leonetUrlString = "http://#GATEWAY#/login.cgi".Replace("#GATEWAY#", GetDefaultGateway());
                            var leonetUrl = new URL(leonetUrlString);
                            Task.Run(() =>
                            {
                                network.OpenConnection(leonetUrl);
                            });
                            
                        }

                    }
                }
                else
                {
                    NetworkInfo[] allNetworks = _connectivityManager.GetAllNetworkInfo();
                    foreach (var item in allNetworks)
                    {
                        var connect = item.IsConnectedOrConnecting ? "cennected" : "disconnected";
                        System.Diagnostics.Debug.WriteLine($"{item.TypeName} is {connect}");
                    }
                }
            }
        }

        public void SetMobileDataEnabled(bool enabled)
        {

            Android.Provider.Settings.Global.PutInt(_contextResolver, Android.Provider.Settings.Global.AirplaneModeOn, 1);
            /*
            try
            {

                Java.Lang.Class telephonyClass = Java.Lang.Class.ForName(_telephonyManager.Class.Name);
                Method setMobileDataEnabledMethod = telephonyClass.GetDeclaredMethod("setDataEnabled", Java.Lang.Boolean.Type);

                if (null != setMobileDataEnabledMethod)
                {
                    setMobileDataEnabledMethod.Invoke(_telephonyManager, enabled);
                }
            }
            catch (Exception ex)
            {
                
            }
            
            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
            {
                Debug.WriteLine("Device does not support mobile data toggling.");
                return;
            }

            try
            {
                if (Build.VERSION.SdkInt <= Android.OS.BuildVersionCodes.KitkatWatch
                    && Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Gingerbread)
                {
                    Java.Lang.Class conmanClass = Java.Lang.Class.ForName(_connectivityManager.Class.Name);
                    Java.Lang.Reflect.Field iConnectivityManagerField = conmanClass.GetDeclaredField("mService");
                    iConnectivityManagerField.Accessible = true;
                    Java.Lang.Object iConnectivityManager = iConnectivityManagerField.Get(_connectivityManager);
                    Java.Lang.Class iConnectivityManagerClass = Java.Lang.Class.ForName(iConnectivityManager.Class.Name);
                    Java.Lang.Reflect.Method setMobileDataEnabledMethod = iConnectivityManagerClass.GetDeclaredMethod("setMobileDataEnabled", Java.Lang.Boolean.Type);
                    setMobileDataEnabledMethod.Accessible = true;

                    setMobileDataEnabledMethod.Invoke(iConnectivityManager, enabled);
                }
                
                if (Build.VERSION.SdkInt < Android.OS.BuildVersionCodes.Gingerbread)
                {

                    TelephonyManager tm = (TelephonyManager)GetSystemService(Context.TelephonyService);

                    Java.Lang.Class telephonyClass = Java.Lang.Class.ForName(tm.Class.Name);
                    Java.Lang.Reflect.Method getITelephonyMethod = telephonyClass.GetDeclaredMethod("getITelephony");
                    getITelephonyMethod.Accessible = true;

                    Java.Lang.Object stub = getITelephonyMethod.Invoke(tm);
                    Java.Lang.Class ITelephonyClass = Java.Lang.Class.ForName(stub.Class.Name);

                    Java.Lang.Reflect.Method dataConnSwitchMethod = null;
                    if (enabled)
                    {
                        dataConnSwitchMethod = ITelephonyClass
                            .GetDeclaredMethod("disableDataConnectivity");
                    }
                    else
                    {
                        dataConnSwitchMethod = ITelephonyClass
                            .GetDeclaredMethod("enableDataConnectivity");
                    }

                    dataConnSwitchMethod.Accessible = true;
                    dataConnSwitchMethod.Invoke(stub);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Device does not support mobile data toggling.");
            }*/
        }
    }
}