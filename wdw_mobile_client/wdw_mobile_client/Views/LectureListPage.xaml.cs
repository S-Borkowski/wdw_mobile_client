using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
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
        private Lecture[] lectureList;
        public static Enrollment enrollment;
        public static Student student;
        public static GroupLectures enrolledGroup = new GroupLectures() { longName = "Zapisane", shortName = "Z" };
        public static GroupLectures availableGroup = new GroupLectures() { longName = "Dostępne", shortName = "D" };
        public static GroupLectures unavailableGroup = new GroupLectures() { longName = "Niedostępne", shortName = "N" };
        public static GroupLectures fullGroup = new GroupLectures() { longName = "Pełne", shortName = "P" };
        private ObservableCollection<GroupLectures> grouped { get; set; }

        public LectureListPage(User usr)
        {
            InitializeComponent();
            user = usr;
            enrollment = user.enrollments[0];
            lectureList = enrollment.lectures;
            activityIndicator = downloadIndicator;
            listView = LecturesList;
            Title = $"Dostępne ECTS: {enrollment.availableEcts}";

            grouped = new ObservableCollection<GroupLectures>();

            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.token);

            activityIndicator.IsRunning = true;
            downloadData();

            listView.RefreshCommand = new Command(() => {
                downloadLectures();
            });
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        public async void downloadData()
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

            await groupLectures();
            downloadIndicator.IsRunning = false;
        }

        private async void downloadLectures()
        {
            try
            {
                HttpResponseMessage json;
                using (json = await httpClient.GetAsync("http://apiwdw.azurewebsites.net/enrollments/1/lectures"))
                {
                    json.EnsureSuccessStatusCode();
                    string lecturesJson = await json.Content.ReadAsStringAsync();
                    lectureList = JsonConvert.DeserializeObject<Lecture[]>(lecturesJson);
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

            await groupLectures();
            listView.IsRefreshing = false;
        }

        private async Task groupLectures()
        {
            grouped.Clear(); enrolledGroup.Clear(); availableGroup.Clear(); unavailableGroup.Clear(); fullGroup.Clear();
            for (int i = 0; i < lectureList.Length; i++)
            {
                if (Array.IndexOf(student.lectures, $"/lectures/{lectureList[i].id}") > -1)
                {
                    enrolledGroup.Add(lectureList[i]);
                }
                else if (lectureList[i].freeSlots != 0)
                {
                    availableGroup.Add(lectureList[i]);
                }
                else if (enrollment.availableEcts == 0)
                {
                    unavailableGroup.Add(lectureList[i]);
                }
                else
                {
                    fullGroup.Add(lectureList[i]);
                }
            }

            grouped.Add(enrolledGroup); grouped.Add(availableGroup); grouped.Add(unavailableGroup); grouped.Add(fullGroup);
            listView.ItemsSource = grouped;
        }

        private async void LecturesList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Lecture lecture = (Lecture) e.Item;
            await LoginPage.page.PushAsync(new LecturePage(httpClient, student, lecture));
        }

        private void LogoutBtn_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new LoginPage();
        }
    }
}