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
            label.Text = $"You can't sign - {lecture.isSigned}.";

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
                using (response = await httpClient.PostAsync("http://apiwdw.azurewebsites.net/lectures/subscribe", stringContent))
                {
                    response.EnsureSuccessStatusCode();
                }
                lecture.isSigned = true;
                enroll.IsEnabled = false;
                disenroll.IsEnabled = true;
                Console.WriteLine("In queue!");
            }
            catch (Exception exc)
            {
                Console.WriteLine("Can't sign! \n" + exc);
            }
        }

        private async void Disenroll_Pressed(object sender, EventArgs e)
        {
            try
            {
                HttpResponseMessage response;
                var jsonString = $"{{ \"idUser\": {student.id}, \"idLecture\": {lecture.id} }}";
                var stringContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
                using (response = await httpClient.PostAsync("http://apiwdw.azurewebsites.net/lectures/unsubscribe", stringContent))
                {
                    response.EnsureSuccessStatusCode();
                }
                lecture.isSigned = null;
                enroll.IsEnabled = true;
                disenroll.IsEnabled = false;
                Console.WriteLine("Unsubscribed!");
            }
            catch (Exception exc)
            {
                Console.WriteLine("Can't unsubscribe! \n" + exc);
            }
        }
    }
}