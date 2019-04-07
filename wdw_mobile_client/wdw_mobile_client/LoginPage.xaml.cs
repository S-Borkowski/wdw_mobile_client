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
        private bool loggedIn = false;
        private HttpClient _client;
        public Student student;
        private ActivityIndicator activityIndicator;
        public static NavigationPage page;

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
            string id = student_id.Text;
            string pass = password.Text;
            //string id = "developer";
            //string pass = "developer";

            string jsonString = $"{{ \"username\":\"{id}\", \"password\":\"{pass}\" }}";

            if (isConnected) {
                loginBtn.IsEnabled = false;
                student_id.IsEnabled = false;
                password.IsEnabled = false;
                await getToken(jsonString);
                if (loggedIn)
                {
                    page = new NavigationPage(new LectureListPage(student));
                    App.Current.MainPage = page;
                }
            }
            else
            {
                await DisplayAlert("Powiadomienie", "Brak połączenia z internetem.", "OK");
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
            indicator.IsRunning = true;

            try
            {
                var stringContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _client.PostAsync("http://apiwdw.azurewebsites.net/login_check", stringContent);
                response.EnsureSuccessStatusCode();
                string responseJson = await response.Content.ReadAsStringAsync();
                student = JsonConvert.DeserializeObject<Student>(responseJson);
                loggedIn = true;
                //Console.WriteLine("This is the token: " + student.token);
            }
            catch(HttpRequestException e)
            {
                await DisplayAlert("Błąd", "Niepoprawny nr. indeksu lub hasło.", "OK");
                Console.WriteLine("Wrong id or password! \n" + e);
                loginBtn.IsEnabled = true;
                student_id.IsEnabled = true;
                password.IsEnabled = true;
            }

            indicator.IsRunning = false;
        }

        private void NextEntry(object sender, EventArgs e)
        {
            password.Focus();
        }
    }
}