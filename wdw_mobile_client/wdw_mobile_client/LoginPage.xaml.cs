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

            if (isConnected) {
                loginBtn.IsEnabled = false;
                getToken(jsonString);
                App.Current.MainPage = new LectureListPage(student);
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
                Console.WriteLine("No connection! \n" + e);
            }
        }

        public async Task getToken(dynamic jsonString)
        {
            var stringContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            dynamic response = _client.PostAsync("http://apiwdw.azurewebsites.net/login_check", stringContent).Result;
            Console.WriteLine("This is the response: " + response.Content.ReadAsStringAsync().Result);
            dynamic responseJson = response.Content.ReadAsStringAsync().Result;
            student = JsonConvert.DeserializeObject<Student>(responseJson);
            Console.WriteLine("This is the token: " + student.token);
        }
    }
}