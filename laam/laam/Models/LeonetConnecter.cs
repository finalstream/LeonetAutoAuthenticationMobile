using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Prism.Logging;

namespace laam.Models
{
    public class LeonetConnecter
    {
        public event EventHandler Connected;

        private readonly IWifiService _wifiService;

        public LeonetConnecter(IWifiService wifiService)
        {
            _wifiService = wifiService;
        }

        public async Task ConnectAsync(string leonetId, string leonetPassword)
        {
            //_wifiService.ConnectAsync(leonetId, leonetPassword);

            //_wifiService.SetMobileDataEnabled(false);
            
            var defaultGatewayAddress = _wifiService.GetDefaultGateway();

            var url = "http://#GATEWAY#/login.cgi".Replace("#GATEWAY#", defaultGatewayAddress);

            using (var handler = new HttpClientHandler { Credentials = new NetworkCredential(leonetId, leonetPassword),  })
            using (var client = new HttpClient(handler))
            {
                client.Timeout = TimeSpan.FromSeconds(5);
                var result = await client.GetAsync(url);

                if (result.IsSuccessStatusCode) OnConnected();
            }
            /*
            _wifiService.SetMobileDataEnabled(true);
            //_wifiService.ConnectAsync();

            
            //HttpWebRequestの作成
                var webreq = 
                System.Net.WebRequest.Create(url.Replace("#GATEWAY#", defaultGatewayAddress));

            //認証の設定
            client.
            webreq.Credentials =
                new System.Net.NetworkCredential(leonetId, leonetPassword);

            // TODO: タイムアウト時間設定
            

            System.Net.HttpWebResponse webres = null;
            try
            {
                //HttpWebResponseの取得
                webreq.get
                webres =
                    (System.Net.HttpWebResponse)webreq.GetResponse();

                // TODO:成功メッセージ
            }
            catch (Exception ex)
            {
               // TODO:エラー処理
            }


            if (webres != null && webres.StatusCode == HttpStatusCode.OK)
            {

                Stopwatch sw = new Stopwatch();
                sw.Start();
                while (sw.ElapsedMilliseconds < Properties.Settings.Default.ViewMillisecond)
                {
                    // 何らかの処理
                    System.Threading.Thread.Sleep(10);

                    // メッセージ・キューにあるWindowsメッセージをすべて処理する
                    Application.DoEvents();
                }
                exit();
            }*/

        }

        protected virtual void OnConnected()
        {
            Connected?.Invoke(this, EventArgs.Empty);
        }
    }
}
