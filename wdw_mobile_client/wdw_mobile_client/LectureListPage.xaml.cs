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
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Navigation.PopModalAsync();
            activityIndicator.IsRunning = true;

            listView = LecturesList;

            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization  = new AuthenticationHeaderValue("Bearer", student.token);

            downloadData();
        }

        public async void downloadData()
        {
            HttpResponseMessage json = await httpClient.GetAsync("http://apiwdw.azurewebsites.net/lectures");
            string lecturesJson = await json.Content.ReadAsStringAsync();
            lectures = JsonConvert.DeserializeObject<List<Lecture>>(lecturesJson);
            listView.ItemsSource = lectures;
            //downloadIndicator.IsRunning = false;
        }

        private async void LecturesList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Lecture lecture = (Lecture) e.Item;
            //await Navigation.PushAsync(new LecturePage(lecture));
            await DisplayAlert("Test", lecture.name, "OK");
        }
    }
}