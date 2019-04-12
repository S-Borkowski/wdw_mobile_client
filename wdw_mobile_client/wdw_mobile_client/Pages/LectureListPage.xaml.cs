using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace wdw_mobile_client
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LectureListPage : ContentPage
	{
        private ListView listView;
        private ActivityIndicator activityIndicator;
        private HttpClient httpClient;
        private User user;
        public static Enrollment enrollment;
        public static Student student;

        public LectureListPage(User usr)
        {
            InitializeComponent();
            user = usr;
            enrollment = user.enrollments[0];
            activityIndicator = downloadIndicator;
            listView = LecturesList;
            Title = $"Dostępne ECTS: {enrollment.availableEcts}";

            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.token);

            activityIndicator.IsRunning = true;
            downloadLectures();

            listView.RefreshCommand = new Command(() => {
                downloadLectures();
            });
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        public async void downloadLectures()
        {
            try
            {
                HttpResponseMessage json;
                using (json = await httpClient.GetAsync($"http://apiwdw.azurewebsites.net{user.user}"))
                {
                    json.EnsureSuccessStatusCode();
                    string studentJson = await json.Content.ReadAsStringAsync();
                    student = JsonConvert.DeserializeObject<Student>(studentJson);
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Connection to api error! \n" + e);
            }
            catch (JsonReaderException e)
            {
                Console.WriteLine("Json error! \n" + e);
            }

            listView.ItemsSource = enrollment.lectures;
            listView.IsRefreshing = false;
            downloadIndicator.IsRunning = false;
        }

        private async void LecturesList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Lecture lecture = (Lecture) e.Item;
            await LoginPage.page.PushAsync(new LecturePage(lecture));
        }

        private void LogoutBtn_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new LoginPage();
        }
    }
}