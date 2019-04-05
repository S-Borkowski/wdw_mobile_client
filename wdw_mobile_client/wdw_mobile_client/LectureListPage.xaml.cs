using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace wdw_mobile_client
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LectureListPage : ContentPage
	{
        private ListView listView;
        private List<Lecture> lectures;
        private ActivityIndicator activityIndicator;
        private WebClient webClient;
        private Uri url;
        private string[] list;

        public LectureListPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            listView = LecturesList;
            activityIndicator = indicator;

            list = new string[25];

            wait();

            webClient = new WebClient();
            /*
            url = new Uri("https://support.oneskyapp.com/hc/en-us/article_attachments/202761627/example_1.json");

            webClient.DownloadDataAsync(url);
            webClient.DownloadDataCompleted += WebClient_DownloadDataCompleted;
        }


        private void WebClient_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            string json = Encoding.UTF8.GetString(e.Result);
            lectures = JsonConvert.DeserializeObject<List<Lecture>>(json);
            indicator.IsRunning = false;
            */
        }
        public async void wait()
        {
            list[0] = "pierwszy";
            await Task.Delay(2000);
            listView.ItemsSource = list;
            indicator.IsRunning = false;
        }

        private void LecturesList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            string a = e.ToString();
            DisplayAlert("Alert", a, "OK");
        }
    }
    public class Cell
    {
        public int id { get; set; }
        public string name { get; set; }

        public string GETNAME
        {
            get { return String.Format("{0}", name); }
        }
        public string GETID
        {
            get { return String.Format("{0}", id); }
        }

    }
}