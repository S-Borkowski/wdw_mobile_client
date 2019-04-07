using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        private List<Lecture> lectures;
        private ActivityIndicator activityIndicator;
        private HttpClient httpClient;
        private Student student;

        public LectureListPage(Student stud)
        {
            InitializeComponent();
            student = stud;
            activityIndicator = downloadIndicator;

            listView = LecturesList;

            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", student.token);

            activityIndicator.IsRunning = true;
            downloadData();

            listView.RefreshCommand = new Command(() => {
                downloadData();
            });
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        public async void downloadData()
        {
            HttpResponseMessage json = await httpClient.GetAsync("http://apiwdw.azurewebsites.net/lectures");
            string lecturesJson = await json.Content.ReadAsStringAsync();
            lectures = JsonConvert.DeserializeObject<List<Lecture>>(lecturesJson);
            listView.ItemsSource = lectures;
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