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
        private bool? isConnected = null;
        private bool loggedIn = false;
        private HttpClient _client;
        public User student;
        private ActivityIndicator activityIndicator;
        public static NavigationPage page;
        public DateTimeOffset startTime;
        public string jsonString;
        Task connection;

        public LoginPage ()
		{
			InitializeComponent ();
            _client = new HttpClient();
            activityIndicator = indicator;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            connection = hasConnection();
        }

        private async void LoginBtn_Clicked(object sender, EventArgs e)
        {
            loginBtn.IsEnabled = false;
            string id = student_id.Text;
            string pass = password.Text;
            //string id = "developer";
            //string pass = "developer";

            jsonString = $"{{ \"username\":\"{id}\", \"password\":\"{pass}\" }}";

            DateTimeOffset startTime = DateTimeOffset.Now;

            while (DateTimeOffset.Now.Subtract(startTime).TotalMilliseconds < 30000 && isConnected != false)
            {
                await connection;
                if (connection.IsCompleted)
                {
                    student_id.IsEnabled = false;
                    password.IsEnabled = false;
                    indicator.IsRunning = true;
                    await getToken(jsonString);
                    indicator.IsRunning = false;
                    if (loggedIn)
                    {
                        page = new NavigationPage(new LectureListPage(student, jsonString));
                        App.Current.MainPage = page;
                        return;
                    }
                }
            }
            loginBtn.IsEnabled = true;
            await DisplayAlert("Powiadomienie", "Brak połączenia z internetem.", "OK");
        }

        public async Task hasConnection()
        {
            try {
                HttpResponseMessage response;
                using (response = await _client.GetAsync("http://wdw.azurewebsites.net/"))
                {
                    response.EnsureSuccessStatusCode();
                }
                isConnected = true;
                Console.WriteLine("Connected!");
            }
            catch(Exception e)
            {
                isConnected = false;
                Console.WriteLine("No connection! \n" + e);
            }
        }

        public async Task getToken(dynamic jsonString)
        {
            try
            {
                var stringContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                HttpResponseMessage response;
                using (response = await _client.PostAsync("http://wdw.azurewebsites.net/login_check", stringContent))
                {
                    response.EnsureSuccessStatusCode();
                    string responseJson = await response.Content.ReadAsStringAsync();
                    student = JsonConvert.DeserializeObject<User>(responseJson);
                }
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
            catch(JsonReaderException e)
            {
                Console.WriteLine("Json error! \n" + e);
            }
        }

        private void NextEntry(object sender, EventArgs e)
        {
            password.Focus();
        }
    }
}