using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace wdw_mobile_client
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LecturePage : ContentPage
    {
        private HttpClient httpClient;
        private Student student;
        private Lecture lecture;

		public LecturePage (HttpClient client, Student stud, Lecture lect)
		{
			InitializeComponent ();
            httpClient = client;
            student = stud;
            lecture = lect;
            label.Text = $"{lecture.name}.";

            if(lecture.isSigned == null)
            {
                enroll.IsEnabled = true;
                disenroll.IsEnabled = false;
            }
            else if(lecture.isSigned == true)
            {
                enroll.IsEnabled = false;
                disenroll.IsEnabled = true;
            }
            else
            {
                enroll.IsEnabled = false;
                disenroll.IsEnabled = false;
            }
        }

        private async void Enroll_Pressed(object sender, EventArgs e)
        {
            try
            {
                HttpResponseMessage response;
                var jsonString = $"{{ \"idUser\": {student.id}, \"idLecture\": {lecture.id}, \"idEnrollment\": 1 }}";
                var stringContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                using (response = await httpClient.PostAsync("http://wdw.azurewebsites.net/lectures/subscribe", stringContent))
                {
                    response.EnsureSuccessStatusCode();
                }
                enroll.IsEnabled = false;
                disenroll.IsEnabled = true;
                Console.WriteLine("In queue!");
                await LoginPage.page.PopAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("No connection! \n" + ex);
                await DisplayAlert("Powiadomienie", "Brak połączenia z internetem.", "OK");
            }
        }

        private async void Disenroll_Pressed(object sender, EventArgs e)
        {
            try
            {
                HttpResponseMessage response;
                var jsonString = $"{{ \"idUser\": {student.id}, \"idLecture\": {lecture.id} }}";
                var stringContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                Console.WriteLine("1!");
                using (response = await httpClient.PostAsync("http://wdw.azurewebsites.net/lectures/unsubscribe", stringContent))
                {
                    Console.WriteLine("2!");
                    response.EnsureSuccessStatusCode();
                    Console.WriteLine("3!");
                }
                enroll.IsEnabled = true;
                disenroll.IsEnabled = false;
                Console.WriteLine("Unsubscribed!");
                await LoginPage.page.PopAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("No connection! \n" + ex);
                await DisplayAlert("Powiadomienie", "Brak połączenia z internetem.", "OK");
            }
        }
    }
}