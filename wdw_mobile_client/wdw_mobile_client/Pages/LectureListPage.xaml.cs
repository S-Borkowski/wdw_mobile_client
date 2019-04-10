using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace wdw_mobile_client
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LectureListPage : ContentPage
	{
        private ListView listView;
        private List<Lecture> lectures;
        private ActivityIndicator activityIndicator;
        private HttpClient httpClient;
        private User user;
        private Specialisation specialisation;
        public static Student student;

        public LectureListPage(User usr)
        {
            InitializeComponent();
            user = usr;
            activityIndicator = downloadIndicator;

            listView = LecturesList;

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
                await downloadData();

                HttpResponseMessage json;
                using (json = await httpClient.GetAsync("http://apiwdw.azurewebsites.net/lectures"))
                {
                    json.EnsureSuccessStatusCode();
                    string lecturesJson = await json.Content.ReadAsStringAsync();
                    lectures = JsonConvert.DeserializeObject<List<Lecture>>(lecturesJson);
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

            listView.ItemsSource = lectures;
            Title = $"Dostępne ECTS: {specialisation.ectsLimit}";
            listView.IsRefreshing = false;
            downloadIndicator.IsRunning = false;
        }

        public async Task downloadData()
        {
            try
            {
                HttpResponseMessage json;
                using (json = await httpClient.GetAsync($"http://apiwdw.azurewebsites.net/users/{user.idUser}"))
                {
                    json.EnsureSuccessStatusCode();
                    string studentJson = await json.Content.ReadAsStringAsync();
                    student = JsonConvert.DeserializeObject<Student>(studentJson);
                }

                using (json = await httpClient.GetAsync($"http://apiwdw.azurewebsites.net/{student.specialisations[0]}"))
                {
                    json.EnsureSuccessStatusCode();
                    string specialisationJson = await json.Content.ReadAsStringAsync();
                    specialisation = JsonConvert.DeserializeObject<Specialisation>(specialisationJson);
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