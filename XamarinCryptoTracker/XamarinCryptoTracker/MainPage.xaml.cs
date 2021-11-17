using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinCryptoTracker.Model;

namespace XamarinCryptoTracker
{
    public partial class MainPage : ContentPage
    {
        private string apiKey = "BA955CE6-B99D-4B92-905D-D4E3195C8F12";
        private string baseImageUrl = "https://s3.eu-central-1.amazonaws.com/bbxt-static-icons/type-id/png_64/";
        private List<Coin> coins;

        public MainPage()
        {
            InitializeComponent();

            ServicePointManager.ServerCertificateValidationCallback +=
            (sender, certificate, chain, sslPolicyErrors) => true;

            var client = new RestClient("https://rest.coinapi.io/v1/assets?filter_asset_id=BTC;ETH;XMR;LTC;DOGE");
            var request = new RestRequest(Method.GET);
            request.AddHeader("X-CoinAPI-Key", apiKey);

            var response = client.Execute(request);

            coins = JsonConvert.DeserializeObject<List<Coin>>(response.Content);

            foreach (var c in coins)
            {
                if (!String.IsNullOrEmpty(c.Id_Icon))
                    c.Icon_Url = baseImageUrl + c.Id_Icon.Replace("-", "") + ".png";
                else
                    c.Icon_Url = "";
            }

            coinListView.ItemsSource = coins;
        }

        async void RefreshView_Refreshing(System.Object sender, System.EventArgs e)
        {
            await Task.Delay(3000);
            myRefreshView.IsRefreshing = false;
        }
    }
}
