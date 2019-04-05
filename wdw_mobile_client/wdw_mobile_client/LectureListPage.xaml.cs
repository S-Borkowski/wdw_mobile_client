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
        private Uri url;
        private Student student;

        public LectureListPage(Student stud)
        {
            InitializeComponent();
            student = stud;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            listView = LecturesList;
            activityIndicator = indicator;

            httpClient = new HttpClient();
            url = new Uri("http://apiwdw.azurewebsites.net/lectures");
            httpClient.DefaultRequestHeaders.Authorization  = new AuthenticationHeaderValue("Bearer", student.token);

            downloadData();
        }

        public async void downloadData()
        {
            dynamic json = httpClient.GetAsync(url).Result;
            Console.WriteLine(json.Content.ReadAsStringAsync().Result);
            dynamic lecturesJson = json.Content.ReadAsStringAsync().Result;
            lectures = JsonConvert.DeserializeObject<List<Lecture>>(lecturesJson);
            listView.ItemsSource = lectures;
            indicator.IsRunning = false;
        }

        private void ViewCell_Tapped(object sender, EventArgs e)
        {
            DisplayAlert("Test", e.ToString(), "OK");
        }
    }
}