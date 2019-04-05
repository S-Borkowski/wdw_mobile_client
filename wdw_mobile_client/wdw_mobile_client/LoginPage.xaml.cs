using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace wdw_mobile_client
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
        private bool isConnected = false;
        private HttpClient _client;


        public LoginPage ()
		{
			InitializeComponent ();

            _client = new HttpClient();

            hasConnection();
        }

        private void LoginBtn_Clicked(object sender, EventArgs e)
        {
            string id = "developer";//student_id.Text;
            string pass = "developer";//password.Text;

            string jsonString = $"{{ \"username\":\"{id}\", \"password\":\"{pass}\" }}";
            dynamic json = JsonConvert.DeserializeObject(jsonString);

            if (isConnected) {
                loginBtn.IsEnabled = false;
                getToken(jsonString);
                App.Current.MainPage = new LectureListPage();
                loginBtn.IsEnabled = true;
            }
            else
            {
                DisplayAlert("Alert", "No connection to internet", "OK");
            }
        }

        public async Task hasConnection()
        {
            try {
                HttpResponseMessage response = await _client.GetAsync("http://apiwdw.azurewebsites.net/");
                response.EnsureSuccessStatusCode();
                isConnected = true;
                Console.WriteLine("Connected!");
            }
            catch(HttpRequestException e)
            {
                Console.WriteLine("No connection!");
            }
        }

        public async Task getToken(dynamic jsonString)
        {
            var stringContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            dynamic response = _client.PostAsync("http://apiwdw.azurewebsites.net/login_check", stringContent).Result;
            Console.WriteLine("Those are the lectures1: " + response);
            dynamic lecturesString = JsonConvert.DeserializeObject(response);
            Console.WriteLine("Those are the lectures2: ");
            Console.WriteLine(lecturesString);
        }
    }
}