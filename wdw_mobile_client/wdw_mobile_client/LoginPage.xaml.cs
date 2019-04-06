using Newtonsoft.Json;
using System;
using System.Net.Http;
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
        public Student student;
        private ActivityIndicator activityIndicator;

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        public LoginPage ()
		{
			InitializeComponent ();
            _client = new HttpClient();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await hasConnection();

            activityIndicator = indicator;
        }

        private async void LoginBtn_Clicked(object sender, EventArgs e)
        {
            indicator.IsRunning = true;
            string id = "developer";//student_id.Text;
            string pass = "developer";//password.Text;

            string jsonString = $"{{ \"username\":\"{id}\", \"password\":\"{pass}\" }}";

            if (isConnected) {
                loginBtn.IsEnabled = false;
                student_id.IsEnabled = false;
                password.IsEnabled = false;
                await getToken(jsonString);
                await Navigation.PushModalAsync(new LectureListPage(student));
            }
            else
            {
                await DisplayAlert("Alert", "No connection to internet", "OK");
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
                Console.WriteLine("No connection! \n" + e);
            }
        }

        public async Task getToken(dynamic jsonString)
        {
            var stringContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync("http://apiwdw.azurewebsites.net/login_check", stringContent);
            string responseJson = await response.Content.ReadAsStringAsync();
            student = JsonConvert.DeserializeObject<Student>(responseJson);
            //Console.WriteLine("This is the token: " + student.token);
        }
    }
}